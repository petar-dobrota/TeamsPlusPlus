using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Data.Controllers;

namespace Chat.Data
{
    class ChatRoomRepository
    {

        private readonly Dictionary<string, ChatRoom> roomsByName = new Dictionary<string, ChatRoom>();

        private void TruncateHictory(ref ChatRoom room, int oldestIdx, int newestIdx)
        {

        }

        internal Task<ChatRoom> GetRoom(string roomName, int historySize)
        {

            if (roomsByName.TryGetValue(roomName,out var room))
            {
                int size = room.messages.Count;
                // TODO: truncate
                return Task.FromResult(room);
            }
            throw new ArgumentException($"Room {roomName} doesn't exist!");         
            
            /*
            ChatMessage msg = new ChatMessage
            {
                senderUsed = "testUsr",
                messageBody = "mesageeeeee"
            };
            ChatRoom room = new ChatRoom
            {
                name = roomName,
                messages = new List<ChatMessage>()
            };
            room.messages.Add(msg);

            return Task.FromResult(room);*/
        }

        internal Task<ChatRoom> GetRoom(string roomName, int fromIdx, int toIdx)
        {
            throw new NotImplementedException();
        }
    }
}
