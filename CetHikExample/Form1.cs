using RFIDReaderDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CetHikExample
{
    /// <summary>
    /// 作者：www.cisharp.com
    /// 描述：此示例基于中电海康304固定式读写器编写；若其它型号的固定式读写器，请自定义更改。
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private RFIDReaderDll.RFIDClient reader;
        private void btn_conn_Click(object sender, EventArgs e)
        {
            if (btn_conn.Text == "连接")
            {
                reader = new RFIDReaderDll.RFIDClient();
                //网口方式连接
                reader.Connect(txt_ip.Text, 7880);
                //串口连接
                //reader.ConnectSerial("COM5",115200);
                reader.OnInventoryReport += Reader_OnInventoryReport;
                btn_conn.Text = "断开";
            }
            else
            {
                reader?.StopPeriodInventory();
                reader?.Disconnect();
                btn_conn.Text = "连接";
            }
        }

        private void Reader_OnInventoryReport(object sender, RFIDReaderDll.InventoryReportEventArgs e)
        {
            //与界面控件交互需要Invoke
            this.Invoke((Action)delegate
            {
                foreach (var item in e.Report.Tags)
                {
                    ri_log.Text += item.Epc + "\r\n";
                }
            });
        }

        private void btn_inv_Click(object sender, EventArgs e)
        {
            if (reader.connectstate == ConnectState.Connected && btn_inv.Text == "开启盘点")
            {
                reader.StartPerioInventory();
                btn_inv.Text = "停止盘点";
            }
            else
            {
                reader.StopPeriodInventory();
                btn_inv.Text = "开启盘点";
            }
        }

        private void btn_cfg_Click(object sender, EventArgs e)
        {
            if (reader.connectstate == ConnectState.Connected)
            {
                SET_ALLANTENNA_PARAM ant = new SET_ALLANTENNA_PARAM()
                {
                    //天线1
                    ant1 = new SET_ANTENNA_PARAMS()
                    {
                        //是否启用
                        bEnable = true,
                        //驻留时长
                        nDwellTime = 500,
                        //盘点周期
                        nInvCycle = 200,
                        //天线功率
                        nPower = 25
                    },
                    //天线2
                    ant2 = new SET_ANTENNA_PARAMS()
                    {
                        bEnable = false,
                        nDwellTime = 500,
                        nInvCycle = 200,
                        nPower = 25
                    },
                    //天线3
                    ant3 = new SET_ANTENNA_PARAMS()
                    {
                        bEnable = false,
                        nDwellTime = 500,
                        nInvCycle = 200,
                        nPower = 25
                    },
                    //天线4
                    ant4 = new SET_ANTENNA_PARAMS()
                    {
                        bEnable = false,
                        nDwellTime = 500,
                        nInvCycle = 200,
                        nPower = 25
                    }
                };

                //设置天线参数
                reader.SetAntenna(ref ant);
                MessageBox.Show("保存成功");
            }
        }
    }
}
