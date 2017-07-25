using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentasAMSOFT.BL;
using VentasAMSOFT.Model;
using VentasAMSOFT.Util;

namespace VentasAMSOFT.Web.Controllers
{
    /// <summary>
    /// Controlador principal, encargado del registro de ventas y del reporte.
    /// </summary>
    public class HomeController : Controller
    {
        #region Atributos

        private VentasService ventasService = new VentasService();

        #endregion

        public ActionResult Index()
        {
            //Obtener los clientes habilidatos para ser usados por la vista.
            RespuestaPeticion<IEnumerable<Cliente>> respuestaClientes = ventasService.ObtenerClientes();
            if (respuestaClientes.Exito)
            {
                ViewBag.Clientes = respuestaClientes.Datos;
            }
            //Obtener los telefonos habilitados para ser usados por la vista.
            RespuestaPeticion<IEnumerable<Telefono>> respuestaTelefonos = ventasService.ObtenerTelefonos();
            if (respuestaTelefonos.Exito)
            {
                ViewBag.Telefonos = respuestaTelefonos.Datos;
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GuardarVenta(Venta venta)
        {
            if (venta != null)
            {
                if (ModelState.IsValid)
                {
                    RespuestaPeticion<Venta> respuesta = ventasService.GuardarVenta(venta);
                    return Json(new { Exito = respuesta.Exito, Mensaje = string.Join(".", respuesta.Mensajes) });
                }
            }
            return Json(new { Exito = false, Mensaje = "Los datos no son validos para guardar la venta." });
        }

        public ActionResult Registros()
        {
            string periodoDeseado = ConfigurationManager.AppSettings["RangoReporteVentas"];
            int periodo = 1;
            RespuestaPeticion<IEnumerable<Venta>> respuesta = ventasService.ObtenerVentas(int.TryParse(periodoDeseado, out periodo) ? periodo : -1);
            if (respuesta.Exito)
            {
                return View(respuesta.Datos);
            }
            return View();
        }

    }
}