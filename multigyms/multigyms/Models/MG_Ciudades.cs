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
    
    public partial class MG_Ciudades
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MG_Ciudades()
        {
            this.MG_Gym = new HashSet<MG_Gym>();
        }
    
        public int Id_Ciudad { get; set; }
        public string Ciudad { get; set; }
        public string CodigoPais { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MG_Gym> MG_Gym { get; set; }
    }
}
