using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chat.Data.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat.Data.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly RoomService roomService;
        
        public RoomController(RoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet("{room}")]
        public async Task<ActionResult> GetRoom(string room)
        // curl ${HH}"/api/room/myroom"
        {
            ChatRoom chatRoom = await roomService.GetRoom(room, HttpContext.RequestAborted);
            if (chatRoom != null)
            {
                return Json(chatRoom);
            } else
            {
                return NotFound();
            }
        }

        [HttpPost("{room}")]
        public ActionResult SendMessage(string room, string user, [FromBody] string message)
        // curl -H "Content-Type: application/json" -X POST --data "\"poruka\"" ${HH}"/api/room/myroom/?user=usrr&PartitionKey=3&PartitionKind=Int64Range"
        {
            Task dummy = roomService.SendMessage(room, user, message, HttpContext.RequestAborted);
            
            return Ok();
        }

    }
}
