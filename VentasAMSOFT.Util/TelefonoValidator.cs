using FluentValidation;
using System;
using VentasAMSOFT.Model;

namespace VentasAMSOFT.Util
{
    /// <summary>
    /// Implementacion de Validacion del tool FluentValidation, especifico a la entidad Telefono.
    /// Permite describir las reglas de negocio referente a la entidad, para validarla, y mantenerlas en un unico sitio para potencialmente ser utilizado por otras capas.
    /// </summary>
    public class TelefonoValidator : AbstractValidator<Telefono>
    {
        #region Constructores 

        /// <summary>
        /// Implementa las reglas de la entidad Telefono.
        /// 1. La propiedad Marca no puede ser nula o vacia.
        /// 2. La propiedad Modelo no puede ser nula o vacia.
        /// 3. La propiedad Año no puede ser menor a 1900 o mayor al año siguiente al actual.
        /// 4. La propiedad CantidadDisponible no puede ser menor o igual a O.
        /// 5. La propiedad PrecioVenta no puede ser menor o igual a 0.
        /// </summary>
        public TelefonoValidator()
        {
            RuleFor(telefono => telefono.Marca).NotEmpty().WithMessage("Debe especificar la marca del telefono.");
            RuleFor(telefono => telefono.Modelo).NotEmpty().WithMessage("Debe especificar el modelo del telefono.");
            RuleFor(telefono => telefono.Ano).GreaterThan(1900).LessThan(DateTime.UtcNow.Year + 1).WithMessage("El año no es valido.");
            RuleFor(telefono => telefono.CantidadDisponible).GreaterThanOrEqualTo(0).WithMessage("La cantidad disponible debe ser mayor a 0.");
            RuleFor(telefono => telefono.PrecioVenta).GreaterThanOrEqualTo(0).WithMessage("Debe especificar un valor de venta del telefono.");
        }

        #endregion
    }
}
