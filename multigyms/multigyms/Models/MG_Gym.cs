//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace multigyms.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MG_Gym
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MG_Gym()
        {
            this.mg_reviews = new HashSet<mg_reviews>();
            this.MG_Gym_Disc = new HashSet<MG_Gym_Disc>();
            this.MG_Gym_Serv = new HashSet<MG_Gym_Serv>();
            this.MG_Visitas = new HashSet<MG_Visitas>();
        }
    
        public int ID { get; set; }
        public int Id_Ciudad { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Referencias { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Web { get; set; }
        public string Facebook { get; set; }
        public string Email { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lon { get; set; }
        public System.Data.Entity.Spatial.DbGeography Ubicacion { get; set; }
        public short Creditos { get; set; }
        public string ImgLogo { get; set; }
        public string Img1 { get; set; }
        public string Img2 { get; set; }
        public string Img3 { get; set; }
        public string HorarioLV { get; set; }
        public string HorarioS { get; set; }
        public string HorarioDF { get; set; }
        public System.DateTime FecActivacion { get; set; }
        public bool Activo { get; set; }
        public string Instagram { get; set; }
        public string Passw { get; set; }
        public Nullable<int> ReviewCount { get; set; }
        public Nullable<decimal> ReviewAverage { get; set; }
    
        public virtual MG_Ciudades MG_Ciudades { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mg_reviews> mg_reviews { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MG_Gym_Disc> MG_Gym_Disc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MG_Gym_Serv> MG_Gym_Serv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MG_Visitas> MG_Visitas { get; set; }
    }
}
