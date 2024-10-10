using BlueSoft.PruebaTecnica.Entities;
using BlueSoft.PruebaTecnica.Interfaces;
using BlueSoft.PruebaTecnica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BlueSoft.PruebaTecnica.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICuentaService cuentaService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ICuentaService cuentaService)
        {
            this.cuentaService = cuentaService;
            _logger = logger;
            ViewData["Title"] = "Transaction Page";
        }

        public IActionResult Index(string action)
        {
            LoadAhorradores();

            return View();
        }

        private void LoadAhorradores()
        {
            var clients = cuentaService.GetAhorradores()
              .Select(c => new SelectListItem
              {
                  Value = c.Id.ToString(),
                  Text = c.Nombre
              })
              .ToList();

            var cuentas = cuentaService.GetCuentas()
             .Select(c => new SelectListItem
             {
                 Value = c.Id.ToString(),
                 Text = c.NroCuenta,
                 Group = new SelectListGroup() { Name = c.AhorradorId.ToString() }
             })
             .ToList();

            ViewBag.Clients = clients;
            ViewBag.Cuentas = cuentas;
        }

        [HttpPost]
        public async Task<ActionResult> Transaction(Guid ahorradorIdOrigen, string Ubicacion, Guid cuentaIdOrigen, decimal amount, string action)
        {
            try
            {
                switch (action)
                {
                    case "consignacion":
                        await this.cuentaService.Movimiento(ahorradorIdOrigen, cuentaIdOrigen, amount, Ubicacion, Enumeraciones.TransactionType.Consignacion);
                        TempData["Message"] = $"Consignacion realizada por el monto de {amount:C}";
                        break;
                    case "retiro":
                        await this.cuentaService.Movimiento(ahorradorIdOrigen, cuentaIdOrigen, amount, Ubicacion, Enumeraciones.TransactionType.Retiro);
                        TempData["Message"] = $"Retiro realizado por el monto de {amount:C}";
                        break;
                    case "ListadoClientes":
                        return RedirectToAction("ListadoClientes", "Home");
                        break;
                    case "ClientesRetiroFueraCiudad":
                        return RedirectToAction("ClientesRetiroFueraCiudad", "Home");
                        break;
                }
            }
            catch (Exception exc)
            {
                TempData["Message"] = $"{exc.Message}";
            }

            LoadAhorradores();
            return Redirect("Index");
        }


        public async Task<IActionResult> ListadoClientes()
        {
            ViewBag.ListadoClientes = await this.cuentaService.ListadoClientes();

            return View();
        }

        public async Task<IActionResult> ClientesRetiroFueraCiudad()
        {
            ViewBag.ClientesRetiroFueraCiudad = await this.cuentaService.ClientesRetiroFueraCiudad();

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class TransactionModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Ingrese ")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

    }
}
