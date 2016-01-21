# 工作定时器
C#实现的工作周期控制，基于之前SoundTimer的功能添加版本。

定时静音锁屏，定时恢复。

## 关键代码：

	using System; 
	using System.Collections.Generic; 
	using System.ComponentModel; 
	using System.Data; 
	using System.Drawing; 
	using System.Text; 
	using System.Windows.Forms; 
	
	using System.Runtime.InteropServices; 
	
	namespace VolumnSet 
	{ 
	    public partial class Form1 : Form 
	    { 
	
	        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)] 
	        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam); 
	        const uint WM_APPCOMMAND = 0x319; 
	        const uint APPCOMMAND_VOLUME_UP = 0x0a; 
	        const uint APPCOMMAND_VOLUME_DOWN = 0x09; 
	        const uint APPCOMMAND_VOLUME_MUTE = 0x08; 
	        public Form1() 
	        { 
	            InitializeComponent(); 
	
	        } 
	        private void button1_Click(object sender, EventArgs e) 
	        { 
	            //加音量 
	            SendMessage(this.Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_UP * 0x10000); 
	        } 
	
	        private void button2_Click(object sender, EventArgs e) 
	        { 
	            //减音量 
	            SendMessage(this.Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_DOWN * 0x10000); 
	        } 
	
	        private void checkBox1_CheckedChanged(object sender, EventArgs e) 
	        { 
	            //静音 
	            SendMessage(this.Handle, WM_APPCOMMAND, 0x200eb0, APPCOMMAND_VOLUME_MUTE * 0x10000); 
	        } 
	    } 
	}

## 参考

[C# 控制windows系统音量,静音 (加减按钮形式)](http://outofmemory.cn/code-snippet/2697/C-control-windows-system-yinliang-jingyin-jiajian-button-xingshi)