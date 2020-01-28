using Chat.Data.Controllers;
using Chat.Data.Repository;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Data.Domain
{
    public class RoomService
    {
        
        private readonly IChatRoomRepository chatRoomRepo;

        public RoomService(IChatRoomRepository chatRoomRepository)
        {
            this.chatRoomRepo = chatRoomRepository;
        }

        public Task<ChatRoom> GetRoom(string roomName, CancellationToken cancellationToken)
        {
            return chatRoomRepo.GetRoomAsync(roomName, 10, cancellationToken);
        }

        public async Task SendMessage(string room, string user, string message, CancellationToken cancellationToken)
        {
            ChatRoom roomStruct = await chatRoomRepo.GetRoomAsync(room, 0, cancellationToken);
            if (roomStruct == null)
            {
                // lazy create room
                roomStruct = new ChatRoom(room);
            }
            
            var chatMessage = new ChatMessage(user, message);

            Task dummy = chatRoomRepo.AddMessageAsync(room, chatMessage, cancellationToken);
            // no need to await for message to be sent
        }

        public async Task<int> GetRoomCountAsync(CancellationToken cancellationToken)
        {
            return await chatRoomRepo.GetRoomCountAsync(cancellationToken);
        }
    }
}
