using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Rules;
using System.Data;
using System.Diagnostics;

namespace ProyectoFinal.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Post(string id)
        {
            var rule = new PublicacionRule(_configuration);
            var post = rule.GetPostById(int.Parse(id));
            if (post == null)
                return NotFound();
            return View(post);
        }

        [Authorize]
        public IActionResult Nuevo()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Add(Publicacion data)
        {
            var rule = new PublicacionRule(_configuration);
            rule.InsertPost(data);
            return View();
        }


        //[AllowAnonymous]
        public IActionResult Index()
        {
            var rule = new PublicacionRule(_configuration);
            var post = rule.GetPostsHome();
            return View(post);
        }

        public IActionResult Luky()
        {
            var rule = new PublicacionRule(_configuration);
            var post = rule.GetOnePostRandom();
            return View(post);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}
        [HttpPost]
        public IActionResult Contacto(Contacto contacto)
        {
            if (!ModelState.IsValid) 
            {
                return View("Contacto", contacto);
            }

            var rule = new ContactoRule(_configuration);
            var mensaje = @"<h1>Gracias por contactarse</h1>
                <p>Hemos recibido tu correo exitosamente.</p>
                <p>A la brevedad nos pondremos en ocntacto</p>
                <hr/><p>Saludos</p><p><b>Polo MC</p>";
            rule.SendEmail(contacto.Email, mensaje, "Mensaje Recibido", "Polo Mina Clavero", "Polo@polomc.com.ar");
            rule.SendEmail("molina_matias@outlook.com", contacto.Mensaje, "Nuevo contacto", contacto.Nombre, contacto.Email);

            return View("Contacto");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}