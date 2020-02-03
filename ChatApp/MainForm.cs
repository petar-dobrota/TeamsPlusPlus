using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamsPlusPlus
{
    public partial class MainForm : Form
    {
        private ChatRoomsModel roomsModel = ChatRoomsModel.Instance;
        private LoginModel loginModel => LoginModel.Instance;

        public MainForm()
        {
            InitializeComponent();
        }

        public async Task RedrawAsync()
        {
            UpdateChatRoomsList(await roomsModel.GetChatRoomNamesAsync());
            UpdateChatMessages(await roomsModel.GetSelectedChatRoomAsync());
            lblSelectedRoom.Text = roomsModel.SelectedRoomName;
        }

        private string MessageToString(ChatMessage message)
        {
            string prefix = "";
            if (message.senderUsed != loginModel.MyUserId)
            {
                prefix = "  ->  ";
            }

            return $"{prefix}{message.senderUsed}:  {message.messageBody}";
        }

        private void UpdateChatRoomsList(List<string> roomNames)
        {
            var newRoomsNames = new List<string>(roomNames);
            
            int selectedIndex = lstChatRooms.SelectedIndex;
            lstChatRooms.DataSource = newRoomsNames;
            if (selectedIndex >= newRoomsNames.Count) selectedIndex = newRoomsNames.Count - 1;
            lstChatRooms.SelectedIndex = selectedIndex;
        }

        private void UpdateChatMessages(ChatRoom room)
        {
            var newMessages = room.messages.Select(msgStruct => MessageToString(msgStruct)).ToList();

            int selectedIdx = lstChatMessages.SelectedIndex;
            lstChatMessages.DataSource = newMessages;
            if (newMessages.Count > 0)
            {
                if (selectedIdx >= newMessages.Count) selectedIdx = newMessages.Count - 1;
                lstChatMessages.SelectedIndex = selectedIdx;
            }
        }

        private void SendMessage()
        {
            if (txtMyMessage.Text.Length > 0)
            {
                roomsModel.SendMessage(txtMyMessage.Text);
                txtMyMessage.Text = "";
            }
        }

        private void ChangeCurrentRoom(string newRoomName)
        {
            roomsModel.SelectedRoomName = newRoomName;
            lblSelectedRoom.Text = newRoomName;
        }

        // TODO: Consider removing this
        private static int CollectionHash<T>(IEnumerable<T> list)
        {
            int hash = 1;
            foreach(T elem in list)
            {
                hash = (hash * 31) + (elem != null ? elem.GetHashCode() : 0);
            }
            return hash;
        }

        #region Form event handlers

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void global_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
            } else if (e.KeyCode == Keys.Escape)
            {
                var newUser = Prompt.ShowDialog("Login", "Enter username: ", LoginModel.Instance.MyUserId);
                LoginModel.Instance.LoginAs(newUser);
            }
        }

        private void lstChatRooms_ClickAsync(object sender, EventArgs e)
        {
            string newRoomName = (string) lstChatRooms.SelectedItem;
            ChangeCurrentRoom(newRoomName);
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            string newRoomName = Prompt.ShowDialog("Join room", "Room name: ", roomsModel.SelectedRoomName);
            if (newRoomName != null && newRoomName.Length > 0)
            {
                roomsModel.JoinRoom(newRoomName);
            }
        }

        #endregion

        private void txtMyMessage_MouseDown(object sender, MouseEventArgs e)
        {
            txtMyMessage.SelectionStart = 0;
            txtMyMessage.SelectionLength = txtMyMessage.Text.Length;
        }
    }
}