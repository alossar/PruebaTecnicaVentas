using FluentValidation.Results;
using System.Linq;
using VentasAMSOFT.Model;

namespace VentasAMSOFT.Util
{
    /// <summary>
    /// Clase encargada de validar las entidades, de acuerdo a los validadores.
    /// </summary>
    public class Validador
    {
        #region Propiedades

        /// <summary>
        /// Valida la entidad telefono.
        /// </summary>
        /// <param name="telefono">Entidad a validar.</param>
        /// <returns></returns>
        public RespuestaPeticion<bool> Validar(Telefono telefono)
        {
            return ValidarEntidad(telefono);
        }

        /// <summary>
        /// Valida la entidad cliente.
        /// </summary>
        /// <param name="telefono">Entidad a validar.</param>
        public RespuestaPeticion<bool> Validar(Cliente cliente)
        {
            return ValidarEntidad(cliente);
        }
        /// <summary>
        /// Valida la entidad de acuerdo a su tipo.
        /// </summary>
        /// <param name="entidad">Entidad a validar</param>
        /// <returns></returns>
        private RespuestaPeticion<bool> ValidarEntidad(object entidad)
        {
            RespuestaPeticion<bool> respuesta = new RespuestaPeticion<bool>();
            try
            {
                ValidationResult resultado = null;

                if (entidad is Cliente)
                {
                    resultado = new ClienteValidator().Validate(entidad as Cliente);
                }
                else if (entidad is Telefono)
                {
                    resultado = new TelefonoValidator().Validate(entidad as Telefono);
                }

                respuesta.Datos = resultado.IsValid;
                respuesta.Mensajes = resultado.Errors.Select(e => e.ErrorMessage).ToList();
            }
            catch
            {
                respuesta.Exito = false;
                respuesta.Mensajes.Add("Error en la validacion.");
            }
            return respuesta;
        }

        #endregion
    }
}
