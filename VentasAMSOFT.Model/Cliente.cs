using System;
using System.Collections.Generic;

namespace VentasAMSOFT.Model
{
    /// <summary>
    /// Entidad cliente. Almacena la informacion de las personas que compran los telefonos.
    /// </summary>
    public class Cliente
    {
        #region Propiedades

        public Guid IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        /// <summary>
        /// Propiedad que mantiene el estado para el uso de la entidad
        /// </summary>
        public bool Habilitado { get; set; } = true;

        /// <summary>
        /// Propiedad para la relacion uno a muchos con la entidad Venta
        /// </summary>
        public virtual ICollection<Venta> Ventas { get; set; }

        #endregion
    }
}
