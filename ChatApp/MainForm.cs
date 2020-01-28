using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
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
            lstChatRooms.DataSource = new List<string>(roomsModel.GetChatRoomNames());

            RenderChatRoom(await roomsModel.GetSelectedChatRoomAsync());
        }

        private string MessageToString(ChatMessage message)
        {
            string prefix = "";
            if (message.senderUsed != loginModel.MyUserId)
            {
                prefix = " ->\t";
            }

            return $"{prefix}{message.senderUsed}:  {message.messageBody}";
        }

        private void RenderChatRoom(ChatRoom room)
        {
            //lstChatMessages.DataSource = room.messages.ConvertAll(msgStruct => msgStruct.messageBody).ToList();
            lstChatMessages.DataSource = room.messages.Select(msgStruct => MessageToString(msgStruct)).ToList();
            var ctl = lstChatMessages.Controls;

            lblSelectedRoom.Text = room.name + "";
        }

        private void SendMessage()
        {
            if (txtMyMessage.Text.Length > 0)
            {
                roomsModel.SendMessage(txtMyMessage.Text);
                txtMyMessage.Text = "";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void btnSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
            } else if (e.KeyCode == Keys.Escape)
            {
                LoginModel.Instance.MyUserId = Prompt.ShowDialog("Login", "Enter username: ", LoginModel.Instance.MyUserId);
            }
        }

        private void lstChatRooms_Click(object sender, EventArgs e)
        {
            string newRoomName = roomsModel.GetChatRoomNames()[lstChatRooms.SelectedIndex];
            ChangeCurrentRoom(newRoomName);
        }

        private void ChangeCurrentRoom(string newRoomName)
        {
            roomsModel.SelectedRoomName = newRoomName;
            lblSelectedRoom.Text = newRoomName;
        }
    }
}