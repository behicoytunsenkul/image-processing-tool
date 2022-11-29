using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging.ComplexFilters;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Accord.Statistics;

namespace behicoytunsenkul_odev
{
    public partial class Form2 : DevExpress.XtraEditors.XtraForm
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void resimbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = (Bitmap)System.Drawing.Image.FromFile(openFile.FileName);
                

            }
        }

        private void btniskelet_Click(object sender, EventArgs e)
        {
            SimpleSkeletonization gray = new SimpleSkeletonization();
             
            pictureBox2.Image = gray.Apply((Bitmap)pictureBox1.Image);
        }



        public static byte[] Dilate(byte[] buffer, int width, int height)
        {
            byte[] result = new byte[buffer.Length];

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int position = x + y * width;
                    byte val = 0;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            int se_pos = position + i + j * width;
                            val = Math.Max(val, buffer[se_pos]);
                        }
                    }
                    result[position] = val;
                }
            }

            return result;
        }
        public static byte[] Erode(byte[] buffer, int width, int height)
        {
            byte[] result = new byte[buffer.Length];

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int position = x + y * width;
                    byte val = 15;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            int se_pos = position + i + j * width;
                            val = Math.Min(val, buffer[se_pos]);
                        }
                    }
                    result[position] = val;
                }
            }

            return result;
        }
        public static Bitmap SimpleSkeletonızaton( Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;

            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format8bppIndexed);

            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            byte[] temp = new byte[bytes];
            byte[] result = new byte[bytes];

            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);
            while (true)
            {
                temp = Erode(buffer,w, h);
                int sum = temp.Sum(x => (int)x);
                if (sum == 0)
                {
                    break;
                }
                temp = Dilate(buffer,w, h);
                for (int i = 0; i < bytes; i++)
                {
                    result[i] += (byte)(buffer[i] - temp[i]);
                }
                buffer = Erode(buffer, w, h);
            }


            Bitmap res_img = new Bitmap(w, h);
            BitmapData res_data = res_img.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format8bppIndexed);
            Marshal.Copy(result, 0, res_data.Scan0, bytes);
            res_img.UnlockBits(res_data);

            return res_img;
        }

      
        private void jpgkayitbtn_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            Bitmap bmpfile = new Bitmap(pictureBox2.Image);
            sfd.Filter = "JPG(*.JPG)|*.jpg";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmpfile.Save(sfd.FileName);
            }
        }

        private void tffkayitbtn_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            Bitmap bmpfile = new Bitmap(pictureBox2.Image);
            sfd.Filter = "TIFF(*.TIFF)|*.tiff";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmpfile.Save(sfd.FileName);
            }
        }

        private void bmpkayitbtn_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            Bitmap bmpfile = new Bitmap(pictureBox2.Image);
            sfd.Filter = "BMP(*.BMP)|*.bmp";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmpfile.Save(sfd.FileName);
            }
        }

        private void pngkayitbtn_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            Bitmap bmpfile = new Bitmap(pictureBox2.Image);
            sfd.Filter = "PNG(*.PNG)|*.png";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmpfile.Save(sfd.FileName);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}