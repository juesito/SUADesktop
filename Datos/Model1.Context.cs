﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class suaEntities : DbContext
    {
        public suaEntities()
            : base("name=suaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Acreditados> Acreditados { get; set; }
        public virtual DbSet<Asegurados> Asegurados { get; set; }
        public virtual DbSet<catalogoMovimientos> catalogoMovimientos { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<Factores> Factores { get; set; }
        public virtual DbSet<Funcions> Funcions { get; set; }
        public virtual DbSet<Grupos> Grupos { get; set; }
        public virtual DbSet<Incapacidades> Incapacidades { get; set; }
        public virtual DbSet<Modulos> Modulos { get; set; }
        public virtual DbSet<Movimientos> Movimientos { get; set; }
        public virtual DbSet<MovimientosAseguradoes> MovimientosAseguradoes { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }
        public virtual DbSet<Patrones> Patrones { get; set; }
        public virtual DbSet<Plazas> Plazas { get; set; }
        public virtual DbSet<RoleFuncions> RoleFuncions { get; set; }
        public virtual DbSet<RoleModulos> RoleModulos { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<TopicosUsuarios> TopicosUsuarios { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
    }
}
