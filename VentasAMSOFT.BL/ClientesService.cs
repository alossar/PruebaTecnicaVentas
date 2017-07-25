using System;
using System.Collections.Generic;
using System.Linq;
using VentasAMSOFT.DAL;
using VentasAMSOFT.Model;
using VentasAMSOFT.Util;

namespace VentasAMSOFT.BL
{
    /// <summary>
    /// Clase encargada de las reglas de negocio y crud asociado a la Entidad Cliente.
    /// Implementa la interfaz IServicio con el contrato especificado.
    /// </summary>
    public class ClientesService : IServicio<Cliente>
    {
        #region Atributos

        private VentasUnitOfWork _ventasUOW = new VentasUnitOfWork();
        private Validador _ventasValidador = new Validador();

        #endregion

        #region Metodos

        public RespuestaPeticion<Cliente> Actualizar(params Cliente[] entidades)
        {
            RespuestaPeticion<Cliente> respuesta = new RespuestaPeticion<Cliente>() { Exito = false };

            if (entidades != null && entidades.Length > 0)
            {
                foreach (Cliente entidad in entidades)
                {
                    RespuestaPeticion<bool> respuestaValidacion = _ventasValidador.Validar(entidad);
                    if (respuestaValidacion.Exito && respuestaValidacion.Datos)
                    {
                        _ventasUOW.RepositorioClientes.Actualizar(entidad);
                    }
                }
                _ventasUOW.Guardar();
            }

            return respuesta;
        }

        public RespuestaPeticion<Cliente> Adicionar(Cliente entidad)
        {
            RespuestaPeticion<Cliente> respuesta = new RespuestaPeticion<Cliente>() { Exito = false };

            if (entidad != null)
            {
                RespuestaPeticion<bool> respuestaValidacion = _ventasValidador.Validar(entidad);
                if (respuestaValidacion.Exito && respuestaValidacion.Datos)
                {
                    entidad.IdCliente = Guid.NewGuid();
                    entidad.Habilitado = true;
                    respuesta = _ventasUOW.RepositorioClientes.Adicionar(entidad);
                    if (respuesta.Exito)
                    {
                        _ventasUOW.Guardar();
                    }
                }
            }

            return respuesta;
        }

        public RespuestaPeticion<Cliente> Consultar(Guid id)
        {
            RespuestaPeticion<Cliente> respuesta = new RespuestaPeticion<Cliente>() { Exito = false };
            if (!Guid.Empty.Equals(id))
            {
                respuesta = _ventasUOW.RepositorioClientes.Consultar(id);
            }
            return respuesta;
        }

        public RespuestaPeticion<Cliente> Habilitar(Guid id, bool habilitado)
        {
            RespuestaPeticion<Cliente> respuesta = Consultar(id);
            if (respuesta.Exito)
            {
                respuesta.Datos.Habilitado = habilitado;
                respuesta = Actualizar(respuesta.Datos);
            }
            return respuesta;
        }

        public RespuestaPeticion<IEnumerable<Cliente>> Listar()
        {
            RespuestaPeticion<IEnumerable<Cliente>> respuesta = new RespuestaPeticion<IEnumerable<Cliente>>() { Exito = false };
            respuesta = _ventasUOW.RepositorioClientes.Listar(ordenarPor: clientes => clientes.OrderBy(c => c.Nombre));
            return respuesta;
        }

        #endregion
    }
}
