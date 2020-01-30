using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chat.Data.Controllers;

namespace Chat.Data
{
    class ChatRoomInMemoryRepository : IChatRoomRepository
    {

        private readonly Dictionary<string, ChatRoom> roomsByName = new Dictionary<string, ChatRoom>();

        public Task<ChatRoom> GetRoomAsync(string roomName, CancellationToken cancellationToken)
        {
            roomsByName.TryGetValue(roomName, out var room);
            return Task.FromResult(room);
        }

        public Task AddMessageAsync(string roomName, ChatMessage chatMessage, CancellationToken cancellationToken)
        {
            ChatRoom room;
            if (!roomsByName.TryGetValue(roomName, out room))
            {
                room = new ChatRoom(roomName);
                roomsByName[roomName] = room;
            }

            room.messages.Add(chatMessage);
            return Task.CompletedTask;
        }

        public Task<int> GetRoomCountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(roomsByName.Count);
        }
    }
}
