using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VentasAMSOFT.BL;
using VentasAMSOFT.Model;
using VentasAMSOFT.Util;

namespace VentasAMSOFT.Web.Controllers
{
    /// <summary>
    /// Clase controladora de la gestion de los clientes del sistema.
    /// </summary>
    public class ClientesController : Controller
    {
        #region Atributos 

        private ClientesService _clientesService = new ClientesService();

        #endregion

        #region Metodos

        /// <summary>
        /// Accion que gestiona la vista de inicio, donde se listan los clientes registrados en el sistema.
        /// </summary>
        /// <returns>Vista de inicio.</returns>
        public ActionResult Index()
        {
            RespuestaPeticion<IEnumerable<Cliente>> respuesta = _clientesService.Listar();
            if (respuesta.Exito)
            {
                return View(respuesta.Datos);
            }
            return View();
        }

        /// <summary>
        /// Accion que retorna la vista de adicion de un nuevo cliente.
        /// </summary>
        /// <returns>Vista de adicion de un cliente.</returns>
        public ActionResult Adicionar()
        {
            return View();
        }

        /// <summary>
        /// Accion que responde a peticiones Post para la adicion de una nueva entidad Cliente.
        /// </summary>
        /// <param name="cliente">Entidad cliente a adicionar.</param>
        /// <returns>Si la adicion es correcta, redirige al a vista de inicio, sino, retorna la misma vista con los datos entregados en la peticion.</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Adicionar(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RespuestaPeticion<Cliente> respuesta = _clientesService.Adicionar(cliente);
                    if (!respuesta.Exito)
                    {
                        ModelState.AddModelError("ErrorAdicionarCliente", string.Join(",", respuesta.Mensajes));
                        return View(cliente);
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Recibe la peticion post para editar un conjunto de clientes.
        /// </summary>
        /// <param name="clientes">Entidades cliente</param>
        /// <returns>Vista de inicio</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Editar(Cliente[] clientes)
        {
            if (clientes != null && clientes.Length > 0)
            {
                _clientesService.Actualizar(clientes);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Recibe la peticion para editar un cliente, cambiando su estado de Habilitado.
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns>Vista de inicio</returns>
        public ActionResult Habilitar(Guid? cliente)
        {
            if (cliente.HasValue)
            {
                _clientesService.Habilitar(cliente.Value, true);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Recibe la peticion para editar un cliente, cambiando su estado de Habilitado.
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns>Vista de inicio</returns>
        public ActionResult Inhabilitar(Guid? cliente)
        {
            if (cliente.HasValue)
            {
                _clientesService.Habilitar(cliente.Value, false);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}