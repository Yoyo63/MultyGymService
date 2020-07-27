using PushSharp.Apple;
using PushSharp.Google;
using NotificacionPush.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificacionPush
{
    public class Enviador
    {
        public List<Token> Tokens { get; set; }
        public List<string> TokensSended { get; set; }
        public Alerta alerta { get; set; }
        public string CertAppleURL { get; set; }
        public string CertApplePass { get; set; }
        public string GCMKeyCliente { get; set; }
        public string GCMKeyEmpresa { get; set; }
        public string GCMKeyConductor { get; set; }
        public int SegundosReintento { get; set; }
        public int CantidadIntentos { get; set; }
        public bool ApplePushProduccion { get; set; }

        private GcmServiceBroker gcmPush = null;
        private ApnsServiceBroker apnsPush = null;

        private int ContadorEnviados = 0;
        private int ContadorNoEnviados = 0;
        private int ContadorIntentos = 0;

        public Enviador(
           List<Token> _Tokens,
       Alerta _alerta,
       string _CertAppleURL,
       string _CertApplePass,
       string _GCMKeyCliente,
       string _GCMKeyEmpresa,
       string _GCMKeyConductor,
       int _SegundosReintento,

       int _CantidadIntentos,
           bool _ApplePushProduccion = false
           )
        {
            this.Tokens = _Tokens;
            this.alerta = _alerta;
            this.CertAppleURL = _CertAppleURL;
            this.CertApplePass = _CertApplePass;
            this.GCMKeyCliente = _GCMKeyCliente;
            this.GCMKeyEmpresa = _GCMKeyEmpresa;
            this.GCMKeyConductor = _GCMKeyConductor;

            this.SegundosReintento = _SegundosReintento;
            this.CantidadIntentos = _CantidadIntentos;
            this.ApplePushProduccion = _ApplePushProduccion;
            this.TokensSended = new List<string>();
        }
        public void DoWork()
        {

            while (!_shouldStop)
            {
                ContadorIntentos++;
                try
                {
                    byte[] appleCert = null;
                    if (!File.Exists(CertAppleURL))
                    {
                        EscribirEnLog("hubo Error : No existe el Certificado iOS");
                    }
                    else
                    {
                        appleCert = File.ReadAllBytes(CertAppleURL);
                        EscribirEnLog("certificado encontrado:" + "iOS:" + CertAppleURL + ",  Pass:" + CertApplePass + ", GCMKeyCliente: " + GCMKeyCliente + ", GCMKeyEmpresa: " + GCMKeyEmpresa + ", GCMKeyConductor: " + GCMKeyConductor + ", Alerta " + alerta);
                    
                    }
                    
                    foreach (var token in Tokens)
                    {
                        if (!TokensSended.Contains(token.TokenID))
                        {
                            try
                            {
                                string FBSenderAuthToken = "";
                                switch (token.TipoAplicacion)
                                {
                                    case Aplicacion.Usuario: FBSenderAuthToken = GCMKeyCliente;
                                        break;
                                    case Aplicacion.Conductor:
                                        FBSenderAuthToken = GCMKeyConductor;
                                        break;
                                    case Aplicacion.Administrador:
                                        FBSenderAuthToken = GCMKeyCliente;
                                        break;
                                    case Aplicacion.Encargado:
                                        FBSenderAuthToken = GCMKeyEmpresa;
                                        break;
                                    default:
                                        FBSenderAuthToken = GCMKeyCliente;
                                        break;
                                }
                                switch (token.TipoDispositivo)
                                {
                                    case Dispositivo.Apple:
                                        if(appleCert!= null)
                                        {
                                            ApnsConfiguration.ApnsServerEnvironment ambiente = ApplePushProduccion ? ApnsConfiguration.ApnsServerEnvironment.Production : ApnsConfiguration.ApnsServerEnvironment.Sandbox;
                                            var configApns = new ApnsConfiguration(ambiente, appleCert, CertApplePass, false);

                                            apnsPush = new ApnsServiceBroker(configApns);
                                            apnsPush.OnNotificationFailed += apnsPush_OnNotificationFailed;
                                            apnsPush.OnNotificationSucceeded += apnsPush_OnNotificationSucceeded;
                                            apnsPush.Start();
                                            apnsPush.QueueNotification((ApnsNotification)alerta.Notificacion(token));
                                            apnsPush.Stop();
                                        }                                        
                                        break;
                                    case Dispositivo.Android:
                                        var configGCM = new GcmConfiguration(FBSenderAuthToken);
                                        
                                        configGCM.OverrideUrl("https://fcm.googleapis.com/fcm/send");
                                        configGCM.GcmUrl = "https://fcm.googleapis.com/fcm/send";
                                        gcmPush = new GcmServiceBroker(configGCM);
                                        
                                        gcmPush.OnNotificationSucceeded += gcmPush_OnNotificationSucceeded;
                                        gcmPush.OnNotificationFailed += gcmPush_OnNotificationFailed;
                                        gcmPush.Start();
                                        gcmPush.QueueNotification((GcmNotification)alerta.Notificacion(token));
                                        
                                        gcmPush.Stop();
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                EscribirEnLog("hubo Error : " + ex.Message);
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    if (todoCorrecto() || (ContadorIntentos >= CantidadIntentos))
                    {
                        this._shouldStop = true;
                    }
                    else
                    {
                        this.ContadorEnviados = 0;
                        this.ContadorNoEnviados = 0;
                        Thread.Sleep(SegundosReintento * 1000);
                    }
                }
                catch (Exception ex)
                {
                    EscribirEnLog("hubo Error : " + ex.Message);
                    Console.WriteLine(ex.Message);
                }
            }
        }

        void apnsPush_OnNotificationSucceeded(ApnsNotification notification)
        {
            string token = "";
            try
            {
                token = notification.DeviceToken;
                TokensSended.Add(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            ContadorEnviados++;
        }

        void apnsPush_OnNotificationFailed(ApnsNotification notification, AggregateException exception)
        {
            ContadorNoEnviados++;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("================================================================================");
            sb.AppendLine("Notificacion: " + notification.ToString());
            sb.AppendLine("Error: " + exception.Message);

            EscribirEnLog(sb.ToString());
        }

        void gcmPush_OnNotificationFailed(GcmNotification notification, AggregateException exception)
        {
            ContadorNoEnviados++;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("================================================================================");
            sb.AppendLine("Notificacion: " + notification.ToString());
            sb.AppendLine("Error: " + exception.Message);

            EscribirEnLog(sb.ToString());
        }

        private void gcmPush_OnNotificationSucceeded(GcmNotification notification)
        {
            string token = "";
            try
            {
                token = notification.RegistrationIds.First();
                TokensSended.Add(token);
            }
            catch
            {

            }
            ContadorEnviados++;
        }

        public void EscribirEnLog(string mensaje)
        {
            var logHab = ConfigurationManager.AppSettings["logHabilitado"];
            if (Convert.ToBoolean(logHab))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("\n" + mensaje);
                    var filePath = ConfigurationManager.AppSettings["fileLog"];
                    if (File.Exists(filePath)) {
                        File.AppendAllText(filePath, sb.ToString());
                    }                        
                    sb.Clear();
                }
                catch (Exception ex)
                {
                }
            }
        }

        bool todoCorrecto()
        {
            return this.ContadorEnviados == (this.Tokens.Count - this.TokensSended.Count);
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop;

    }
}
