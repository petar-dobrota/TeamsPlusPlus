using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Data.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserInfo(string userId)
        {
            UserInfo userInfo = await userService.GetUserInfoAsync(userId, HttpContext.RequestAborted);
            if (userInfo != null)
            {
                return Json(userInfo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{userId}/joinRoom")]
        public async Task<ActionResult> JoinRoom(string userId, [FromQuery] string roomName)
        {
            await userService.JoinRoomAsync(userId, roomName, HttpContext.RequestAborted);
            return Ok();
        }
    }
}