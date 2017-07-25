using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VentasAMSOFT.Model
{
    /// <summary>
    /// Entidad encargada de guardar la informacion de los telefonos que van a ser ofrecidos en venta.
    /// </summary>
    public class Telefono
    {
        #region Propiedades

        public Guid IdTelefono { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public decimal PrecioVenta { get; set; }
        public int CantidadDisponible { get; set; }
        /// <summary>
        /// Propiedad que mantiene el estado para el uso de la entidad
        /// </summary>
        public bool Habilitado { get; set; } = true;

        /// <summary>
        /// Propiedad para la relacion uno a muchos con la entidad DetalleVenta
        /// </summary>
        public virtual ICollection<DetalleVenta> Ventas { get; set; }

        #endregion

        #region Metodos

        public override string ToString()
        {
            return string.Format("{0} {1} ({2})", Marca, Modelo, Ano);
        }

        #endregion
    }
}
