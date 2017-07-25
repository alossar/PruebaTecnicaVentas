using System;
using System.Collections.Generic;
using System.Linq;
using VentasAMSOFT.DAL;
using VentasAMSOFT.Model;
using VentasAMSOFT.Util;
using System.Linq.Expressions;

namespace VentasAMSOFT.BL
{
    /// <summary>
    /// Clase encargada de las operaciones de negocio y CRUD de las ventas.
    /// </summary>
    public class VentasService
    {
        #region Atributos

        private VentasUnitOfWork _ventasUOW = new VentasUnitOfWork();
        private Validador _ventasValidador = new Validador();

        #endregion

        #region Metodos

        /// <summary>
        /// Obtiene una lista con los telefonos habilitados.
        /// </summary>
        /// <returns>Colleccion de telefonos.</returns>
        public RespuestaPeticion<IEnumerable<Telefono>> ObtenerTelefonos()
        {
            return _ventasUOW.RepositorioTelefonos.Listar(filtro: tel => tel.Habilitado);
        }

        /// <summary>
        /// Retorna una lista con los clientes habilitados.
        /// </summary>
        /// <returns>Coleccion de clientes.</returns>
        public RespuestaPeticion<IEnumerable<Cliente>> ObtenerClientes()
        {
            return _ventasUOW.RepositorioClientes.Listar(filtro: cli => cli.Habilitado);
        }

        /// <summary>
        /// Registra una nueva venta en la base de datos, y disminuye las cantidades disponibles en los telefonos.
        /// </summary>
        /// <param name="venta">Venta y detalles a registrar.</param>
        /// <returns>La venta registrada, con el exito de la operacion.</returns>
        public RespuestaPeticion<Venta> GuardarVenta(Venta venta)
        {
            RespuestaPeticion<Venta> respuesta = new RespuestaPeticion<Venta>();
            //Validar que se hayan enviado datos para continuar con el registro de la venta.
            if (venta != null && venta.Detalles != null && venta.Detalles.Count > 0)
            {
                venta.IdVenta = Guid.NewGuid();
                venta.Monto = 0;
                venta.Fecha = DateTime.UtcNow;
                var telefonosVendidos = venta.Detalles.Select(d => d.IdTelefono).Distinct();

                //Obtener los telefonos incluidos en la venta para efectuar la validacion de cantidad y modificar las cantidades restantes.
                RespuestaPeticion<IEnumerable<Telefono>> respuestaTelefonos = _ventasUOW.RepositorioTelefonos.Listar(filtro: tel => telefonosVendidos.Contains(tel.IdTelefono));
                if (respuestaTelefonos.Exito && respuestaTelefonos.Datos != null && respuestaTelefonos.Datos.Count() > 0)
                {
                    telefonosVendidos = respuestaTelefonos.Datos.Select(t => t.IdTelefono);
                    //Agrupar los detalles de la venta duplicados y sumar las cantidades.
                    venta.Detalles = venta.Detalles
                        .GroupBy(d => d.IdTelefono)
                        .Select(g => new DetalleVenta() { IdDetalleVenta = Guid.NewGuid(), Cantidad = g.Sum(d => d.Cantidad), IdTelefono = g.Key })
                        .ToList();

                    //Validar la venta y Guardar si las validaciones son correctas
                    try
                    {
                        RespuestaPeticion<bool> respuestaValidacion = EsVentaValida(venta, respuestaTelefonos.Datos);
                        if (respuestaValidacion.Datos)
                        {
                            foreach (Telefono tel in respuestaTelefonos.Datos)
                            {
                                _ventasUOW.RepositorioTelefonos.Actualizar(tel);
                            }
                            _ventasUOW.RepositorioVentas.Adicionar(venta);
                            _ventasUOW.Guardar();
                        }
                        else
                        {
                            respuesta.Mensajes.AddRange(respuestaValidacion.Mensajes);
                            respuesta.Exito = false;
                        }
                    }
                    catch (Exception e)
                    {
                        respuesta.Exito = false;
                        respuesta.Mensajes.Add("No se pudo registrar la venta");
                        respuesta.Mensajes.Add(e.Message);
                    }
                }
            }
            else
            {
                respuesta.Exito = false;
                respuesta.Mensajes.Add("Los datos no de la venta se encuentran incompletos.");
            }
            return respuesta;
        }

