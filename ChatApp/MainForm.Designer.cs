﻿namespace TeamsPlusPlus
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstChatRooms = new System.Windows.Forms.ListBox();
            this.lblChatRooms = new System.Windows.Forms.Label();
            this.lstChatMessages = new System.Windows.Forms.ListBox();
            this.lblSelectedRoom = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMyMessage = new System.Windows.Forms.TextBox();
            this.btnJoinRoom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstChatRooms
            // 
            this.lstChatRooms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstChatRooms.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstChatRooms.FormattingEnabled = true;
            this.lstChatRooms.ItemHeight = 16;
            this.lstChatRooms.Items.AddRange(new object[] {
            "public"});
            this.lstChatRooms.Location = new System.Drawing.Point(12, 36);
            this.lstChatRooms.Name = "lstChatRooms";
            this.lstChatRooms.Size = new System.Drawing.Size(180, 340);
            this.lstChatRooms.TabIndex = 1;
            this.lstChatRooms.Click += new System.EventHandler(this.lstChatRooms_ClickAsync);
            this.lstChatRooms.KeyDown += new System.Windows.Forms.KeyEventHandler(this.global_KeyDown);
            // 
            // lblChatRooms
            // 
            this.lblChatRooms.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChatRooms.Location = new System.Drawing.Point(12, 9);
            this.lblChatRooms.Name = "lblChatRooms";
            this.lblChatRooms.Size = new System.Drawing.Size(180, 24);
            this.lblChatRooms.TabIndex = 2;
            this.lblChatRooms.Text = "Chat Rooms:";
            // 
            // lstChatMessages
            // 
            this.lstChatMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstChatMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstChatMessages.FormattingEnabled = true;
            this.lstChatMessages.ItemHeight = 16;
            this.lstChatMessages.Items.AddRange(new object[] {
            "   "});
            this.lstChatMessages.Location = new System.Drawing.Point(198, 36);
            this.lstChatMessages.Name = "lstChatMessages";
            this.lstChatMessages.Size = new System.Drawing.Size(590, 340);
            this.lstChatMessages.TabIndex = 3;
            this.lstChatMessages.TabStop = false;
            this.lstChatMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.global_KeyDown);
            // 
            // lblSelectedRoom
            // 
            this.lblSelectedRoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectedRoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedRoom.Location = new System.Drawing.Point(198, 9);
            this.lblSelectedRoom.Name = "lblSelectedRoom";
            this.lblSelectedRoom.Size = new System.Drawing.Size(590, 24);
            this.lblSelectedRoom.TabIndex = 4;
            this.lblSelectedRoom.Text = "public";
            this.lblSelectedRoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(719, 420);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(69, 20);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.btnSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.global_KeyDown);
            // 
            // txtMyMessage
            // 
            this.txtMyMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMyMessage.Location = new System.Drawing.Point(198, 420);
            this.txtMyMessage.Name = "txtMyMessage";
            this.txtMyMessage.Size = new System.Drawing.Size(515, 20);
            this.txtMyMessage.TabIndex = 6;
            this.txtMyMessage.Text = " type message here";
            this.txtMyMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.global_KeyDown);
            this.txtMyMessage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtMyMessage_MouseDown);
            // 
            // btnJoinRoom
            // 
            this.btnJoinRoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJoinRoom.Location = new System.Drawing.Point(12, 382);
            this.btnJoinRoom.Name = "btnJoinRoom";
            this.btnJoinRoom.Size = new System.Drawing.Size(180, 23);
            this.btnJoinRoom.TabIndex = 7;
            this.btnJoinRoom.Text = "Join/Create new Room";
            this.btnJoinRoom.UseVisualStyleBackColor = true;
            this.btnJoinRoom.Click += new System.EventHandler(this.btnJoinRoom_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnJoinRoom);
            this.Controls.Add(this.txtMyMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblSelectedRoom);
            this.Controls.Add(this.lstChatMessages);
            this.Controls.Add(this.lblChatRooms);
            this.Controls.Add(this.lstChatRooms);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainForm";
            this.Text = "ChatSoft Teams++";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstChatRooms;
        private System.Windows.Forms.Label lblChatRooms;
        private System.Windows.Forms.ListBox lstChatMessages;
        private System.Windows.Forms.Label lblSelectedRoom;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMyMessage;
        private System.Windows.Forms.Button btnJoinRoom;
    }
}

