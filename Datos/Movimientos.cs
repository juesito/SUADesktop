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
    
    public partial class Movimientos
    {
        public int id { get; set; }
        public Nullable<int> aseguradoId { get; set; }
        public Nullable<int> acreditadoId { get; set; }
        public string lote { get; set; }
        public System.DateTime fechaTransaccion { get; set; }
        public string tipo { get; set; }
        public string nombreArchivo { get; set; }
    
        public virtual Acreditados Acreditados { get; set; }
        public virtual Asegurados Asegurados { get; set; }
    }
}