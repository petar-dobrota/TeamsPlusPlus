using Chat.Data.Controllers;
using Chat.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Data.Domain
{
    public class RoomService
    {
        public static readonly RoomService Instance = new RoomService();

        private readonly IChatRoomRepository chatRoomRepo;

        public RoomService()
        {

            // TODO: Impl
            // this.chatRoomRepo = new ChatRoomInMemoryRepository();
            this.chatRoomRepo = new ChatRoomReliableRepository();
        }

        public Task<ChatRoom> GetRoom(string roomName, CancellationToken cancellationToken)
        {
            return chatRoomRepo.GetRoom(roomName, 10, cancellationToken);
        }

        public async Task SendMessage(string room, string user, string message, CancellationToken cancellationToken)
        {
            ChatRoom roomStruct = await chatRoomRepo.GetRoom(room, 0, cancellationToken);
            if (roomStruct == null)
            {
                // lazy create room
                roomStruct = new ChatRoom(room);
                roomStruct.name = room;
                roomStruct.messages = new List<ChatMessage>();

                await chatRoomRepo.Persist(roomStruct, cancellationToken);
            }

            var chatMessage = new ChatMessage
            {
                senderUsed = user,
                messageBody = message
            };

            Task dummy = chatRoomRepo.AddMessageAsync(roomStruct.name, chatMessage, cancellationToken);
            // no need to await for message to be sent

        }
    }
}
