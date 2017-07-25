using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VentasAMSOFT.Util;

namespace VentasAMSOFT.BL
{
    /// <summary>
    /// Contratos de servicios para una entidad T.
    /// </summary>
    /// <typeparam name="T">Entidad sobre la que actuan las operaciones del contrato</typeparam>
    public interface IServicio<T> where T : class
    {
        #region Metodos

        /// <summary>
        /// Persiste una nueva entidad.
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        RespuestaPeticion<T> Adicionar(T entidad);
        /// <summary>
        /// Elimina una entidad, buscandola por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Exito de la operacion.</returns>
        RespuestaPeticion<T> Habilitar(Guid id, bool habilitado);
        /// <summary>
        /// Elimina una entidad
        /// </summary>
        /// <param name="entidades">Instancias a editar.</param>
        /// <returns>Exito de la operacion.</returns>
        RespuestaPeticion<T> Actualizar(params T[] entidades);
        /// <summary>
        /// Obtiene una entidad buscandola por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>El estado de la operacion, y la entidad buscada.</returns>
        RespuestaPeticion<T> Consultar(Guid id);
        /// <summary>
        /// Busca las entidades, las filtra de acuerdo a las condiciones si se especifican, las ordena si se requiere y obtiene solamente las propiedades que se definan.
        /// </summary>
        /// <param name="filtro">Condiciones que deben de cumplir las entidades para ser seleccionadas.</param>
        /// <param name="ordenarPor">Criterios para ordenar las entidades.</param>
        /// <param name="propiedades">Propiedades a seleccionar de las entidades, para limitar la cantidad de datos que se van a transferir.</param>
        /// <returns></returns>
        RespuestaPeticion<IEnumerable<T>> Listar();

        #endregion

    }
}
