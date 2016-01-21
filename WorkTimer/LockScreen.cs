using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorkTimer
{
    public partial class LockScreen : Form
    {
        public LockScreen()
        {
            InitializeComponent();
            this.Hide();

            this.lbl_tip.Location = new Point((this.Size.Width - this.lbl_tip.Size.Width) / 2, (this.Size.Height - this.lbl_tip.Size.Height) / 2);
        }
    }
}
