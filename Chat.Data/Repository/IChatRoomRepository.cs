﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chat.Data.Controllers;

namespace Chat.Data
{
    interface IChatRoomRepository
    {
        Task AddMessageAsync(string roomName, ChatMessage chatMessage, CancellationToken cancellationToken);
        Task<ChatRoom> GetRoomAsync(string roomName, int historySize, CancellationToken cancellationToken);
    }
}