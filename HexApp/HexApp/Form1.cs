using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HexApp
{
    public partial class Form1 : Form
    {
        public static string path = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Warhawk Hex Tool";
                ofd.CheckFileExists = true;
                ofd.InitialDirectory = Application.StartupPath;
                ofd.Filter = "Warhawk Save Data|*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    /* OFD sucks and wont let me easily filter files with no extension so I did it this way */
                    if (Path.GetFileNameWithoutExtension(ofd.FileName) == "WHPREF" || Path.GetFileNameWithoutExtension(ofd.FileName) == "ROT-LST")
                    {
                        path = ofd.FileName;
                        FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.ReadWrite);
                        byte[] ImageData = new byte[fs.Length];
                        fs.Read(ImageData, 0, Convert.ToInt32(fs.Length));
                        /* Check if file is a decrypted save file by seeing if it reads "TwkBin" at its file start */
                        if (ImageData[0] == 0x54 && ImageData[1] == 0x77 && ImageData[2] == 0x6B && ImageData[3] == 0x42 && ImageData[4] == 0x69 && ImageData[5] == 0x6E)
                        {
                            /* Current blue team */
                            fs.Seek(26, SeekOrigin.Begin);
                            numericUpDown1.Value = fs.ReadByte() + 1;
                            fs.Seek(27, SeekOrigin.Begin);
                            numericUpDown2.Value = fs.ReadByte() + 1;
                            fs.Seek(28, SeekOrigin.Begin);
                            numericUpDown3.Value = fs.ReadByte() + 1;
                            fs.Seek(30, SeekOrigin.Begin);
                            numericUpDown4.Value = fs.ReadByte();
                            fs.Seek(54, SeekOrigin.Begin);
                            numericUpDown9.Value = fs.ReadByte() + 1;
                            fs.Seek(55, SeekOrigin.Begin);
                            numericUpDown10.Value = fs.ReadByte();

                            /* Current Red Team */
                            fs.Seek(40, SeekOrigin.Begin);
                            numericUpDown7.Value = fs.ReadByte() + 1;
                            fs.Seek(41, SeekOrigin.Begin);
                            numericUpDown11.Value = fs.ReadByte() + 1;
                            fs.Seek(42, SeekOrigin.Begin);
                            numericUpDown12.Value = fs.ReadByte() + 1;
                            fs.Seek(44, SeekOrigin.Begin);
                            numericUpDown8.Value = fs.ReadByte();
                            fs.Seek(65, SeekOrigin.Begin);
                            numericUpDown6.Value = fs.ReadByte() + 1;
                            fs.Seek(66, SeekOrigin.Begin);
                            numericUpDown5.Value = fs.ReadByte();

                            statusTxt.Text = string.Format("{0} successfully loaded", Path.GetFileName(ofd.FileName));

                            fs.Close();
                        }
                        else
                        {
                            statusTxt.Text = string.Format("Failed to load {0}.", Path.GetFileName(ofd.FileName));
                            fs.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                fs.Seek(26, SeekOrigin.Begin); // B-HEAD
                fs.WriteByte(Convert.ToByte(numericUpDown1.Value - 1));
                fs.Seek(27, SeekOrigin.Begin); // B-CHEST
                fs.WriteByte(Convert.ToByte(numericUpDown2.Value - 1));
                fs.Seek(28, SeekOrigin.Begin); // B-LEGS
                fs.WriteByte(Convert.ToByte(numericUpDown3.Value - 1));
                fs.Seek(30, SeekOrigin.Begin); // B-INSIGNIA
                fs.WriteByte(Convert.ToByte(numericUpDown4.Value));
                fs.Seek(54, SeekOrigin.Begin); // B-PLANE
                fs.WriteByte(Convert.ToByte(numericUpDown9.Value - 1));
                fs.Seek(55, SeekOrigin.Begin); // B-PLANE-INSIGNIA
                fs.WriteByte(Convert.ToByte(numericUpDown10.Value));

                fs.Seek(40, SeekOrigin.Begin); // R-HEAD
                fs.WriteByte(Convert.ToByte(numericUpDown7.Value - 1));
                fs.Seek(41, SeekOrigin.Begin); // R-CHEST
                fs.WriteByte(Convert.ToByte(numericUpDown11.Value - 1));
                fs.Seek(42, SeekOrigin.Begin); // R-LEGS
                fs.WriteByte(Convert.ToByte(numericUpDown12.Value - 1));
                fs.Seek(44, SeekOrigin.Begin); // R-INSIGNIA
                fs.WriteByte(Convert.ToByte(numericUpDown8.Value));
                fs.Seek(65, SeekOrigin.Begin); // R-PLANE
                fs.WriteByte(Convert.ToByte(numericUpDown6.Value - 1));
                fs.Seek(66, SeekOrigin.Begin); // R-PLANE-INSIGNIA
                fs.WriteByte(Convert.ToByte(numericUpDown5.Value));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
