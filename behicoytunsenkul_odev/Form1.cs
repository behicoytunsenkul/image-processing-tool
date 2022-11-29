using Accord.Imaging.Filters;
using Accord.Statistics;
using AForge;
using AForge.Imaging.ComplexFilters;
using AForge.Imaging.Filters;
using DevExpress.XtraEditors;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Page;
using Crop = AForge.Imaging.Filters.Crop;

namespace behicoytunsenkul_odev
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        


        public  Form1()
        {
            InitializeComponent();
        }

        private void resimeklebtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                ilkresimbox.Image = new Bitmap(openFile.FileName);


            }
        }
        public bool GriResmeCevirmeIslemi(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color bmpcolor = bmp.GetPixel(i, j);
                    int red = bmpcolor.R;
                    int green = bmpcolor.G;
                    int blue = bmpcolor.B;
                    int gray = (byte)(.299 * red + .587 * green + .114 * blue);
                    red = gray;
                    green = gray;
                    blue = gray;
                    bmp.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }

            return true;
        }
        private void gricevirbtn_Click(object sender, EventArgs e)
        {
            Bitmap copy = new Bitmap(ilkresimbox.Image);
            GriResmeCevirmeIslemi(copy);
            sonresimbox.Image = copy;
        }

        public bool NegatifResmeCevirmeIslemi(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color bmpcolor = bmp.GetPixel(i, j);
                    int red = 255-bmpcolor.R;
                    int green = 255-bmpcolor.G;
                    int blue = 255-bmpcolor.B;
                    int gray = (byte)(.299 * red + .587 * green + .114 * blue);
                    red = gray;
                    green = gray;
                    blue = gray;
                    bmp.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }

            }
            return true;
        }




        private void negatifcevirbtn_Click(object sender, EventArgs e)
        {
            Bitmap copy = new Bitmap(ilkresimbox.Image);
            NegatifResmeCevirmeIslemi(copy);
            sonresimbox.Image = copy;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void siyahcevirbtn_Click(object sender, EventArgs e)
        {
            Bitmap copy = new Bitmap(ilkresimbox.Image);

            SiyahCevirmeIslemi(copy);
            sonresimbox.Image = copy;
  
        }
        public bool SiyahCevirmeIslemi(Bitmap bmp)
        {
            
            
            for(int i = 0; i<bmp.Width;i++)
            {
                for(int j = 0; j < bmp.Height; j++)
                {

                    Color bmpcolor = bmp.GetPixel(i, j);
                    int red =  bmpcolor.R;
                    int green =  bmpcolor.G;
                    int blue = bmpcolor.B;
                    int gray = (red + green + blue) / 3;
                    red = gray;
                    green = gray;
                    blue = gray;
                    bmp.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    int temp;
                    if (gray > 128)
                    {
                        temp = 255;
                    }
                    else
                    {
                        temp = 0;
                    }
                    bmp.SetPixel(i, j, Color.FromArgb(temp, temp, temp));
                    //Color color = bmp.GetPixel(x, y);
                    //var ava = (color.R + color.B + color.G)/3;
                    //bmp.SetPixel(x,y,Color.FromArgb(color.A,ava,ava,ava));
                    //int temp;
                    //if (ava > 128)
                    //{
                    //    temp = 255;
                    //}
                    //else
                    //{
                    //    temp = 0;
                    //}
                    //bmp.SetPixel(x, y, Color.FromArgb(color.A, temp, temp, temp));

                }
            }



            return true;

        }
        private Bitmap Gaussian(Bitmap bmp)
        {
            AForge.Imaging.Filters.GaussianBlur filter = new AForge.Imaging.Filters.GaussianBlur(10, 10);
            filter.ApplyInPlace(bmp);
            return bmp;
        }
        private void histogramcizbtn_Click(object sender, EventArgs e)
        {
            ResminHistograminiCiz();
        }
        public void ResminHistograminiCiz()
        {
            ArrayList DiziPiksel = new ArrayList();
            int OrtalamaRenk = 0;
            Color OkunanRenk;
            int R = 0, G = 0, B = 0;
            Bitmap GirisResmi; //Histogram için giriş resmi gri-ton olmalıdır.
            GirisResmi = new Bitmap(sonresimbox.Image);
            int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
            int ResimYuksekligi = GirisResmi.Height;
            int i = 0; //piksel sayısı tutulacak.
            for (int x = 0; x < GirisResmi.Width; x++)
            {
                for (int y = 0; y < GirisResmi.Height; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3;
                    //Griton resimde üç kanal rengi aynı değere sahiptir.
                    DiziPiksel.Add(OrtalamaRenk); //Resimdeki tüm noktaları diziye atıyor.
                }
            }
            int[] DiziPikselSayilari = new int[256];
            for (int r = 0; r <= 255; r++) //256 tane renk tonu için dönecek.
            {
                int PikselSayisi = 0;
                for (int s = 0; s < DiziPiksel.Count; s++) //resimdeki piksel sayısınca
        
            {
                    if (r == Convert.ToInt16(DiziPiksel[s]))
                        PikselSayisi++;
                }
                DiziPikselSayilari[r] = PikselSayisi;
            }
            //Değerleri listbox'a ekliyor.
            int RenkMaksPikselSayisi = 0; //Grafikte y eksenini ölçeklerken kullanılacak.
            for (int k = 0; k <= 255; k++)
            {
                //Maksimum piksel sayısını bulmaya çalışıyor.
                if (DiziPikselSayilari[k] > RenkMaksPikselSayisi)

                {
                    RenkMaksPikselSayisi = DiziPikselSayilari[k];
                }
            }
            //Grafiği çiziyor.
            Graphics CizimAlani;
            Pen Kalem1 = new Pen(System.Drawing.Color.Yellow, 1);
            Pen Kalem2 = new Pen(System.Drawing.Color.Red, 1);
            CizimAlani = histogrambox.CreateGraphics();
            histogrambox.Refresh();
            int GrafikYuksekligi = 300;
            double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi;
            double OlcekX = 1.5;
            int X_kaydirma = 10;
            for (int x = 0; x <= 255; x++)
            {
                if (x % 50 == 0)
                    CizimAlani.DrawLine(Kalem2, (int)(X_kaydirma + x * OlcekX),
                   GrafikYuksekligi, (int)(X_kaydirma + x * OlcekX), 0);
                CizimAlani.DrawLine(Kalem1, (int)(X_kaydirma + x * OlcekX),
               GrafikYuksekligi,
                (int)(X_kaydirma + x * OlcekX), (GrafikYuksekligi -
               (int)(DiziPikselSayilari[x] / OlcekY)));
                //Dikey kırmızı çizgiler.
            }
        }

        private void ayniciktibtn_Click(object sender, EventArgs e)
        {
            Bitmap copy = new Bitmap(ilkresimbox.Image);
   
            sonresimbox.Image = copy;
        }
        int scaleFactor = 5;
        float constant = 1.7f;
        private void zoominbtn_Click(object sender, EventArgs e)
        {
            ilkresimbox.Height += Convert.ToInt32(scaleFactor / constant);
            ilkresimbox.Width  += scaleFactor;
        }
       

 

        private void yumusatmabtn_Click(object sender, EventArgs e)
        {

            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap g = Gaussian(bmp);
            sonresimbox.Image = g;



        }
        public static Bitmap Gaussıan(Bitmap srcImage,double[,] kernel)
        {
            
         
            int width = srcImage.Width;
            int height = srcImage.Height;
            BitmapData srcData = srcImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            srcImage.UnlockBits(srcData);
            int colorChannels = 3;
            double[] rgb = new double[colorChannels];
            int foff = (kernel.GetLength(0) - 1) / 2;
            int kcenter = 0;
            int kpixel = 0;
            for (int y = foff; y < height - foff; y++)
            {
                for (int x = foff; x < width - foff; x++)
                {
                    for (int c = 0; c < colorChannels; c++)
                    {
                        rgb[c] = 0.0;
                    }
                    kcenter = y * srcData.Stride + x * 4;
                    for (int fy = -foff; fy <= foff; fy++)
                    {
                        for (int fx = -foff; fx <= foff; fx++)
                        {
                            kpixel = kcenter + fy * srcData.Stride + fx * 4;
                            for (int c = 0; c < colorChannels; c++)
                            {
                                rgb[c] += (double)(buffer[kpixel + c]) * kernel[fy + foff, fx + foff];
                            }
                        }
                    }
                    for (int c = 0; c < colorChannels; c++)
                    {
                        if (rgb[c] > 255)
                        {
                            rgb[c] = 255;
                        }
                        else if (rgb[c] < 0)
                        {
                            rgb[c] = 0;
                        }
                    }
                    for (int c = 0; c < colorChannels; c++)
                    {
                        result[kcenter + c] = (byte)rgb[c];
                    }
                    result[kcenter + 3] = 255;
                }
            }
            Bitmap resultImage = new Bitmap(width, height);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, resultData.Scan0, bytes);
            resultImage.UnlockBits(resultData);
            return resultImage;
        }

        private void btnkeskinlestir_Click(object sender, EventArgs e)
        {
            GoruntuNetlestirme();

        }
        public void GoruntuNetlestirme()
        {
            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(ilkresimbox.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int x, y, i, j, toplamR, toplamG, toplamB;
            int R, G, B;
            int[] Matris = { 0, -2, 0, -2, 11, -2, 0, -2, 0 };
            int MatrisToplami = 0 + -2 + 0 + -2 + 11 + -2 + 0 + -2 + 0;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2;
           x++) 
 {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1)
               / 2; y++)
                {
                    toplamR = 0;
                    toplamG = 0;
                    toplamB = 0;
                    int k = 0;
                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2;
                       j++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                            toplamR = toplamR + OkunanRenk.R * Matris[k];
                            toplamG = toplamG + OkunanRenk.G * Matris[k];
                            toplamB = toplamB + OkunanRenk.B * Matris[k];
                            k++;
                        }
                    }
                    R = toplamR / MatrisToplami;
                    G = toplamG / MatrisToplami;
                    B = toplamB / MatrisToplami;

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
  
                    CikisResmi.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
            sonresimbox.Image = CikisResmi;
        }

        private void kenarbulmabtn_Click(object sender, EventArgs e)
        {
            KenarBulmaFiltresi();
        }
        public void KenarBulmaFiltresi()
        {
            Bitmap GirisResmi, CikisResmiXY, CikisResmiX, CikisResmiY;
            GirisResmi = new Bitmap(ilkresimbox.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmiX = new Bitmap(ResimGenisligi, ResimYuksekligi);
            CikisResmiY = new Bitmap(ResimGenisligi, ResimYuksekligi);
            CikisResmiXY = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int x, y;
            Color Renk;
            int P1, P2, P3, P4, P5, P6, P7, P8, P9;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2;
           x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve
          
  {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1)
               / 2; y++)
                {
                    Renk = GirisResmi.GetPixel(x - 1, y - 1);
                    P1 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y - 1);
                    P2 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y - 1);
                    P3 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x - 1, y);
                    P4 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y);
                    P5 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y);
                    P6 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x - 1, y + 1);
                    P7 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y + 1);
                    P8 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y + 1);
                    P9 = (Renk.R + Renk.G + Renk.B) / 3;
                    //Hesaplamayı yapan Sobel Temsili matrisi ve formülü.
                    int Gx = Math.Abs(-P1 + P3 - 2 * P4 + 2 * P6 - P7 + P9); //Dikey
       
                int Gy = Math.Abs(P1 + 2 * P2 + P3 - P7 - 2 * P8 - P9); //Yatay

                int Gxy = Gx + Gy;
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak. Negatif
     
                    if (Gx > 255) Gx = 255;
                    if (Gy > 255) Gy = 255; if (Gxy > 255) Gxy = 255;
                    CikisResmiX.SetPixel(x, y, Color.FromArgb(Gx, Gx, Gx));
                    CikisResmiY.SetPixel(x, y, Color.FromArgb(Gy, Gy, Gy));
                    CikisResmiXY.SetPixel(x, y, Color.FromArgb(Gxy, Gxy, Gxy));
                }
            }

                //pictureBox2.Image = CikisResmiX;
       
                //pictureBox2.Image = CikisResmiY;
   
                sonresimbox.Image = CikisResmiXY;
        }

        private void btnbinaryyap_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(ilkresimbox.Image);
            Bitmap binary = binaryyapmaislemi(image);
            sonresimbox.Image = binary;
        }
        private Bitmap binaryyapmaislemi(Bitmap image)
        {
            Bitmap bmp = new Bitmap(image);
            int temp = 0;
            int esik = 200;
            Color renk;

            for(int i = 0; i < bmp.Height; i++)
            {
                for(int j = 0; j < bmp.Width; j++)
                {
                    temp = bmp.GetPixel(j, i).R;
                    if (temp < esik)
                    {
                        renk = Color.FromArgb(0, 0, 0);
                        bmp.SetPixel(j, i, renk);
                    }
                    else
                    {
                        renk = Color.FromArgb(255, 255, 255);
                        bmp.SetPixel(j, i, renk);

                    }
                }
            }
            return bmp;
        }





        private int ortancabulma(Bitmap image,int j,int i)
        {
            int[] dizi = new int[9];
            Color renk;
            int sagkomsu;
            int sagustcaprazkomsu;
            int ustkomsu;
            int solusstcaprazkomsu;
            int solkomsu;
            int solaltcaprazkomsu;
            int altkomsu;
            int sagaltcaprazkomsu;



            sagkomsu = image.GetPixel(j + 1, i).R;
            sagustcaprazkomsu = image.GetPixel(j + 1, i - 1).R;
            ustkomsu = image.GetPixel(j, i - 1).R;
            solusstcaprazkomsu = image.GetPixel(j - 1, i - 1).R;
            solkomsu = image.GetPixel(j - 1, i).R;
            solaltcaprazkomsu = image.GetPixel(j - 1, i + 1).R;
            altkomsu = image.GetPixel(j, i + 1).R;
            sagaltcaprazkomsu = image.GetPixel(j + 1, i + 1).R;

            dizi[0] = image.GetPixel(j, i).R;
            dizi[1] = sagkomsu;
            dizi[2] = sagustcaprazkomsu;
            dizi[3] = ustkomsu;
            dizi[4] = solusstcaprazkomsu;
            dizi[5] = solkomsu;
            dizi[6] = solaltcaprazkomsu;
            dizi[7] = altkomsu;
            dizi[8] = sagaltcaprazkomsu;
       
            for(int k = 0; k < 8; k++)
            {
                for (int l = k + 1; l < 9; l++)
                {



                    if (dizi[k] < dizi[l])
                    {

                        continue;

                    }
                    else
                    {
                        int temp = dizi[l];
                        dizi[l] = dizi[k];
                        dizi[k] = temp;
                    }
                }
            }
            return dizi[4];
        }

        private void btnmedian_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap dil = HarmonicMean(bmp);

            sonresimbox.Image = dil;
        }
        public static Bitmap HarmonicMean( Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;

            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);

            int r = 1;
            int wres = w - 2 * r;
            int hres = h - 2 * r;
            Bitmap result_image = new Bitmap(wres, hres);
            BitmapData result_data = result_image.LockBits(
                new Rectangle(0, 0, wres, hres),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            int res_bytes = result_data.Stride * result_data.Height;
            byte[] result = new byte[res_bytes];

            for (int x = r; x < w - r; x++)
            {
                for (int y = r; y < h - r; y++)
                {
                    int pixel_location = x * 3 + y * image_data.Stride;
                    int res_pixel_loc = (x - r) * 3 + (y - r) * result_data.Stride;
                    double[] mean = new double[3];

                    for (int kx = -r; kx <= r; kx++)
                    {
                        for (int ky = -r; ky <= r; ky++)
                        {
                            int kernel_pixel = pixel_location + kx * 3 + ky * image_data.Stride;
                            for (int c = 0; c < 3; c++)
                            {
                                mean[c] += 1d / buffer[kernel_pixel + c];
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        result[res_pixel_loc + c] = (byte)(Math.Pow(2 * r + 1, 2) / mean[c]);
                    }
                }
            }

            Marshal.Copy(result, 0, result_data.Scan0, res_bytes);
            result_image.UnlockBits(result_data);
            return result_image;
        }

 

    

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Bitmap gecici = new Bitmap(sonresimbox.Image);
            ilkresimbox.Image = gecici;
        }

        private void btnResimKaydet_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            Bitmap kayit = new Bitmap(sonresimbox.Image);
            sf.Filter = "*";
            if (sf.ShowDialog()==DialogResult.OK)
            {
                kayit.Save(sf.FileName);
            }
        }

        private void dropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void pngkayitbtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            Bitmap bmpfile = new Bitmap(sonresimbox.Image);
            sfd.Filter = "PNG(*.PNG)|*.png";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmpfile.Save(sfd.FileName);
            }
        }

        private void jpgkayitbtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            Bitmap bmpfile = new Bitmap(sonresimbox.Image);
            sfd.Filter = "JPG(*.JPG)|*.jpg";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmpfile.Save(sfd.FileName);
            }
        }

        private void tffkayitbtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            Bitmap bmpfile = new Bitmap(sonresimbox.Image);
            sfd.Filter = "TIFF(*.TIFF)|*.tiff";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmpfile.Save(sfd.FileName);
            }
        }

        private void bmpkayitbtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            Bitmap bmpfile = new Bitmap(sonresimbox.Image);
            sfd.Filter = "BMP(*.BMP)|*.bmp";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmpfile.Save(sfd.FileName);
            }
        }

        private void histogramesitlebtn_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap hst = HistogramEsitle(bmp);
            sonresimbox.Image = hst;
        }

        private Bitmap HistogramEsitle(Bitmap bmp)
        {
            AForge.Imaging.Filters.HistogramEqualization filter = new AForge.Imaging.Filters.HistogramEqualization();
            filter.ApplyInPlace(bmp);
            return bmp;
        }

        private void btnDilation_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap dil = DilationIslemi(bmp);

            sonresimbox.Image = dil;
        }
        private Bitmap DilationIslemi(Bitmap SrcImage)
        {
            // Create Destination bitmap.
    Bitmap tempbmp = new Bitmap(SrcImage.Width, SrcImage.Height);

            // Take source bitmap data.
            BitmapData SrcData = SrcImage.LockBits(new Rectangle(0, 0,
                SrcImage.Width, SrcImage.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            // Take destination bitmap data.
            BitmapData DestData = tempbmp.LockBits(new Rectangle(0, 0, tempbmp.Width,
                tempbmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Element array to used to dilate.
            byte[,] sElement = new byte[5, 5] {
        {0,0,1,0,0},
        {0,1,1,1,0},
        {1,1,1,1,1},
        {0,1,1,1,0},
        {0,0,1,0,0}
    };

            // Element array size.
            int size = 5;
            byte max, clrValue;
            int radius = size / 2;
            int ir, jr;

            unsafe
            {

                // Loop for Columns.
                for (int colm = radius; colm < DestData.Height - radius; colm++)
                {
                    // Initialise pointers to at row start.
                    byte* ptr = (byte*)SrcData.Scan0 + (colm * SrcData.Stride);
                    byte* dstPtr = (byte*)DestData.Scan0 + (colm * SrcData.Stride);

                    // Loop for Row item.
                    for (int row = radius; row < DestData.Width - radius; row++)
                    {
                        max = 0;
                        clrValue = 0;

                        // Loops for element array.
                        for (int eleColm = 0; eleColm < 5; eleColm++)
                        {
                            ir = eleColm - radius;
                            byte* tempPtr = (byte*)SrcData.Scan0 +
                                ((colm + ir) * SrcData.Stride);

                            for (int eleRow = 0; eleRow < 5; eleRow++)
                            {
                                jr = eleRow - radius;

                                // Get neightbour element color value.
                                clrValue = (byte)((tempPtr[row * 3 + jr] +
                                    tempPtr[row * 3 + jr + 1] + tempPtr[row * 3 + jr + 2]) / 3);

                                if (max < clrValue)
                                {
                                    if (sElement[eleColm, eleRow] != 0)
                                        max = clrValue;
                                }
                            }
                        }

                        dstPtr[0] = dstPtr[1] = dstPtr[2] = max;

                        ptr += 3;
                        dstPtr += 3;
                    }
                }
            }

            // Dispose all Bitmap data.
            SrcImage.UnlockBits(SrcData);
            tempbmp.UnlockBits(DestData);

            // return dilated bitmap.
            return tempbmp;
        }

        private void btnMeanFiltre_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap hst = ArithmeticMean(bmp);
            sonresimbox.Image = hst;
        }
        public static Bitmap ArithmeticMean( Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;
            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);

            int r = 1;
            int wres = w - 2 * r;
            int hres = h - 2 * r;
            Bitmap result_image = new Bitmap(wres, hres);
            BitmapData result_data = result_image.LockBits(
                new Rectangle(0, 0, wres, hres),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            int res_bytes = result_data.Stride * result_data.Height;
            byte[] result = new byte[res_bytes];

            for (int x = r; x < w - r; x++)
            {
                for (int y = r; y < h - r; y++)
                {
                    int pixel_location = x * 3 + y * image_data.Stride;
                    int res_pixel_loc = (x - r) * 3 + (y - r) * result_data.Stride;
                    double[] mean = new double[3];

                    for (int kx = -r; kx <= r; kx++)
                    {
                        for (int ky = -r; ky <= r; ky++)
                        {
                            int kernel_pixel = pixel_location + kx * 3 + ky * image_data.Stride;

                            for (int c = 0; c < 3; c++)
                            {
                                mean[c] += buffer[kernel_pixel + c] / Math.Pow(2 * r + 1, 2);
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        result[res_pixel_loc + c] = (byte)mean[c];
                    }
                }
            }

            Marshal.Copy(result, 0, result_data.Scan0, res_bytes);
            result_image.UnlockBits(result_data);
            return result_image;
        }

        private void btnErosion_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap hst = Erosion(bmp,5);
            sonresimbox.Image = hst;
        }
        public static Bitmap Erosion( Bitmap image, int se_dim)
        {
            int w = image.Width;
            int h = image.Height;

            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];

            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);

            int o = (se_dim - 1) / 2;
            for (int i = o; i < w - o; i++)
            {
                for (int j = o; j < h - o; j++)
                {
                    int position = i * 3 + j * image_data.Stride;
                    byte val = 255;
                    for (int x = -o; x <= o; x++)
                    {
                        for (int y = -o; y <= o; y++)
                        {
                            int kposition = position + x * 3 + y * image_data.Stride;
                            val = Math.Min(val, buffer[kposition]);
                        }
                    }
                    for (int c = 0; c < 3; c++)
                    {
                        result[position + c] = val;
                    }
                }
            }

            Bitmap res_img = new Bitmap(w, h);
            BitmapData res_data = res_img.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(result, 0, res_data.Scan0, bytes);
            res_img.UnlockBits(res_data);
            return res_img;
        }

        

        private void ilkresimbox_Click(object sender, EventArgs e)
        {

        }

        private void btnBuyut_Click(object sender, EventArgs e)
        {
            ilkresimbox.Height -= scaleFactor;
            ilkresimbox.Width -= Convert.ToInt32(scaleFactor / constant);
        }



       



        private void btniskeletcikar_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();

        }
        int xDown = 0;
        int yDown = 0;
        int xUp = 0;
        int yUp = 0;
        Rectangle rectCropArea = new Rectangle();
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        Task timeout;
        private void btnSec_Click(object sender, EventArgs e)
        {
            ilkresimbox.Cursor = Cursors.Cross;
        }

        private void ilkresimbox_MouseDown(object sender, MouseEventArgs e)
        {
            ilkresimbox.Invalidate();

            xDown = e.X;
            yDown = e.Y;
            btnkirpmakaydet.Enabled = true;
        }

        private void ilkresimbox_MouseUp(object sender, MouseEventArgs e)
        {
            xUp = e.X;
            yUp = e.Y;
            Rectangle rec = new Rectangle(xDown, yDown, Math.Abs(xUp - xDown), Math.Abs(yUp - yDown));
            using (Pen pen = new Pen(Color.YellowGreen, 3))
            {

                ilkresimbox.CreateGraphics().DrawRectangle(pen, rec);
            }
            rectCropArea = rec;
            btnkirpmakaydet.Enabled = true;
        }

        private void btnkirpmakaydet_Click(object sender, EventArgs e)
        {
            try
            {
                sonresimbox.Refresh();
                //Prepare a new Bitmap on which the cropped image will be drawn
                Bitmap sourceBitmap = new Bitmap(ilkresimbox.Image, ilkresimbox.Width, ilkresimbox.Height);
                Graphics g = sonresimbox.CreateGraphics();

                //Draw the image on the Graphics object with the new dimesions
                g.DrawImage(sourceBitmap, new Rectangle(0, 0, sonresimbox.Width, sonresimbox.Height), rectCropArea, GraphicsUnit.Pixel);
                sourceBitmap.Dispose();
                btnkirpmakaydet.Enabled = false;
                var path = Environment.CurrentDirectory.ToString();
                ms = new System.IO.MemoryStream();

          

                byte[] ar = new byte[ms.Length];
                var timeout = ms.WriteAsync(ar, 0, ar.Length);
   

            }
            catch (Exception ex)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnNoise_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap hst = UniformNoise(bmp);
            sonresimbox.Image = hst;
        }
        public static Bitmap UniformNoise( Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;

            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);

            byte[] noise = new byte[bytes];
            double[] uniform = new double[256];
            double a = 32;
            double b = 64;
            Random rnd = new Random();
            double sum = 0;

            for (int i = 0; i < 256; i++)
            {
                double step = (double)i;
                if (step >= a && step <= b)
                {
                    uniform[i] = (double)(1 / (b - a));
                }
                else
                {
                    uniform[i] = 0;
                }
                sum += uniform[i];
            }

            for (int i = 0; i < 256; i++)
            {
                uniform[i] /= sum;
                uniform[i] *= bytes;
                uniform[i] = (int)Math.Floor(uniform[i]);
            }

            int count = 0;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < (int)uniform[i]; j++)
                {
                    noise[j + count] = (byte)i;
                }
                count += (int)uniform[i];
            }

            for (int i = 0; i < bytes - count; i++)
            {
                noise[count + i] = 0;
            }

            noise = noise.OrderBy(x => rnd.Next()).ToArray();

            for (int i = 0; i < bytes; i++)
            {
                result[i] = (byte)(buffer[i] + noise[i]);
            }

            Bitmap result_image = new Bitmap(w, h);
            BitmapData result_data = result_image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(result, 0, result_data.Scan0, bytes);
            result_image.UnlockBits(result_data);
            return result_image;
        }

        private void btnexpo_Click(object sender, EventArgs e)
        {

            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap hst = ExponentialNoise(bmp);
            sonresimbox.Image = hst;
        }
        public static Bitmap ExponentialNoise( Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;

            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);

            byte[] noise = new byte[bytes];
            double[] erlang = new double[256];
            double a = 5;
            Random rnd = new Random();
            double sum = 0;

            for (int i = 0; i < 256; i++)
            {
                double step = (double)i * 0.01;
                if (step >= 0)
                {
                    erlang[i] = (double)(a * Math.Exp(-a * step));
                }
                else
                {
                    erlang[i] = 0;
                }
                sum += erlang[i];
            }

            for (int i = 0; i < 256; i++)
            {
                erlang[i] /= sum;
                erlang[i] *= bytes;
                erlang[i] = (int)Math.Floor(erlang[i]);
            }

            int count = 0;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < (int)erlang[i]; j++)
                {
                    noise[j + count] = (byte)i;
                }
                count += (int)erlang[i];
            }

            for (int i = 0; i < bytes - count; i++)
            {
                noise[count + i] = 0;
            }

            noise = noise.OrderBy(x => rnd.Next()).ToArray();

            for (int i = 0; i < bytes; i++)
            {
                result[i] = (byte)(buffer[i] + noise[i]);
            }

            Bitmap result_image = new Bitmap(w, h);
            BitmapData result_data = result_image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(result, 0, result_data.Scan0, bytes);
            result_image.UnlockBits(result_data);

            return result_image;
        }

     

        private void btngausgurultusu_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap hst = GaussianNoise(bmp);
            sonresimbox.Image = hst;
        }
        public static Bitmap GaussianNoise( Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;

            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);

            byte[] noise = new byte[bytes];
            double[] gaussian = new double[256];
            int std = 20;
            Random rnd = new Random();
            double sum = 0;
            for (int i = 0; i < 256; i++)
            {
                gaussian[i] = (double)((1 / (Math.Sqrt(2 * Math.PI) * std)) * Math.Exp(-Math.Pow(i, 2) / (2 * Math.Pow(std, 2))));
                sum += gaussian[i];
            }

            for (int i = 0; i < 256; i++)
            {
                gaussian[i] /= sum;
                gaussian[i] *= bytes;
                gaussian[i] = (int)Math.Floor(gaussian[i]);
            }

            int count = 0;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < (int)gaussian[i]; j++)
                {
                    noise[j + count] = (byte)i;
                }
                count += (int)gaussian[i];
            }

            for (int i = 0; i < bytes - count; i++)
            {
                noise[count + i] = 0;
            }

            noise = noise.OrderBy(x => rnd.Next()).ToArray();

            for (int i = 0; i < bytes; i++)
            {
                result[i] = (byte)(buffer[i] + noise[i]);
            }

            Bitmap result_image = new Bitmap(w, h);
            BitmapData result_data = result_image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(result, 0, result_data.Scan0, bytes);
            result_image.UnlockBits(result_data);
            return result_image;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(ilkresimbox.Image);
            Bitmap hst = ImpulseNoise(bmp);
            sonresimbox.Image = hst;
        }
        public static Bitmap ImpulseNoise( Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;

            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);

            Random rnd = new Random();
            int noise_chance = 10;
            for (int i = 0; i < bytes; i += 3)
            {
                int max = (int)(1000 / noise_chance);
                int tmp = rnd.Next(max + 1);
                for (int j = 0; j < 3; j++)
                {
                    if (tmp == 0 || tmp == max)
                    {
                        int sorp = tmp / max;
                        result[i + j] = (byte)(sorp * 255);
                    }
                    else
                    {
                        result[i + j] = buffer[i + j];
                    }
                }
            }

            Bitmap result_image = new Bitmap(w, h);
            BitmapData result_data = result_image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(result, 0, result_data.Scan0, bytes);
            result_image.UnlockBits(result_data);

            return result_image;
        }

        private void btnGama_Click(object sender, EventArgs e)
        {

        }
    }

}
