using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multigyms.Models
{
    public class EPlan
    {
        public int idplan { get; set; }
        public string plan { get; set; }
        public string descripcion { get; set; }
        public int precio { get; set; }
        public int creditos { get; set; }
        public bool activo { get; set; }
    }
}