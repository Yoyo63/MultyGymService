using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multigyms.Models
{
    public class EResVisitas
    {
        public string creditosganados { get; set; }
        public string visitastotales { get; set; }
        public List<Evisita> visitas { get; set; }
    }
}