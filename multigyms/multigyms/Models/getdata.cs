using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multigyms.Models
{
    public class getdata
    {
        public int iddisc { get; set; }
        public int idusuario { get; set; }
        public int idgym { get; set; }
        public string usuariomail { get; set; }
        public string password { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string celular { get; set; }
        public string fechanacimiento { get; set; }
        public string email { get; set; }
        public int idplan { get; set; }
        public string nombregym { get; set; }

        public string lat { get; set; }
        public string lon { get; set; }
        public int fcategoria { get; set; }
        public int montoapagar { get; set; }
        public int fdistancia { get; set; }
        public int fcreditos { get; set; }
        public int fcalificacion { get; set; }
        public int fciudad { get; set; }
        public string imei { get; set; }
        public string pushid { get; set; }
        public int tipoapp { get; set; }
        public bool total { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime fechafin { get; set; }
    }
}