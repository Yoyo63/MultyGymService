using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multigyms.Models
{
    public class EPersona
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public string celular { get; set; }
        public DateTime fechaingreso { get; set; }
        public int creditos { get; set; }
        public int idplan { get; set; }
        public string nplan { get; set; }
        public bool activo { get; set; }
        public string token { get; set; }
    }
}