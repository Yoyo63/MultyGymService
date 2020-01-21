using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multigyms.Models
{
    public class Evisita
    {
        public int idusuario { get; set; }
        public int idgym { get; set; }
        public string nombregym { get; set; }
        public int creditousados { get; set; }
        public DateTime fechayhora { get; set; }
    }
}