﻿using NotificacionPush.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificacionPush
{
    public class NotificacionEnviador
    {
        public Alerta Alerta { get; set; }
        public List<Token> Tokens { get; set; }
        public int CantidadPorHilo { get; set; }
        public int SegundosReintento { get; set; }
        public int CantidadIntentos { get; set; }

        public string CertAppleURL { get; set; }
        public string CertApplePass { get; set; }
        public string GCMKeyCliente { get; set; }
        public string GCMKeyEmpresa { get; set; }
        public string GCMKeyConductor { get; set; }
        public bool ApplePushProduccion { get; set; }

        public NotificacionEnviador(
               Alerta _Alerta,
          List<Token> _Tokens,
          int _CantidadPorHilo,
          int _SegundosReintento,
          int _CantidadIntentos,
          string _CertAppleURL,
          string _CertApplePass,
          string _GCMKeyCliente,
          string _GCMKeyEmpresa,
          string _GCMKeyConductor,
          bool _ApplePushProduccion = false
              )
        {
            this.Alerta = _Alerta;
            this.Tokens = _Tokens;
            this.CantidadPorHilo = _CantidadPorHilo;
            this.SegundosReintento = _SegundosReintento;
            this.CantidadIntentos = _CantidadIntentos;
            this.CertAppleURL = _CertAppleURL;
            this.CertApplePass = _CertApplePass;
            this.GCMKeyCliente = _GCMKeyCliente;
            this.GCMKeyEmpresa = _GCMKeyEmpresa;
            this.GCMKeyConductor = _GCMKeyConductor;
            this.ApplePushProduccion = _ApplePushProduccion;
        }

        public void Enviar()
        {
            if (string.IsNullOrEmpty(CertAppleURL) || string.IsNullOrWhiteSpace(CertAppleURL))
            {
                throw new Exception("Se requiere url del certificado APPLE.");
            }
            if (string.IsNullOrEmpty(GCMKeyCliente) || string.IsNullOrWhiteSpace(GCMKeyCliente))
            {
                throw new Exception("Se requiere la llave del FBSender Cliente.");
            }
            if (string.IsNullOrEmpty(GCMKeyEmpresa) || string.IsNullOrWhiteSpace(GCMKeyEmpresa))
            {
                throw new Exception("Se requiere la llave del FBSender Empresa.");
            }
            if (string.IsNullOrEmpty(GCMKeyConductor) || string.IsNullOrWhiteSpace(GCMKeyConductor))
            {
                throw new Exception("Se requiere la llave del FBSender Conductor.");
            }

            if (Tokens.Count > 0)
            {
                foreach (var tokens in DividirTokens(Tokens))
                {
                    this.CrearHilo(tokens);
                }
            }
        }

        private List<List<Token>> DividirTokens(List<Token> source)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / CantidadPorHilo)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        private void CrearHilo(List<Token> tkns)
        {
            Enviador sender = new Enviador(tkns, Alerta, CertAppleURL, CertApplePass, GCMKeyCliente, GCMKeyEmpresa, GCMKeyConductor, SegundosReintento, CantidadIntentos, ApplePushProduccion);
            Thread senderThread = new Thread(sender.DoWork);
            senderThread.Start();
        }
    }
}
