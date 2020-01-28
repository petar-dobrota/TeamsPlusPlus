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

        public MainForm()
        {
            InitializeComponent();

            lstChatMessages.DataBindings.GetType();
            var s = lstChatMessages.Items.GetEnumerator();
            s.Reset();
            s.MoveNext();
            var cr = s.Current;

            int x = 0;
        }

        public async Task RedrawAsync()
        {
            lstChatRooms.DataSource = new List<string>(roomsModel.GetChatRoomNames());

            RenderChatRoom(await roomsModel.GetSelectedChatRoomAsync());
        }

        private void RenderChatRoom(ChatRoom room)
        {
            lstChatMessages.DataSource = room.messages.ConvertAll(msgStruct => msgStruct.messageBody).ToList();
            var ctl = lstChatMessages.Controls;


            lblSelectedRoom.Text = room.name + "";
        }
    }
}
