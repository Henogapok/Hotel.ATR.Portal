using Hotel.ATR.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.ATR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private HotelAtrContext _db;
        public RoomController(HotelAtrContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Room> Get()
        {
            var rooms = _db.Rooms;

            return rooms;
        }
        [HttpGet("{Id}")]
        public Room Get(int id)
        {
            var room = _db.Rooms.FirstOrDefault<Room>(f => f.Id == id);

            return room;
        }
        [HttpGet("GetAvailableRoom")]
        public IEnumerable<Room> GetAvailableRoom(int id)
        {
            var rooms = _db.Rooms;

            return rooms;
        }

        [HttpPost]
        public Room Post( [FromBody] Room room)
        {
            _db.Add(room);
            _db.SaveChanges();
            return room;
        }
        [HttpPut]
        public StatusCodeResult Put([FromForm] Room room)
        {
            var data = _db.Rooms.FirstOrDefault<Room>(f => f.Id == room.Id);
            if(data != null)
            {
                data.Name = room.Name;
                data.Price = room.Price;
                data.PathToImage = room.PathToImage;
                data.Description = room.Description;

                _db.Update(data);
                _db.SaveChanges();

                return Ok();
            }

            return BadRequest();

        }

        [HttpDelete("{Id}")]
        public StatusCodeResult Delete(int Id)
        {
            Room data = _db.Rooms.FirstOrDefault<Room>(f => f.Id == Id);
            if (data != null)
            {
                _db.Rooms.Remove(data);
                return Ok();
            }
            return BadRequest();
        }
    }
}
