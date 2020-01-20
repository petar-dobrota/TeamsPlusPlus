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

        public Task<ChatRoom> GetRoom(string roomName, int historySize, CancellationToken cancellationToken)
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
                throw new ArgumentException("");
            }

            room.messages.Add(chatMessage);
            
            return Task.CompletedTask;
        }

        public Task Persist(ChatRoom roomStruct, CancellationToken cancellationToken)
        {
            roomsByName[roomStruct.name] = roomStruct;
            return Task.CompletedTask;
        }

        public List<string> GetRoomNames(string forUser, CancellationToken cancellationToken)
        {
            // TODO: Impl
            var keys = new List<string>(roomsByName.Keys);
            return keys;
        }
    }
}
