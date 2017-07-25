using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VentasAMSOFT.Util;

namespace VentasAMSOFT.DAL
{
    /// <summary>
    /// Interfaz que sirve de contrato para las operaciones CRUD de una entidad generica.
    /// </summary>
    /// <typeparam name="T">Entidad base del repositorio.</typeparam>
    public interface IRepositorio<T> where T : class
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
        bool Eliminar(Guid id);
        /// <summary>
        /// Elimina una entidad
        /// </summary>
        /// <param name="entidad">Instancia a eliminar.</param>
        /// <returns>Exito de la operacion.</returns>
        bool Eliminar(T entidad);
        /// <summary>
        /// Actualiza una entidad.
        /// </summary>
        /// <param name="entidad">Instancia a actualizar.</param>
        /// <returns>El estado de la operacion, y la entidad modificada.</returns>
        RespuestaPeticion<T> Actualizar(T entidad);
        /// <summary>
        /// Obtiene una entidad buscandola por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <param name="propiedades">Propiedades a seleccionar de las entidad, para limitar la cantidad de datos que se van a transferir.</param>
        /// <returns>El estado de la operacion, y la entidad buscada.</returns>
        RespuestaPeticion<T> Consultar(Guid id);
        /// <summary>
        /// Obtiene una entidad buscandola por distintos criterios.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <param name="propiedades">Propiedades a seleccionar de las entidad, para limitar la cantidad de datos que se van a transferir.</param>
        /// <returns>El estado de la operacion, y la entidad buscada.</returns>
        RespuestaPeticion<T> Buscar(Expression<Func<T, bool>> filtro, params Expression<Func<T, object>>[] propiedades);
        /// <summary>
        /// Busca las entidades, las filtra de acuerdo a las condiciones si se especifican, las ordena si se requiere y obtiene solamente las propiedades que se definan.
        /// </summary>
        /// <param name="filtro">Condiciones que deben de cumplir las entidades para ser seleccionadas.</param>
        /// <param name="ordenarPor">Criterios para ordenar las entidades.</param>
        /// <param name="propiedades">Propiedades a seleccionar de las entidades, para limitar la cantidad de datos que se van a transferir.</param>
        /// <returns>Entidades consultadas.</returns>
        RespuestaPeticion<IEnumerable<T>> Listar(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor = null, params Expression<Func<T, object>>[] propiedades);

        #endregion
    }

}
