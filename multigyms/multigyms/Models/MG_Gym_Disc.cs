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
    
    public partial class MG_Gym_Disc
    {
        public int Id { get; set; }
        public int Id_Gym { get; set; }
        public int Id_Disciplina { get; set; }
    
        public virtual MG_Disciplinas MG_Disciplinas { get; set; }
        public virtual MG_Gym MG_Gym { get; set; }
    }
}