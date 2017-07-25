using System;
using System.Collections.Generic;
using System.Linq;
using VentasAMSOFT.DAL;
using VentasAMSOFT.Model;
using VentasAMSOFT.Util;

namespace VentasAMSOFT.BL
{
    /// <summary>
    /// Clase encargada de las reglas de negocio y crud asociado a la Entidad Telefono.
    /// Implementa la interfaz IServicio con el contrato especificado.
    /// </summary>
    public class TelefonosService : IServicio<Telefono>
    {
        #region Atributos 

        private VentasUnitOfWork _ventasUOW = new VentasUnitOfWork();
        private Validador _ventasValidador = new Validador();

        #endregion

        #region Metodos

        public RespuestaPeticion<Telefono> Actualizar(params Telefono[] entidades)
        {
            RespuestaPeticion<Telefono> respuesta = new RespuestaPeticion<Telefono>() { Exito = false };

            if (entidades != null && entidades.Length > 0)
            {
                foreach (Telefono entidad in entidades)
                {
                    RespuestaPeticion<bool> respuestaValidacion = _ventasValidador.Validar(entidad);
                    if (respuestaValidacion.Exito && respuestaValidacion.Datos)
                    {
                        _ventasUOW.RepositorioTelefonos.Actualizar(entidad);
                    }
                }
                _ventasUOW.Guardar();
            }

            return respuesta;
        }

        public RespuestaPeticion<Telefono> Adicionar(Telefono entidad)
        {
            RespuestaPeticion<Telefono> respuesta = new RespuestaPeticion<Telefono>() { Exito = false };

            if (entidad != null)
            {
                RespuestaPeticion<bool> respuestaValidacion = _ventasValidador.Validar(entidad);
                if (respuestaValidacion.Exito && respuestaValidacion.Datos)
                {
                    entidad.IdTelefono = Guid.NewGuid();
                    entidad.Habilitado = true;
                    respuesta = _ventasUOW.RepositorioTelefonos.Adicionar(entidad);
                    if (respuesta.Exito)
                    {
                        _ventasUOW.Guardar();
                    }
                }
            }

            return respuesta;
        }

        public RespuestaPeticion<Telefono> Consultar(Guid id)
        {
            RespuestaPeticion<Telefono> respuesta = new RespuestaPeticion<Telefono>() { Exito = false };
            if (!Guid.Empty.Equals(id))
            {
                respuesta = _ventasUOW.RepositorioTelefonos.Consultar(id);
            }
            return respuesta;
        }

        public RespuestaPeticion<Telefono> Habilitar(Guid id, bool habilitado)
        {
            RespuestaPeticion<Telefono> respuesta = Consultar(id);
            if (respuesta.Exito)
            {
                respuesta.Datos.Habilitado = habilitado;
                respuesta = Actualizar(respuesta.Datos);
            }
            return respuesta;
        }

        public RespuestaPeticion<IEnumerable<Telefono>> Listar()
        {
            RespuestaPeticion<IEnumerable<Telefono>> respuesta = new RespuestaPeticion<IEnumerable<Telefono>>() { Exito = false };
            respuesta = _ventasUOW.RepositorioTelefonos.Listar(ordenarPor: tel => tel.OrderBy(t => t.Marca).ThenBy(t => t.Modelo).ThenByDescending(t => t.Ano));
            return respuesta;
        }

        #endregion
    }
}
