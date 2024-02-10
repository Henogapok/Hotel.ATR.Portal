using Hotel.ATR.Portal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.ATR.Portal.Controllers
{
    public class RoomController : Controller
    {
        private IWebHostEnvironment webHost;

        public RoomController(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }

        public IActionResult Index(int page, int counter)
        {
            return View();
        }
        public IActionResult RoomList()
        {
            return View();
        }
        public IActionResult RoomDetails()
        {
            return View();
        }

        /// <summary>
        ///1.string email
        ///2.User user
        ///3.Request.Form
        /// </summary>
        [HttpPost]
        public IActionResult SubscribeNewsLetter(IFormFile userFile)
        {
            var req = Request.Form["email"];

            string path = Path.Combine(webHost.WebRootPath, userFile.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                userFile.CopyTo(stream);
            }

            return View();
        }
    }
}
