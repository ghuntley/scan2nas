using System;
using System.Drawing;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace scantonas
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{

        [DllImport("winmm.dll", SetLastError=true, 
                                CallingConvention=CallingConvention.Winapi)]
        static extern bool PlaySound(
                string pszSound,
                IntPtr hMod,
                SoundFlags sf );

        [Flags]
        public enum SoundFlags : int 
        {
            SND_SYNC = 0x0000,  // play synchronously (default) 
            SND_ASYNC = 0x0001,  // play asynchronously 
            SND_NODEFAULT = 0x0002,  // silence (!default) if sound not found 
            SND_MEMORY = 0x0004,  // pszSound points to a memory file
            SND_LOOP = 0x0008,  // loop the sound until next sndPlaySound 
            SND_NOSTOP = 0x0010,  // don't stop any currently playing sound 
            SND_NOWAIT = 0x00002000, // don't wait if the driver is busy 
            SND_ALIAS = 0x00010000, // name is a registry alias 
            SND_ALIAS_ID = 0x00110000, // alias is a predefined ID
            SND_FILENAME = 0x00020000, // name is file name 
            SND_RESOURCE = 0x00040004  // name is resource name or atom 
        }        

		private System.Windows.Forms.TextBox textBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(8, 8);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(568, 208);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(576, 210);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(592, 248);
            this.MinimumSize = new System.Drawing.Size(592, 248);
            this.Name = "Form1";
            this.Text = "Scan to NAS";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            string exe = System.Windows.Forms.Application.ExecutablePath.ToString();
            exe = exe.Substring(exe.LastIndexOf("\\") + 1, exe.Length - exe.LastIndexOf("\\") - 1).ToLower();      
			if (args.Length > 0) 
			{
				string thefile = args[0];
				DateTime ds = DateTime.Now;			
				int li = thefile.LastIndexOf( ".");
			    string extn = thefile.Substring(li, thefile.Length- li);				
				                              
                exe = exe.Substring(0, exe.LastIndexOf("."));
                
                string path = @"\\nas\scans";

                // I think we couldn't pass parameters or something.
                // So we had to assume it was a receipt
                // or if it was manually run it was a scan?
                // I blame Lexmark's shitty software for the code below.


                //if (exe != "scantonas")
                //{
                //    path = string.Concat(path, @"\receipts\", exe, @"\");
                //}
                //else
                //{
                //    path = string.Concat(path, @"\scans\");
                //}

                if (!System.IO.Directory.Exists(path)) {
                    try
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Concat("Cannot create directory for path: ", path, "\n", ex.Message), "Scan to NAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        System.Environment.Exit(1);
                    }
                }
                
                path = string.Concat(path, ds.ToString("yyyyMMdd.HHmmss"), extn);
                PlaySound(@"C:\windows\media\chimes.wav", IntPtr.Zero, SoundFlags.SND_FILENAME | SoundFlags.SND_ASYNC);
                System.Threading.Thread.Sleep(400);
                try
                {
                    System.IO.File.Copy(thefile, path);
                } catch (Exception mew) {
                    MessageBox.Show(String.Concat("Error copying file to NAS:\n", mew.Message), "Scan to NAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Environment.Exit(1);
                }        
			} 
			else 
			{
                MessageBox.Show(String.Concat(@"Usage: ", exe, @" <file>"), "Scan to NAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



	
	}
}
