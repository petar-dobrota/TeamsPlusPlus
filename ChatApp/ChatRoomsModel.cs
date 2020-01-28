using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatApp
{
    public struct ChatMessage
    {
        public ChatMessage(string _senderUser, string _messageBody)
        {
            senderUsed = _senderUser;
            messageBody = _messageBody;
        }

        public readonly string senderUsed;
        public readonly string messageBody;
    }

    public class ChatRoom
    {
        public ChatRoom()
        { }
        public ChatRoom(string name)
        {
            this.name = name;
            this.messages = new List<ChatMessage>();
        }

        public string name;

        /// <summary>
        /// Oldest message is at index 0
        /// </summary>
        public List<ChatMessage> messages;
    }

    public class ChatRoomsModel
    {
        public static readonly ChatRoomsModel Instance = new ChatRoomsModel();

        private readonly HttpClient httpClient = new HttpClient();

        public ChatRoomsModel()
        {
            var t = new Thread(()=>
            {
                while (true)
                {
                    newMsgHandler.Invoke(new ChatMessage("", ""));
                    Thread.Sleep(1000);
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        public List<string> GetChatRoomNames()
        {
            // TODO: Get
            return new List<string>() { "myroom", "rum2", "rum3"};
        }

        public ChatRoom GetChatRoom(string name)
        {
            return new ChatRoom(name);
        }

        private Action<ChatMessage> newMsgHandler = x => { };

        public void SubscribeOnMessage(Action<ChatMessage> newMsgHandler) {
            this.newMsgHandler = newMsgHandler ?? throw new ArgumentException();
        }

        public string GetSelectedChatRoomName()
        {
            return null;
        }

        private string selectedRoom = "myroom";

        public async Task<ChatRoom> GetSelectedChatRoomAsync()
        {
            string proxyAddress = "http://localhost:8906";


            string proxyUrl = $"{proxyAddress}/api/room/{selectedRoom}";

            using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    // TODO: handle error
                }

                var resp = await response.Content.ReadAsStringAsync();
                var cr = JsonConvert.DeserializeObject<ChatRoom>(resp);
                return cr;
            }



            var room = new ChatRoom("myRandomChatRoom");
            for (int i =0;i<10;i++)
            {
                room.messages.Add(new ChatMessage("senderX", "Messageeee  sasff num " + i));
            }

            return room;
        }

        public void SetSelectedRoom(string roomName) { }

    }
}
