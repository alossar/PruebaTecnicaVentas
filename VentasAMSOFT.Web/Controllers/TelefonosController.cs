using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VentasAMSOFT.BL;
using VentasAMSOFT.Model;
using VentasAMSOFT.Util;

namespace VentasAMSOFT.Web.Controllers
{
    /// <summary>
    /// Clase controladora de la gestion de los telefonos del sistema.
    /// </summary>
    public class TelefonosController : Controller
    {
        #region Atributos

        private TelefonosService _telefonosService = new TelefonosService();

        #endregion

        #region Metodos

        /// <summary>
        /// Accion que gestiona la vista de inicio, donde se listan los telefonos registrados en el sistema.
        /// </summary>
        /// <returns>Vista de inicio.</returns>
        public ActionResult Index()
        {
            RespuestaPeticion<IEnumerable<Telefono>> respuesta = _telefonosService.Listar();
            if (respuesta.Exito)
            {
                return View(respuesta.Datos);
            }
            return View();
        }

        /// <summary>
        /// Accion que retorna la vista de adicion de un nuevo telefono.
        /// </summary>
        /// <returns>Vista de adicion de un telefono.</returns>
        public ActionResult Adicionar()
        {
            return View();
        }

        /// <summary>
        /// Accion que responde a peticiones Post para la adicion de una nueva entidad Telefono.
        /// </summary>
        /// <param name="cliente">Entidad telefono a adicionar.</param>
        /// <returns>Si la adicion es correcta, redirige al a vista de inicio, sino, retorna la misma vista con los datos entregados en la peticion.</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Adicionar(Telefono telefono)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RespuestaPeticion<Telefono> respuesta = _telefonosService.Adicionar(telefono);
                    if (!respuesta.Exito)
                    {
                        ModelState.AddModelError("ErrorAdicionarTelefono", string.Join(",", respuesta.Mensajes));
                        return View(telefono);
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(telefono);
            }
        }

        /// <summary>
        /// Recibe la peticion post para editar un conjunto de telefonos.
        /// </summary>
        /// <param name="clientes">Entidades telefono</param>
        /// <returns>Vista de inicio</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Editar(Telefono[] telefonos)
        {
            if (telefonos != null && telefonos.Length > 0)
            {
                _telefonosService.Actualizar(telefonos);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Recibe la peticion para editar un telefono, cambiando su estado de Habilitado.
        /// </summary>
        /// <param name="cliente">Identificador del telefono</param>
        /// <returns>Vista de inicio</returns>
        public ActionResult Habilitar(Guid? telefono)
        {
            if (telefono.HasValue)
            {
                _telefonosService.Habilitar(telefono.Value, true);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Recibe la peticion para editar un telefono, cambiando su estado de Habilitado.
        /// </summary>
        /// <param name="cliente">Identificador del telefono</param>
        /// <returns>Vista de inicio</returns>
        public ActionResult Inhabilitar(Guid? telefono)
        {
            if (telefono.HasValue)
            {
                _telefonosService.Habilitar(telefono.Value, false);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
