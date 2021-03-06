﻿using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Fabric.Query;
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
        private readonly IReliableStateManager stateManager;
        
        public ChatRoomReliableRepository(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        private async Task<IReliableQueue<ChatMessage>> GetRoomQueue(string roomName)
        {
            return await stateManager.GetOrAddAsync<IReliableQueue<ChatMessage>>($"room:{roomName}");
        }
        
        public async Task AddMessageAsync(string roomName, ChatMessage chatMessage, CancellationToken cancellationToken)
        {
            IReliableQueue<ChatMessage> room = await GetRoomQueue(roomName);

            using (var tx = stateManager.CreateTransaction())
            {
                long msgCount = await room.GetCountAsync(tx);
                if (msgCount == 0)
                {
                    // new room, increment counter
                    await IncrementRoomCountAsync(1, cancellationToken, tx);
                }

                await room.EnqueueAsync(tx, chatMessage);
                await tx.CommitAsync();
            }
        }

        public async Task<ChatRoom> GetRoomAsync(string roomName, CancellationToken cancellationToken)
        {
            IReliableQueue<ChatMessage> room = await GetRoomQueue(roomName);

            ChatRoom chatRoom = new ChatRoom(roomName);

            using (var tx = stateManager.CreateTransaction())
            {
                var roomIterator = (await room.CreateEnumerableAsync(tx)).GetAsyncEnumerator();
                
                while(await roomIterator.MoveNextAsync(cancellationToken))
                {
                    var msg = roomIterator.Current;
                    chatRoom.messages.Add(msg);
                }
            }

            return chatRoom;
        }

        private async Task<int> IncrementRoomCountAsync(int increment, CancellationToken cancellationToken, ITransaction tx)
        {
            // queue should contain single element which is room count
            var roomCountQueue = await stateManager.GetOrAddAsync<IReliableQueue<int>>("roomCount");
            var roomCountConditional = await roomCountQueue.TryDequeueAsync(tx);

            int newCount = (roomCountConditional.HasValue ? roomCountConditional.Value : 0) + increment;
            await roomCountQueue.EnqueueAsync(tx, newCount);

            return newCount;
        }
        
        public async Task<int> GetRoomCountAsync(CancellationToken cancellationToken)
        {
            using (var tx = stateManager.CreateTransaction())
            {
                // queue should contain single element which is room count
                var roomCountQueueOptional = await stateManager.TryGetAsync<IReliableQueue<int>>("roomCount");

                if (!roomCountQueueOptional.HasValue || (await roomCountQueueOptional.Value.GetCountAsync(tx)) == 0)
                {
                    // if queue doesn't exist, that means none of chat rooms were created
                    return 0;
                }

                var it =  (await roomCountQueueOptional.Value.CreateEnumerableAsync(tx)).GetAsyncEnumerator();
                await it.MoveNextAsync(cancellationToken);
                return it.Current;
            }
        }
    }
}
