//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Modulos
    {
        public Modulos()
        {
            this.Funcions = new HashSet<Funcions>();
            this.RoleModulos = new HashSet<RoleModulos>();
        }
    
        public int id { get; set; }
        public string descripcionCorta { get; set; }
        public string descripcionLarga { get; set; }
        public System.DateTime fechaCreacion { get; set; }
        public string estatus { get; set; }
    
        public virtual ICollection<Funcions> Funcions { get; set; }
        public virtual ICollection<RoleModulos> RoleModulos { get; set; }
    }
}
