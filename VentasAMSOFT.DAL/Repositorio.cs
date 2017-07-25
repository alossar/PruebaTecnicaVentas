using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VentasAMSOFT.Util;

namespace VentasAMSOFT.DAL
{
    /// <summary>
    /// Repositorio generico que hace operaciones CRUD sobre la entidad que se especifique.
    /// </summary>
    /// <typeparam name="T">Entidad base.</typeparam>
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        #region Atributos

        internal VentasContext contexto;
        internal DbSet<T> entidades;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="contexto">El contexto, que contiene las entidades y sobre el que se efectuan operaciones.</param>
        public Repositorio(VentasContext contexto)
        {
            this.contexto = contexto;
            this.entidades = contexto.Set<T>();
        }

        #endregion

        #region Metodos

        public RespuestaPeticion<T> Actualizar(T entidad)
        {
            RespuestaPeticion<T> respuesta = new RespuestaPeticion<T>() { Exito = true };
            try
            {
                respuesta.Datos = entidades.Attach(entidad);
                contexto.Entry(entidad).State = EntityState.Modified;
            }
            catch
            {
                respuesta.Exito = false;
            }
            return respuesta;
        }

        public RespuestaPeticion<T> Adicionar(T entidad)
        {
            RespuestaPeticion<T> respuesta = new RespuestaPeticion<T>() { Exito = true };
            try
            {
                respuesta.Datos = entidades.Add(entidad);
            }
            catch
            {
                respuesta.Exito = false;
            }
            return respuesta;
        }

        public RespuestaPeticion<T> Consultar(Guid id)
        {
            RespuestaPeticion<T> respuesta = new RespuestaPeticion<T>() { Exito = true };
            respuesta.Datos = entidades.Find(id);
            return respuesta;
        }

        public RespuestaPeticion<T> Buscar(Expression<Func<T, bool>> filtro, params Expression<Func<T, object>>[] propiedades)
        {
            RespuestaPeticion<T> respuesta = new RespuestaPeticion<T>() { Exito = true };

            IQueryable<T> query = entidades;

            //Aplicar la consulta de propiedades especificas si existen.
            foreach (Expression<Func<T, object>> propiedad in propiedades)
                query = query.Include<T, object>(propiedad);

            //Consultar y aplicar el filtro
            respuesta.Datos = query.FirstOrDefault(filtro);
            return respuesta;
        }

        public bool Eliminar(Guid id)
        {
            return Eliminar(entidades.Find(id));
        }

        public bool Eliminar(T entidad)
        {
            try
            {
                if (contexto.Entry(entidad).State == EntityState.Detached)
                {
                    entidades.Attach(entidad);
                }
                entidades.Remove(entidad);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public RespuestaPeticion<IEnumerable<T>> Listar(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor = null, params Expression<Func<T, object>>[] propiedades)
        {
            RespuestaPeticion<IEnumerable<T>> respuesta = new RespuestaPeticion<IEnumerable<T>>() { Exito = true };
            IQueryable<T> query = entidades;
            try
            {
                //Aplicar el filtro si existe.
                if (filtro != null)
                {
                    query = query.Where(filtro);
                }

                //Aplicar la consulta de propiedades especificas si existen.
                foreach (Expression<Func<T, object>> propiedad in propiedades)
                    query = query.Include<T, object>(propiedad);

                //Ordener si es requerido, de lo contrario, retornar los redultados.
                if (ordenarPor != null)
                {
                    respuesta.Datos = ordenarPor(query).ToList();
                }
                else
                {
                    respuesta.Datos = query.ToList();
                }
            }
            catch
            {
                respuesta.Exito = false;
            }
            return respuesta;
        }

        #endregion
    }
}
