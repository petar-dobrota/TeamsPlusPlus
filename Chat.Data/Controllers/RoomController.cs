using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat.Data.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {

        private readonly ChatRoomRepository chatRoomRepo;

        private readonly IReliableStateManager stateManager;

        public RoomController(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;

            // TODO: Impl
            this.chatRoomRepo = new ChatRoomRepository();
        }

        [HttpGet("{room}")]
        public async Task<ActionResult<string>> GetRoomAsync(string room)
        {
            ChatRoom roomStruct = await chatRoomRepo.GetRoom(room, 10);
            return Json(roomStruct);
        }

        [HttpPost("{room}")]
        public async Task<ActionResult<string>> SendMessage(string room, string user, [FromBody] string message)
        {
            //curl  -H "Content-Type: application/json" -X POST --data "poruka" "http://MDCS-PEDOBROT1.europe.corp.microsoft.com:1200/1fac7f2c-1a56-4eb5-839f-629b4a63b72a/132236670309702419/c71cb315-1eea-459b-bc08-b55e4ac8839a/api/room/myroom?user=usrr&PartitionKey=3&PartitionKind=Int64Range"

            //return new ActionResult<string>($"HELLOU from {room}\nto {user}\nwith message {message}\n");
            ChatRoom roomStruct = await chatRoomRepo.GetRoom(room, 10);
            return null;
        }

    }
}
