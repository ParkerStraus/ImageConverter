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

namespace ImageConverterDemo
{
    enum Filters
    {
        BlackNWhite=0,
        Invert=1,
        Pixelated=2,
        SwapChannel = 3,
        Posterization = 4,
        FrostedGlass = 5,
        Blur = 6,
        Warble = 7,
        Mirror = 8,
        Sepia = 9,
        Rotate = 10,
    }
    public partial class Form1 : Form
    {
        public DirectBitmap currentImage = null;
        public DirectBitmap originalimage = null;
        public int EffectsSetting = 0;
       
        public Form1()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.hnet_com_image;
            GetStarterImage();
        }

        private void GetStarterImage()
        {
            Bitmap image = new Bitmap(ImageConverterDemo.Properties.Resources.maxresdefault);
            DirectBitmap newimage = new DirectBitmap(image.Width, image.Height);
            for (int x = 0; x < newimage.Width; x++)
            {
                for (int y = 0; y < newimage.Height; y++)
                {
                    newimage.SetPixel(x, y, image.GetPixel(x, y));
                }
            }
            originalimage = newimage;
            UpdateImage(newimage);
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            this.Text = "PhotoConverter";
            ImageTitle.Text = "";
            GetStarterImage();
        }

        
        private void LoadBtn_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Png Image|*.png|All Files|*.*";
            openFileDialog1.Title = "Open an Image File";
            openFileDialog1.RestoreDirectory = true;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                ImageTitle.Text = Path.GetFileNameWithoutExtension(filename);
                Bitmap loadMap = new Bitmap(@filename);
                DirectBitmap newimage = new DirectBitmap(loadMap.Width, loadMap.Height);
                for(int x = 0; x < newimage.Width; x++)
                {
                    for(int y = 0; y < newimage.Height; y++)
                    {
                        newimage.SetPixel(x, y, loadMap.GetPixel(x, y));
                    }
                }
                this.Text = "PhotoConverter - " + filename;
                originalimage = newimage;
                UpdateImage(originalimage);
            }

                
            
        }


        private void SaveBtn_Click(object sender, EventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Png Image|*.png";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        currentImage.Bitmap.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        currentImage.Bitmap.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        currentImage.Bitmap.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }

                fs.Close();
            }
        }

        public void UpdateImage(DirectBitmap image)
        {
            pictureBox1.Image = image.Bitmap;
            currentImage = image;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            switch (index)
            {
                case (int)Filters.BlackNWhite:
                    EffectsSetting = (int)Filters.BlackNWhite;
                    label2.Hide();
                    trackBar1.Hide();
                    label3.Hide();
                    trackBar2.Hide();
                    label4.Hide();
                    trackBar3.Hide();
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
                case (int)Filters.Invert:
                    EffectsSetting = (int)Filters.Invert;
                    label2.Hide();
                    trackBar1.Hide();
                    label3.Hide();
                    trackBar2.Hide();
                    label4.Hide();
                    trackBar3.Hide();
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
                case (int)Filters.Pixelated:
                    EffectsSetting = (int)Filters.Pixelated;
                    label2.Show();
                    label2.Text = "Pixel Size";
                    trackBar1.Show();
                    trackBar1.Maximum = 200;
                    label3.Hide();
                    trackBar2.Hide();
                    label4.Hide();
                    trackBar3.Hide();
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
                case (int)Filters.SwapChannel:
                    EffectsSetting = (int)Filters.SwapChannel;
                    label2.Show();
                    label2.Text = "Channel Swap setting";
                    trackBar1.Show();
                    trackBar1.Maximum = 4;
                    label3.Hide();
                    trackBar2.Hide();
                    label4.Hide();
                    trackBar3.Hide();
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
                case (int)Filters.Posterization:
                    EffectsSetting = (int)Filters.Posterization;
                    label2.Show();
                    label2.Text = "Posterization intensity";
                    trackBar1.Show();
                    trackBar1.Maximum = 8;
                    label3.Hide();
                    trackBar2.Hide();
                    label4.Hide();
                    trackBar3.Hide();
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
                case (int)Filters.FrostedGlass:
                    EffectsSetting = (int)Filters.FrostedGlass;
                    label2.Show();
                    label2.Text = "Frostiness";
                    trackBar1.Show();
                    trackBar1.Maximum = 10;
                    label3.Show();
                    label3.Text = "Horizontal intensity";
                    trackBar2.Show();
                    trackBar2.Maximum = 20;
                    label4.Show();
                    label4.Text = "Vertical intensity";
                    trackBar3.Show();
                    trackBar3.Maximum = 20;
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
                case (int)Filters.Blur:
                    EffectsSetting = (int)Filters.Blur;
                    label2.Show();
                    label2.Text = "Blur amount";
                    trackBar1.Show();
                    trackBar1.Maximum = 5;
                    label3.Hide();
                    trackBar2.Hide();
                    trackBar2.Maximum = 20;
                    label4.Hide();
                    trackBar3.Hide();
                    trackBar3.Maximum = 20;
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
                case (int)Filters.Warble:
                    EffectsSetting = (int)Filters.Warble;
                    label2.Show();
                    label2.Text = "Warble amount";
                    trackBar1.Show();
                    trackBar1.Maximum = 30;
                    label3.Show();
                    label3.Text = "X intensity";
                    trackBar2.Show();
                    trackBar2.Maximum = 50;
                    label4.Show();
                    label4.Text = "Y intensity";
                    trackBar3.Show();
                    trackBar3.Maximum = 50;
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
                case (int)Filters.Mirror:
                    EffectsSetting = (int)Filters.Mirror;
                    label2.Hide();
                    trackBar1.Hide();
                    trackBar1.Maximum = 10;
                    label3.Hide();
                    trackBar2.Hide();
                    trackBar2.Maximum = 30;
                    label4.Hide();
                    trackBar3.Hide();
                    trackBar3.Maximum = 30;
                    checkBox1.Show();
                    checkBox2.Show();
                    break;
                case (int)Filters.Sepia:
                    EffectsSetting = (int)Filters.Sepia;
                    label2.Hide();
                    trackBar1.Hide();
                    label3.Hide();
                    trackBar2.Hide();
                    label4.Hide();
                    trackBar3.Hide();
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
                case (int)Filters.Rotate:
                    EffectsSetting = (int)Filters.Rotate;
                    label2.Show();
                    label2.Text = "Rotation Direction";
                    trackBar1.Show();
                    trackBar1.Maximum = 1;
                    label3.Hide();
                    trackBar2.Hide();
                    label4.Hide();
                    trackBar3.Hide();
                    checkBox1.Hide();
                    checkBox2.Hide();
                    break;
            }
        }

        private void ApplyEffect_Click(object sender, EventArgs e)
        {
            DirectBitmap newimage = null;
            try{ 
                switch (EffectsSetting)
                {
                    case (int)Filters.BlackNWhite:
                        newimage = Program.BlackNWhite(currentImage);
                        break;
                    case (int)Filters.Invert:
                        newimage = Program.Invert(currentImage);
                        break;
                    case (int)Filters.Pixelated:
                        newimage = Program.Pixelated(currentImage, trackBar1.Value);
                        break;
                    case (int)Filters.SwapChannel:
                        newimage = Program.SwapChannel(currentImage, trackBar1.Value);
                        break;
                    case (int)Filters.Posterization:
                        newimage = Program.Posterization(currentImage, trackBar1.Value);
                        break;
                    case (int)Filters.FrostedGlass:
                        newimage = Program.FrostedGlass(currentImage, trackBar2.Value, trackBar3.Value, trackBar1.Value);
                        break;
                    case (int)Filters.Blur:
                        newimage = Program.Blur(currentImage, trackBar1.Value);
                        break;
                    case (int)Filters.Warble:
                        newimage = Program.Warble(currentImage, trackBar1.Value, trackBar2.Value, trackBar3.Value);
                        break;
                    case (int)Filters.Mirror:
                        newimage = Program.Mirror(currentImage, checkBox2.Checked, checkBox1.Checked);
                        break;
                    case (int)Filters.Sepia:
                        newimage = Program.Sepia(currentImage);
                        break;
                    case (int)Filters.Rotate:
                        newimage = Program.Rotate(currentImage, trackBar1.Value);
                        break;
                }
                UpdateImage(newimage);
            }
            catch(Exception error){
                MessageBox.Show(error.Message, "Incorrect Value Entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory,"["+ DateTime.Now.ToString() + "] " + error.Message);
            }
            
            }
    }
}
