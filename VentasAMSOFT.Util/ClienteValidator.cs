using FluentValidation;
using VentasAMSOFT.Model;

namespace VentasAMSOFT.Util
{
    /// <summary>
    /// Implementacion de Validacion del tool FluentValidation, especifico a la entidad Cliente.
    /// Permite describir las reglas de negocio referente a la entidad, para validarla, y mantenerlas en un unico sitio para potencialmente ser utilizado por otras capas.
    /// </summary>
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        #region Constructores

        /// <summary>
        /// Implementa las reglas de la entidad Cliente.
        /// 1. La propiedad Nombre no puede ser nula o vacia.
        /// </summary>
        public ClienteValidator()
        {
            RuleFor(cliente => cliente.Nombre).NotEmpty().WithMessage("Debe especificar el nombre del cliente.");
        }

        #endregion
    }
}
