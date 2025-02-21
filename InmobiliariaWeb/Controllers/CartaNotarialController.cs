using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class CartaNotarialController:Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Index() 
        {
            return View();
        }

    }
}
