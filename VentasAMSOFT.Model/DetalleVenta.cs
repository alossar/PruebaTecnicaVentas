using System;

namespace VentasAMSOFT.Model
{
    /// <summary>
    /// Detalles de las ventas. Cada detalle tiene asociado un telefono y la cantidad de unidades que se venden.
    /// </summary>
    public class DetalleVenta
    {
        #region Propiedades

        public Guid IdDetalleVenta { get; set; }
        public Guid IdVenta { get; set; }
        public Guid IdTelefono { get; set; }
        public int Cantidad { get; set; }

        /// <summary>
        /// Propiedad para la relacion muchos a uno con la entidad Telefono.
        /// </summary>
        public virtual Telefono Telefono { get; set; }

        /// <summary>
        /// Propiedad para la relacion muchos a uno con la entidad Venta.
        /// </summary>
        public virtual Venta Venta { get; set; }

        #endregion
    }
}
