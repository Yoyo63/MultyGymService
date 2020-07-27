using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multigyms.Models
{
    public class EGym
    {
        public int idgym { get; set; }
        public string nombregym { get; set; }
        public string img { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public int creditos { get; set; }
        public string horariolv { get; set; }
        public string horarios { get; set; }
        public string horariod { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string distancia { get; set; }
        public string reviewaverage { get; set; }
        public string reviewcount { get; set; }
        public List<EDisciplina> disciplinas { get; set; }
        public List<EServicio> servicios { get; set; }
    }
}