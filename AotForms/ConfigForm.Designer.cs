using AotForms;
using System;
using System.Windows.Forms;

namespace Client
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent(); 
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            this.TopMost = false; 
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Config.Notif();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(281, 189); 
            this.Name = "ConfigForm";
            this.Text = "ConfigForm";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.ResumeLayout(false);
        }
    }
}
