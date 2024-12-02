using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OPCAutomation;
using OpcRcw.Da;

namespace opcHelper
{
    public partial class Form1 : Form
    {
        private OPCItem item;

        private OPCServer server;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 创建 OPC Server 对象
            server = new OPCServer();
            // 连接到本地的 OPC 服务器
            string serverName = textBox1.Text; // 替换为实际的 OPC Server 名称
            server.Connect(serverName, textBox2.Text);

            Console.WriteLine("成功连接到 OPC Server: " + server.ServerName);

            // 获取 OPC Groups
            OPCGroups groups = server.OPCGroups;

            // 创建一个新的 Group
            OPCGroup group = groups.Add("MyGroup");
            group.IsActive = true;
            group.IsSubscribed = true;

            // 添加一个 Item 到 Group
            OPCItems items = group.OPCItems;
            item = items.AddItem(textBox3.Text, 1); // 替换为实际的 Tag 名称

            Console.WriteLine("成功添加 OPC Item: " + item.ItemID);

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 读取该 Item 的值
            object value;
            object quality;
            object timestamp;
            item.Read((short)OPCDATASOURCE.OPC_DS_DEVICE, out value, out quality, out timestamp);

            label3.Text = value.ToString();
            Console.WriteLine($"读取值成功: Value={value}, Quality={quality}, Timestamp={timestamp}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            // 清理资源
            server.Disconnect();
            Console.WriteLine("已断开与 OPC Server 的连接");
        }
    }



}
