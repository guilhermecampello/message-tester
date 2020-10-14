using ABnet.Messages.PLC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABnet.MessageTester
{
    public partial class MainForm : Form
    {
        private RabbitMQMessageHandler messageHandler { get; set; }

        public MainForm()
        {
            InitializeComponent();
            this.messageHandler = new RabbitMQMessageHandler();
            LoadMessageTemplate(new SendDestinationMessage("PLC1","123","1","1","2"));
        }

        private void LoadMessageTemplate(Messages.Message message)
        {
            messageTextBox.Text = messageHandler.Serializer.Serialize(message);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            this.messageHandler.SendMessage(this.messageTextBox.Text);
        }
    }
}
