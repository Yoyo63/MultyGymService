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
    
    public partial class MG_Disciplinas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MG_Disciplinas()
        {
            this.MG_Gym_Disc = new HashSet<MG_Gym_Disc>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MG_Gym_Disc> MG_Gym_Disc { get; set; }
    }
}