        /// <summary>
        /// Valida si la venta es valida, con los siguientes criterios.
        /// 1. Se haya vendido un telefono que no se encuentra registrado.
        /// 2. Que la cantidad a vender sea menor o igual a la cantidad disponible.
        /// </summary>
        /// <param name="venta">Entidad con los datos a validar</param>
        /// <param name="telefonos">Telefonos que se intentan vender</param>
        /// <returns>Si la venta es valida, con los mensajes de validacion.</returns>
        private RespuestaPeticion<bool> EsVentaValida(Venta venta, IEnumerable<Telefono> telefonos)
        {
            RespuestaPeticion<bool> respuesta = new RespuestaPeticion<bool>() { Datos = true };
            foreach (DetalleVenta detalle in venta.Detalles)
            {
                Telefono telefono = telefonos.Where(t => t.IdTelefono.Equals(detalle.IdTelefono)).FirstOrDefault();
                //Se valida que el telefono exista en la base de datos. 
                //Error que puede ocurrir si se modifican los datos de la peticion.
                if (telefono == null)
                {
                    respuesta.Mensajes.Add(string.Format("Se intenta vender el telefono {0} {1} {2} que no se encuentra registrado.", telefono.Marca, telefono.Modelo, telefono.Ano));
                    respuesta.Datos = false;
                }
                else
                {
                    //Validar que se este vendiendo una cantidad que se encuentre en inventario.
                    if (detalle.Cantidad > telefono.CantidadDisponible)
                    {
                        respuesta.Mensajes.Add(string.Format("Se intenta vender una cantidad mayor a la disponible del telefono {0} {1} {2}. La cantidad maxima del telefono es {3} pero se intentan vender {4}", telefono.Marca, telefono.Modelo, telefono.Ano, telefono.CantidadDisponible, detalle.Cantidad));
                        respuesta.Datos = false;
                    }
                    else
                    {
                        //Reduce la cantidad disponible de cada telefono y calcula adiciona el total del detalle al monto total de la venta.
                        telefono.CantidadDisponible -= detalle.Cantidad;
                        venta.Monto += telefono.PrecioVenta * detalle.Cantidad;
                    }
                }
            }
            return respuesta;
        }

        /// <summary>
        /// Retorna el listado de ventas realizadas en el periodo de tiempo definido.
        /// </summary>
        /// <param name="periodo">0: Ventas del Dia, 1: Ventas del Mes, 2: Ventas del año</param>
        /// <returns>El listado de ventas.</returns>
        public RespuestaPeticion<IEnumerable<Venta>> ObtenerVentas(int periodo = 1)
        {
            Expression<Func<Venta, bool>> exp = null;
            switch (periodo)
            {
                case 0: { exp = v => v.Fecha.Year.Equals(DateTime.UtcNow.Year) && v.Fecha.Month.Equals(DateTime.UtcNow.Month) && v.Fecha.Day.Equals(DateTime.UtcNow.Day); } break;
                case 1: { exp = v => v.Fecha.Month.Equals(DateTime.UtcNow.Month); } break;
                case 2: { exp = v => v.Fecha.Year.Equals(DateTime.UtcNow.Year); } break;
                default: { exp = v => v.Fecha.Month.Equals(DateTime.UtcNow.Month); } break;
            }
            return _ventasUOW.RepositorioVentas.Listar(exp, venta => venta.OrderByDescending(v => v.Fecha), v => v.Detalles, v => v.Detalles.Select(d => d.Telefono), v => v.Cliente);
        }

        #endregion
    }
}
