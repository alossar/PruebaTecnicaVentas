using System.Collections.Generic;

namespace VentasAMSOFT.Util
{
    /// <summary>
    /// Permite la transferencia de datos y mensajes entre las capas de la aplicacion.
    /// Se utiliza para los casos que requieren de datos, ademas de informar si la peticion fue exitosa, y evitar peticiones adicionales entre las capas.
    /// </summary>
    /// <typeparam name="T">Tipo generico que denota los datos a transferir.</typeparam>
    public class RespuestaPeticion<T>
    {
        #region Propiedades

        /// <summary>
        /// Si la peticion fue exitosa.
        /// </summary>
        public bool Exito { get; set; } = true;
        /// <summary>
        /// Mensajes que deben ser transmitidos entre las capas. Principalmente para informar la causa de un error especifico en la peticion.
        /// Se puede complementar con informacion de excepciones o mensajes a usuarios, pero se deja de manera simple.
        /// </summary>
        public List<string> Mensajes { get; set; } = new List<string>();
        /// <summary>
        /// Los datos a ser transferidos. No se utilizan en todas las transacciones.
        /// </summary>
        public T Datos { get; set; }

        #endregion

    }
}
