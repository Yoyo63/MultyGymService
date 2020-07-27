using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multigyms.Models
{
    public class EDisciplina
    {
        public int iddisciplina { get; set; }
        public string nombre { get; set; }
        public string rutaimg { get; set; }
        public List<ESubDisciplina> subdisciplinas { get; set; }
    }
}