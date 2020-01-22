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

        private void TruncateHictory(ref ChatRoom room, int oldestIdx, int newestIdx)
        {

        }

        public Task<ChatRoom> GetRoomAsync(string roomName, int historySize, CancellationToken cancellationToken)
        {

            if (roomsByName.TryGetValue(roomName,out var room))
            {
                int size = room.messages.Count;
                // TODO: truncate
                return Task.FromResult(room);
            }

            return Task.FromResult((ChatRoom) null);
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
    }
}
