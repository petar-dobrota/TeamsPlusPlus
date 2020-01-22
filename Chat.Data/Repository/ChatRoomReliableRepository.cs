using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chat.Data.Controllers;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Chat.Data.Repository
{
    public class ChatRoomReliableRepository : IChatRoomRepository
    {
        private IReliableStateManager stateManager => RoomController.StateManager;
        
        private async Task<IReliableQueue<ChatMessage>> GetRoomQueue(string roomName)
        {
            return await stateManager.GetOrAddAsync<IReliableQueue<ChatMessage>>($"room:{roomName}");
        }

        public async Task AddMessageAsync(string roomName, ChatMessage chatMessage, CancellationToken cancellationToken)
        {
            IReliableQueue<ChatMessage> room = await GetRoomQueue(roomName);

            using (var tx = stateManager.CreateTransaction())
            {
                await room.EnqueueAsync(tx, chatMessage);
                await tx.CommitAsync();
            }
            
        }

        public async Task<ChatRoom> GetRoomAsync(string roomName, int historySize, CancellationToken cancellationToken)
        {
            IReliableQueue<ChatMessage> room = await GetRoomQueue(roomName);

            ChatRoom chatRoom = new ChatRoom(roomName);

            using (var tx = stateManager.CreateTransaction())
            {
                var roomIterator = (await room.CreateEnumerableAsync(tx)).GetAsyncEnumerator();
                // roomIterator.Reset(); ?!
                
                while(await roomIterator.MoveNextAsync(cancellationToken))
                {
                    var msg = roomIterator.Current;
                    chatRoom.messages.Add(msg);
                }

            }

            return chatRoom;
        }
       
    }
}
