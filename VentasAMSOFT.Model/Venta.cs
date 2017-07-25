using System;
using System.Collections.Generic;

namespace VentasAMSOFT.Model
{
    /// <summary>
    /// Entidad encargada de guardar la informacion de la venta, el monto vendido, los detalles y el cliente que compra.
    /// </summary>
    public class Venta
    {
        #region Propiedades

        /// <summary>
        /// Clave primaria de la entidad Venta
        /// </summary>
        public Guid IdVenta { get; set; }

        /// <summary>
        /// Clave foranea con la entidad Cliente.
        /// </summary>
        public Guid IdCliente { get; set; }

        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Propiedad para la relacion muchos a uno con la entidad Cliente.
        /// </summary>
        public virtual Cliente Cliente { get; set; }

        /// <summary>
        /// Propiedad para la relacion uno a muchos con la entidad DetalleVenta
        /// </summary>
        public virtual ICollection<DetalleVenta> Detalles { get; set; }

        #endregion
    }
}
