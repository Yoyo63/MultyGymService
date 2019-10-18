using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace multigyms.Models
{
    [DataContract]
    public class RespuestaApi<T>
    {
        [DataMember]
        public string Titulo
        {
            get;
            set;
        }

        [DataMember]
        public int Codigo
        {
            get;
            set;
        }

        [DataMember]
        public string FechaHora
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssss"); }
            set {; }
        }

        [DataMember]
        public string Mensaje
        {
            get;
            set;
        }

        [DataMember]
        public T Data
        {
            get;
            set;
        }
        public static RespuestaApi<T> createRespuestaError(string mensaje)
        {
            RespuestaApi<T> respuesta = new RespuestaApi<T>();
            respuesta.Titulo = "";
            respuesta.Codigo = 1;
            respuesta.Mensaje = mensaje;
            return respuesta;
        }
        public static RespuestaApi<T> createRespuestaError(string mensaje, int code)
        {
            RespuestaApi<T> respuesta = new RespuestaApi<T>();
            respuesta.Titulo = "";
            respuesta.Codigo = code;
            respuesta.Mensaje = mensaje;
            return respuesta;
        }

        public static RespuestaApi<T> createRespuestaError(string mensaje, string titulo)
        {
            RespuestaApi<T> respuesta = new RespuestaApi<T>();
            respuesta.Titulo = titulo;
            respuesta.Codigo = 1;
            respuesta.Mensaje = mensaje;
            return respuesta;
        }
        public static RespuestaApi<T> createRespuestaErrorTocken(string titulo = "")
        {
            RespuestaApi<T> respuesta = new RespuestaApi<T>();
            respuesta.Titulo = titulo;
            respuesta.Codigo = 3;
            respuesta.Mensaje = "Otro dispositivo ha iniciado sesión con este usuario, inicie sesión nuevamente para continuar.";
            return respuesta;
        }

        public static RespuestaApi<T> createRespuestaError(int codigo, string mensaje, string titulo = "")
        {
            RespuestaApi<T> respuesta = new RespuestaApi<T>();
            respuesta.Titulo = titulo;
            respuesta.Codigo = codigo;
            respuesta.Mensaje = mensaje;
            return respuesta;
        }

        public static RespuestaApi<string> createRespuestaEncriptedSuccess(string mensaje, string data, string titulo = "")
        {

            RespuestaApi<string> respuesta = new RespuestaApi<string>();
            respuesta.Titulo = titulo;
            respuesta.Codigo = 0;
            respuesta.Mensaje = mensaje;
            respuesta.Data = data;
            return respuesta;
        }

        public static RespuestaApi<T> createrespuestasuccess(string mensaje, string titulo = "")
        {
            RespuestaApi<T> respuesta = new RespuestaApi<T>();
            respuesta.Titulo = titulo;
            respuesta.Codigo = 0;
            respuesta.Mensaje = mensaje;
            return respuesta;
        }

        public static RespuestaApi<T> createrespuestasuccess(string mensaje, T data, string titulo = "")
        {
            RespuestaApi<T> respuesta = new RespuestaApi<T>();
            respuesta.Titulo = titulo;
            respuesta.Codigo = 0;
            respuesta.Mensaje = mensaje;
            respuesta.Data = data;
            return respuesta;
        }

        public static RespuestaApi<T> createRespuestaSuccess(T data, string titulo = "")
        {
            RespuestaApi<T> respuesta = new RespuestaApi<T>();
            respuesta.Titulo = titulo;
            respuesta.Codigo = 0;
            respuesta.Mensaje = "";
            respuesta.Data = data;
            return respuesta;
        }
    }
}