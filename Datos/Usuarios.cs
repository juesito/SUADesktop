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
    
    public partial class Usuarios
    {
        public Usuarios()
        {
            this.Factores = new HashSet<Factores>();
            this.RoleFuncions = new HashSet<RoleFuncions>();
            this.RoleModulos = new HashSet<RoleModulos>();
            this.TopicosUsuarios = new HashSet<TopicosUsuarios>();
            this.TopicosUsuarios1 = new HashSet<TopicosUsuarios>();
        }
    
        public int Id { get; set; }
        public string nombreUsuario { get; set; }
        public string contrasena { get; set; }
        public string claveUsuario { get; set; }
        public string email { get; set; }
        public string apellidoMaterno { get; set; }
        public string apellidoPaterno { get; set; }
        public string estatus { get; set; }
        public System.DateTime fechaIngreso { get; set; }
        public int roleId { get; set; }
        public int plazaId { get; set; }
    
        public virtual ICollection<Factores> Factores { get; set; }
        public virtual Plazas Plazas { get; set; }
        public virtual ICollection<RoleFuncions> RoleFuncions { get; set; }
        public virtual ICollection<RoleModulos> RoleModulos { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual ICollection<TopicosUsuarios> TopicosUsuarios { get; set; }
        public virtual ICollection<TopicosUsuarios> TopicosUsuarios1 { get; set; }
    }
}