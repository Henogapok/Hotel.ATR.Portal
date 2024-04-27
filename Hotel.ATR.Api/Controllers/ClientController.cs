using Hotel.ATR.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Hotel.ATR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private HotelAtrContext _db;
        public ClientController(HotelAtrContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Client> Get()
        {
            var clients = _db.Clients;
            return clients;
        }
    }
}
