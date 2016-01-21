using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WorkTimer
{
    public partial class WorkTimer : Form
    {
        private static int STATE_PLAY = 0;
        private static int STATE_STOP = 1;

        private int state;          //当前状态
        private int playInterval;   //播放分钟数
        private int stopInterval;   //暂停分钟数
        private DateTime startTime; //定时器开始时时间
        private LockScreen lockScreen;   //屏保窗体
        public WorkTimer()
        {
            InitializeComponent();
            //定时器一秒钟调用一次时间
            this.timer.Interval = 1000;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            //开启定时器
            try
            {
                String playInterval = this.tb_play.Text;
                this.playInterval = Convert.ToInt32(playInterval);
                String stopInterval = this.tb_stop.Text;
                this.stopInterval = Convert.ToInt32(stopInterval);

                if (this.playInterval == 0 || this.stopInterval == 0)
                {
                    MessageBox.Show("启动无效！数字不能为0！请重新输入...");
                    return;
                }
                //开启定时器
                this.startTime = DateTime.Now;

                //创建锁屏
                this.lockScreen = new LockScreen();

                //播放状态
                this.state = STATE_STOP;
                this.timer.Start();
                this.tb_play.Enabled = false;
                this.tb_stop.Enabled = false;
                this.btn_start.Enabled = false;
                this.btn_cancel.Enabled = true;
            }
            catch (OverflowException)
            {
                MessageBox.Show("启动无效！数字过大！请重新输入...");
            }
            catch (FormatException)
            {
                MessageBox.Show("启动无效！数字输入格式错误！重新输入...");
            }


        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.timer.Stop();                                          //停止定时器功能
            //关闭锁屏
            this.lockScreen.Close();
            RecoverySystemVolume();                                     //恢复系统音量

            //恢复窗口状态
            this.lb_leftTime.Text = "";
            this.lb_staus.Text = "";
            this.tb_play.Enabled = true;
            this.tb_stop.Enabled = true;
            this.btn_start.Enabled = true;
            this.btn_cancel.Enabled = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //当前时间
            DateTime now = DateTime.Now;                                
            TimeSpan elapsedSpan = now - this.startTime;

            //计算时间差
            int totalInterval = this.playInterval + this.stopInterval;  
            int seconds = Convert.ToInt32(elapsedSpan.TotalSeconds) % (totalInterval * 60);
            if (seconds < this.playInterval * 60)
            {
                if (this.state != STATE_PLAY)
                {
                    this.lb_staus.Text = "工作中";
                    workingProcess();
                }
                int lefeSeconds = this.playInterval * 60 - seconds;
                this.lb_leftTime.Text = String.Format("剩余时间：{0}分{1}秒", lefeSeconds / 60, lefeSeconds % 60);
            }
            else
            {
                if (this.state != STATE_STOP)
                {
                    this.lb_staus.Text = "休息中";
                    restingProcess();
                }
                int lefeSeconds = totalInterval * 60 - seconds;
                this.lb_leftTime.Text = String.Format("剩余时间：{0}分{1}秒", lefeSeconds / 60, lefeSeconds % 60);
            }
        }

        //工作状态动作
        private void workingProcess() 
        {
            lockScreen.Hide();  //隐藏锁屏
            RecoverySystemVolume();
        }

        //休息状态动作
        private void restingProcess()
        {
            lockScreen.Show();  //显示锁屏
            DecreaseSystemVolume();
        }

        //降低系统音量
        private void DecreaseSystemVolume()
        {
            this.state = STATE_STOP;
            VolumeMute();                   //静音 
        }

        //恢复系统音量
        private void RecoverySystemVolume()
        {
            this.state = STATE_PLAY;

            VolumeUp();
            VolumeDown();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        const uint WM_APPCOMMAND = 0x319;
        const uint APPCOMMAND_VOLUME_UP = 0x0a;
        const uint APPCOMMAND_VOLUME_DOWN = 0x09;
        const uint APPCOMMAND_VOLUME_MUTE = 0x08;

        private void VolumeUp()
        {
            //加音量 
            SendMessage(this.Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_UP * 0x10000);
        }

        private void VolumeDown()
        {
            //减音量 
            SendMessage(this.Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_DOWN * 0x10000);
        }

        //系统静音
        private void VolumeMute()
        { 
            SendMessage(this.Handle, WM_APPCOMMAND, 0x200eb0, APPCOMMAND_VOLUME_MUTE * 0x10000);
        }

        //托盘双击事件处理函数
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        //窗体最小化处理函数
        private void SoundTimer_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }
    }
}
