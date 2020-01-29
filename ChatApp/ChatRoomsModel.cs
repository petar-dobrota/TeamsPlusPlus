using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TeamsPlusPlus
{
    public struct ChatMessage
    {
        public string senderUsed;
        public string messageBody;
    }

    public class ChatRoom
    {
        public ChatRoom()
        { }

        public ChatRoom(string name)
        {
            this.name = name;
            this.messages = new ChatMessage[0];
        }

        public string name;

        /// <summary>
        /// Oldest message is at index 0
        /// </summary>
        public ChatMessage[] messages;
    }

    public class UserInfo
    {
        public string[] joinedRooms;
    }

    public class ChatRoomsModel
    {
        public static readonly ChatRoomsModel Instance = new ChatRoomsModel();

        private readonly HttpClient httpClient = new HttpClient();

        private string serverAddress => loginModel.ServerAddress;

        public string SelectedRoomName;

        private Action<ChatMessage> newMsgHandler = x => { };

        private LoginModel loginModel => LoginModel.Instance;

        private HashSet<string> joindedRooms;

        public void Reset()
        {
            joindedRooms = null;
            SelectedRoomName = "public";
        }

        public ChatRoomsModel()
        {
            Reset();

            var t = new Thread(()=>
            {
                while (true)
                {
                    newMsgHandler.Invoke(new ChatMessage());
                    Thread.Sleep(300);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public async Task<List<string>> GetChatRoomNamesAsync()
        {
            if (joindedRooms == null)
            {
                string proxyUrl = $"{serverAddress}/api/user/{loginModel.MyUserId}/";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        // TODO: handle error
                    }

                    var rawResponse = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(rawResponse);

                    joindedRooms = new HashSet<string>(userInfo.joinedRooms);
                }
            }
            
            List<string> rooms = new List<string>(joindedRooms);
            rooms.Sort();
            return rooms;
        }

        public async Task<ChatRoom> GetChatRoomAsync(string roomName)
        {
            string proxyUrl = $"{serverAddress}/api/room/{roomName}";

            using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    // TODO: handle error
                    return new ChatRoom("ERROR");
                }

                var rawResponse = await response.Content.ReadAsStringAsync();
                var chatRoomResponse = JsonConvert.DeserializeObject<ChatRoom>(rawResponse);
                return chatRoomResponse;
            }
        }

        public void SubscribeOnMessage(Action<ChatMessage> newMsgHandler) {
            this.newMsgHandler += newMsgHandler ?? throw new NullReferenceException();
        }
        
        public async Task<ChatRoom> GetSelectedChatRoomAsync()
        {
            return await GetChatRoomAsync(SelectedRoomName);
        }

        public void SendMessage(string message)
        {
            var newMessageStruct = new ChatMessage();
            newMessageStruct.senderUsed = loginModel.MyUserId;
            newMessageStruct.messageBody = message;

            string proxyUrl = $"{serverAddress}/api/room/{SelectedRoomName}/?user={loginModel.MyUserId}";

            StringContent content = new StringContent($"\"{message}\"", Encoding.UTF8, "application/json");
            
            Task dummy = this.httpClient.PostAsync(proxyUrl, content);
        }

        public void JoinRoom(string newRoomName)
        {
            string proxyUrl = $"{serverAddress}/api/user/{loginModel.MyUserId}/joinRoom?roomName={newRoomName}";
            //StringContent content = new StringContent($"\"{message}\"", Encoding.UTF8, "application/json");
            Task dummy = this.httpClient.PutAsync(proxyUrl, null);

            joindedRooms?.Add(newRoomName);
            SelectedRoomName = newRoomName;
        }
    }
}
