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
    
    public partial class MovimientosAseguradoes
    {
        public int id { get; set; }
        public int aseguradoId { get; set; }
        public int movimientoId { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public string sdi { get; set; }
        public Nullable<int> numeroDias { get; set; }
        public string folio { get; set; }
        public int incapacidadId { get; set; }
        public string credito { get; set; }
        public string estatus { get; set; }
    
        public virtual Asegurados Asegurados { get; set; }
        public virtual catalogoMovimientos catalogoMovimientos { get; set; }
        public virtual Incapacidades Incapacidades { get; set; }
    }
}