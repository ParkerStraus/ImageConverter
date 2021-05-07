using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;

namespace ImageConverterDemo
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Form1 form = new Form1();
            form.ShowDialog();
            
        }

        public static DirectBitmap BlackNWhite(DirectBitmap image)
        {

            //Precondition: image != null
            //Postcondition: The method will return a black and white image

            DirectBitmap newimage = image;
            int x, y;
            for (x = 0; x < newimage.Height; x++)
            {
                for (y = 0; y < newimage.Width; y++)
                {
                    Color pixelColor = newimage.GetPixel(y, x);
                    double b = 0.2126 * pixelColor.R + 0.7152 * pixelColor.G + 0.0722 * pixelColor.B;

                    Color monoColor = Color.FromArgb(255, (int)Math.Round(b), (int)Math.Round(b), (int)Math.Round(b));
                    newimage.SetPixel(y, x, monoColor);
                }
            }
            return newimage;
        }

        public static DirectBitmap Invert(DirectBitmap original)
        {

            //Precondition: image != null
            //Postcondition: The method will return a negative image

            DirectBitmap filter = original;

            //sepia
            for (int y = 0; y < filter.Height; y++)
            {
                for (int x = 0; x < filter.Width; x++)
                {
                    //get pixel value
                    Color p = original.GetPixel(x, y);

                    //extract pixel component ARGB
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    //find negative value
                    r = 255 - r;
                    g = 255 - g;
                    b = 255 - b;

                    //set new ARGB value in pixel
                    filter.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            return filter;
        }

        public static DirectBitmap Pixelated(DirectBitmap image, int pixelSize)
        {

            //Precondition: image != null, pixelSize > 0
            //Postcondition: The method will return a pixelated image

            DirectBitmap pixelImage = new DirectBitmap(image.Width / pixelSize, image.Height / pixelSize);

            for (int x = 0; x < image.Height / pixelSize; x++)
            {
                for (int y = 0; y < image.Width / pixelSize; y++)
                {
                    Color pixelColor = image.GetPixel((pixelSize * y), (pixelSize * x));
                    pixelImage.SetPixel(y, x, pixelColor);
                }
            }
            DirectBitmap UpscaledImage = new DirectBitmap(pixelImage.Width * pixelSize, pixelImage.Height * pixelSize);
            for (int x = 0; x < pixelImage.Height; x++)
            {
                for (int y = 0; y < pixelImage.Width; y++)
                {
                    Color pixelColor = pixelImage.GetPixel(y, x);
                    for (int nx = 0 + x * pixelSize; nx < pixelSize + x * pixelSize; nx++)
                    {
                        for (int ny = 0 + y * pixelSize; ny < pixelSize + y * pixelSize; ny++)
                        {
                            UpscaledImage.SetPixel(ny, nx, pixelColor);
                        }
                    }
                }
            }
            return UpscaledImage;
        }

        public static DirectBitmap SwapChannel(DirectBitmap image, int channelCycle)
        {
            //Precondition: image != null, 5 >= channel >= 0
            //Postcondition: The method will return a image with flipped color channels
            DirectBitmap newimage = image;
            int r, g, b;
            for (int x = 0; x < newimage.Height; x++)
            {
                for (int y = 0; y < newimage.Width; y++)
                {
                    Color pixelColor = newimage.GetPixel(y, x);
                    Color newColor = Color.Empty;
                    r = pixelColor.R;
                    g = pixelColor.G;
                    b = pixelColor.B;
                    switch (channelCycle)
                    {
                        case 0:
                            newColor = Color.FromArgb(255, r, b, g);
                            break;
                        case 1:
                            newColor = Color.FromArgb(255, g, b, r);
                            break;
                        case 2:
                            newColor = Color.FromArgb(255, b, r, g);
                            break;
                        case 3:
                            newColor = Color.FromArgb(255, g, r, b);
                            break;
                        case 4:
                            newColor = Color.FromArgb(255, b, g, r);
                            break;
                        case 5:
                            newColor = Color.FromArgb(255, r, g, b);
                            break;
                    }
                    newimage.SetPixel(y, x, newColor);
                }
            }
            return newimage;
        }

        public static DirectBitmap Posterization(DirectBitmap image, int postMag)
        {
            //Precondition: image != null, postMag> 0
            //Postcondition: The method will return a posterized image

            DirectBitmap newimage = image;
            int r, g, b;
            int postLevel = (int)Math.Round(Math.Pow(2f, postMag));
            for (int x = 0; x < newimage.Height; x++)
            {
                for (int y = 0; y < newimage.Width; y++)
                {
                    Color pixel = newimage.GetPixel(y, x);
                    r = (pixel.R / postLevel) * postLevel;
                    g = (pixel.G / postLevel) * postLevel;
                    b = (pixel.B / postLevel) * postLevel;
                    newimage.SetPixel(y, x, Color.FromArgb(255, r, g, b));
                }
            }

            return newimage;
        }

        

        public static DirectBitmap Swirl(DirectBitmap image, int K)
        {
            //Precondition: image != null, k > 0
            //Postcondition: The method will return a swirled image

            DirectBitmap newimage = image;

            //Mid point of the image
            int midx = image.Width / 2;
            int midy = image.Height / 2;

            int[,] CoorX = new int[image.Width, image.Height];
            int[,] CoorY = new int[image.Width, image.Height];

            for (int i = 0; i < image.Width; i++)
            {

                for (int j = 0; j < image.Height; j++)
                {
                    int x = i - midx;
                    int y = j - midy;
                    double[] Polar = Cart2Pol(x, y);

                    double newangle = Polar[1];

                    int[] coord = Pol2Cart(Polar[0], Polar[1]);
                    CoorX[i, j] = coord[0]+midx;
                    CoorY[i, j] = coord[1]+midy;

                }
            }



            for (int i = 1; i < image.Width; i++)
            {

                for (int j = 1; j < image.Height; j++)
                {
                    if (CoorX[i, j] < 0)
                    {
                        CoorX[i, j] = 0;
                    }
                    else if (CoorX[i, j] > image.Width)
                    {
                        CoorX[i, j] = image.Width;
                    }
                    if (CoorY[i, j] < 0)
                    {
                        CoorY[i, j] = 0;
                    }
                    else if (CoorY[i, j] > image.Height)
                    {
                        CoorY[i, j] = image.Height;
                    }
                    Color newcolor = image.GetPixel(CoorX[i, j], CoorY[i, j]);

                    newimage.SetPixel(i, j, newcolor);

                }

            }
            return newimage;


        }

        private static double[] Cart2Pol(double x, double y)
        {
            //Precondition: radius > 0
            //Postcondition: The method will return the cartesian coordinates converted to polar

            double radius = Math.Sqrt((x * x) + (y * y));
            double o = Math.Atan(y/x);

            if (x < 0 & y > 0)
            {
                o = o + 180;
            }
            else if (x < 0 & y < 0)
            {
                o = o + 180;
            }
            else if (x > 0 & y < 0)
            {
                o = o + 360;
            }

            double[] angles = { radius, o };
            return angles;
        }

        private static int[] Pol2Cart(double angle, double radius)
        {
            //Precondition: radius > 0
            //Postcondition: The method will return the polar coordinate converted to cartesian

            double x = radius * Math.Cos(angle);
            double y = radius * Math.Sin(angle);
            int[] points = { Convert.ToInt32(Math.Round(x)), Convert.ToInt32(Math.Round(y)) };
            return points;
        }

        public static DirectBitmap FrostedGlass(DirectBitmap image, int xthreshold, int ythreshold, int scatteramount)
        {

            //Precondition: image != null, xthreshold > 0, ythreshold > 0, scatteramount;
            //Postcondition: The method will returns a wavy image

            Random random = new Random();
            DirectBitmap newimage = image;

            int x = 0, y = 0; // Setup amount of frostyness for each direction

            // for each amount of pixel placed per location
            for (int n = 0; n < scatteramount; n++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    x = random.Next(-xthreshold, xthreshold); // from highest and lowest point within the radius of x declare the insertion point along x
                    for (int j = 0; j < image.Height; j++)
                    {

                        y = random.Next(-ythreshold, ythreshold); // from highest and lowest point within the radius of y declare the insertion point along y
                        try
                        {
                            newimage.SetPixel(i + x, j + y, image.GetPixel(i, j));
                        }
                        catch (IndexOutOfRangeException e)
                        {

                        }
                    }
                    x = 0;
                    y = 0;
                }
            }
            return newimage;
        
        }

        public static DirectBitmap Blur(DirectBitmap img, int radius)
        {

            //Precondition: image != null, radius > 0;
            //Postcondition: The method will returns a blurry image

            //int SectionDiam = 64;
            if (radius <= 0)
            {
                throw new Exception("The blur radius is not greater than zero.\nPlease enter a acceptable value.");
            }
            DirectBitmap newimage = img;
            int[] Accum = new int[4];
            for (int x = 0; x < newimage.Width; x++)
            {
                for (int y = 0; y < newimage.Height; y++)
                {
                    Accum[0] = 0;
                    Accum[1] = 0;
                    Accum[2] = 0;
                    Accum[3] = 0;

                    for (int i = -radius; i <= radius; i++)
                    {
                        for (int j = -radius; j <= radius; j++)
                        {
                            try
                            {
                                Color color = img.GetPixel(x + i, y + j);
                                Accum[0] += color.R;
                                Accum[1] += color.G;
                                Accum[2] += color.B;
                                Accum[3]++;
                            }
                            catch (IndexOutOfRangeException e)
                            {

                            }
                        }
                    }
                    Accum[0] = Accum[0] / Accum[3];
                    if (Accum[0] > 255)
                    {
                        Accum[0] = 255;
                    }
                    Accum[1] = Accum[1] / Accum[3];
                    if (Accum[1] > 255)
                    {
                        Accum[1] = 255;
                    }
                    Accum[2] = Accum[2] / Accum[3];
                    if (Accum[2] > 255)
                    {
                        Accum[2] = 255;
                    }
                    newimage.SetPixel(x, y, Color.FromArgb(255, Accum[0], Accum[1], Accum[2]));
                }
            }

            return newimage;
        }

        public static DirectBitmap Cartoon(DirectBitmap image, int lineThick, int threshold)
        {
            DirectBitmap finalimage = image;
            int radius = 3;
            DirectBitmap blurimage = Blur(image, 2);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    int[] accum = new int[] { 0, 0, 0 };
                    int pixelcount = 0;
                    Color centerpixel = image.GetPixel(x, y);
                    List<Color> allcolors = new List<Color>();
                    for (int i = -radius; i < radius; i++)
                    {
                        for (int j = -radius; j < radius; j++)
                        {
                            if (i != 0 && j != 0)
                            {
                                try
                                {
                                    Color newcolor = image.GetPixel(x + i, y + j);
                                    accum[0] = newcolor.R;
                                    accum[1] = newcolor.G;
                                    accum[2] = newcolor.B;

                                    pixelcount++;
                                }
                                catch (IndexOutOfRangeException e)
                                {

                                }
                            }
                        }
                    }

                    int difference = ((accum[0] + accum[1] + accum[2]) / (3 * pixelcount)) - ((centerpixel.R + centerpixel.G + centerpixel.B) / (3*pixelcount));

                    if (Math.Abs(difference / 16) > threshold)
                    {
                        blurimage.SetPixel(x, y, Color.Black);
                    }
                }
            }
            return blurimage;
        }

        /*public static DirectBitmap NewBlur (DirectBitmap image, int radius)
        {
            int sectorDiam = 128;
            DirectBitmap newimage = image;
            List<Rectangle> renderSectors = new List<Rectangle>();
            Rectangle rect;
            for (int x = 0; x < (image.Width / sectorDiam); x++)
            {
                int xwidth = sectorDiam;
                for (int y = 0; y < (image.Height / sectorDiam); y++)
                {
                    int yheight = sectorDiam;
                    rect = new Rectangle(x * sectorDiam, y * sectorDiam, xwidth, yheight);
                    renderSectors.Add(rect);
                }
            }
            //Get data ready for threads

            var Picrect = new Rectangle(0, 0, image.Width, image.Height);
            var data = image.Bitmap.LockBits(Picrect, ImageLockMode.ReadWrite, image.Bitmap.PixelFormat);
            var depth = Bitmap.GetPixelFormatSize(data.PixelFormat) / 8; //bytes per pixel
            var buffer = new byte[data.Width * data.Height * depth];

            //Actual Threadwork
            
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            /*
            Parallel.ForEach(renderSectors, sector =>
            {
                BlurSection(buffer, sector, Picrect, radius, image.Width, image.Height, depth);
            }
            );
            

            BlurSection(buffer, renderSectors.Last(), Picrect, radius, image.Width, image.Height, depth);

            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);

            image.Bitmap.UnlockBits(data);

            return image;
        }*/

        /*public static void BlurSection(byte[] buffer, Rectangle blurSector, Rectangle bitmaprect, int radius, int width, int height, int depth)
        {

            int[] Accum = new int[4];
            for (int x = blurSector.X; x < blurSector.Width+ blurSector.X; x++)
            {
                for (int y = blurSector.Y; y < blurSector.Height + blurSector.Y; y++)
                {
                    Accum[0] = 0;
                    Accum[1] = 0;
                    Accum[2] = 0;
                    Accum[3] = 0;

                    for (int i = -radius; i <= radius; i++)
                    {
                        for (int j = -radius; j <= radius; j++)
                        {
                            var offset = 0;
                            offset = (((y + j) * width) + (x + i)) * depth;
                            if (bitmaprect.Contains(new Point(x + i, y + j)))
                            {
                                Accum[0] += buffer[offset + 0];
                                Accum[1] += buffer[offset + 1];
                                Accum[2] += buffer[offset + 2];
                                Accum[3]++;
                            
                            }
                        }
                    }
                    Accum[0] = Accum[0] / Accum[3];
                    if (Accum[0] > 255)
                    {
                        Accum[0] = 255;
                    }
                    Accum[1] = Accum[1] / Accum[3];
                    if (Accum[1] > 255)
                    {
                        Accum[1] = 255;
                    }
                    Accum[2] = Accum[2] / Accum[3];
                    if (Accum[2] > 255)
                    {
                        Accum[2] = 255;
                    }
                    var newoffset = ((y * width) + (x * depth*2));
                    buffer[newoffset + 0] = (byte)Accum[0];
                    buffer[newoffset + 1] = (byte)Accum[1];
                    buffer[newoffset + 2] = (byte)Accum[2];
                }
            }
        }*/

        public static DirectBitmap NewerBlur(DirectBitmap image, int radius)
        {
            DirectBitmap newimage = image;
            int[] Accum = new int[4];
            
            for (int x = 0; x < newimage.Width; x++)
            {
                for (int y = 0; y < newimage.Height; y++)
                {
                    Accum[0] = 0;
                    Accum[1] = 0;
                    Accum[2] = 0;
                    Accum[3] = 0;

                    for (int i = -radius; i <= radius; i++)
                    {
                        for (int j = -radius; j <= radius; j++)
                        {
                            try
                            {
                                Color color = image.GetPixel(x + i, y + j);
                                Accum[0] += color.R;
                                Accum[1] += color.G;
                                Accum[2] += color.B;
                                Accum[3]++;
                            }
                            catch (IndexOutOfRangeException e)
                            {

                            }
                        }
                    }
                    Accum[0] = Accum[0] / Accum[3];
                    if (Accum[0] > 255)
                    {
                        Accum[0] = 255;
                    }
                    Accum[1] = Accum[1] / Accum[3];
                    if (Accum[1] > 255)
                    {
                        Accum[1] = 255;
                    }
                    Accum[2] = Accum[2] / Accum[3];
                    if (Accum[2] > 255)
                    {
                        Accum[2] = 255;
                    }
                    newimage.SetPixel(x, y, Color.FromArgb(255, Accum[0], Accum[1], Accum[2]));
                }
            }

            return newimage;
        }        


        
        public static DirectBitmap NewBNW(DirectBitmap image)
        {
            int sectorDiam = 128;
            DirectBitmap newimage = image;
            List<Rectangle> renderSectors = new List<Rectangle>();
            Rectangle rect;
            for (int x = 0; x < (image.Width / sectorDiam); x++)
            {
                int xwidth = sectorDiam;
                for (int y = 0; y < (image.Height / sectorDiam); y++)
                {
                    int yheight = sectorDiam;
                    rect = new Rectangle(x * sectorDiam, y * sectorDiam, xwidth, yheight);
                    renderSectors.Add(rect);
                }
            }
            //Get data ready for threads

            var Picrect = new Rectangle(0, 0, image.Width, image.Height);
            var data = image.Bitmap.LockBits(Picrect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var depth = Bitmap.GetPixelFormatSize(data.PixelFormat) / 8; //bytes per pixel
            var buffer = new byte[data.Width * data.Height * depth];

            //Actual Threadwork
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

            Parallel.ForEach(renderSectors, sector =>
            {
               BNWSection(buffer, sector, Picrect, image.Width, image.Height, depth);
            }
            );

            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);

            image.Bitmap.UnlockBits(data);

            return image;
        }

        public static void BNWSection(byte[] buffer, Rectangle blurSector, Rectangle bitmaprect, int width, int height, int depth)
        {

            int[] Accum = new int[4];
            for (int x = blurSector.X; x < blurSector.Width + blurSector.X; x++)
            {
                for (int y = blurSector.Y; y < blurSector.Height + blurSector.Y; y++)
                {
                    int color = 0;
                    var newoffset = ((y * width) + (x * depth));
                    color = (buffer[newoffset + 1] / 255)*76;
                    color += (buffer[newoffset + 2] / 255)*128;
                    color += (buffer[newoffset + 3] / 255) * 51;

                    byte grey = Convert.ToByte(color);
                    buffer[newoffset + 1] = grey;
                    buffer[newoffset + 2] = grey;
                    buffer[newoffset + 3] = grey;

                }
            }
        }
        
        public static DirectBitmap Warble(DirectBitmap image, int warbamt, int yintensity, int xintensity)
        {

            //Precondition: image != null, warbamt > 0, yintensity > 0
            //Postcondition: The method will returns a wavy image

            if(warbamt <= 0)
            {
                throw new Exception("The amount of waves is not greater than zero.\nPlease enter a acceptable value.");
            }
            if(yintensity < 0)
            {
                throw new Exception("The amount of intensity for the waves on the y axis is not greater than zero.\nPlease enter a acceptable value.");
            }
            if (xintensity < 0)
            {
                throw new Exception("The amount of intensity for the waves on the x axis is not greater than zero.\nPlease enter a acceptable value.");
            }
            DirectBitmap newimage = image;
            int yheight = yintensity * 5;
            Rectangle picRect = new Rectangle(0, 0, image.Width, image.Height);
            for (int x =0; x < image.Width; x++)
            {
                double percentofwidth = (double)x / (double)image.Width;
                double waveheightpercent = Math.Sin(percentofwidth * Math.PI * 2 *warbamt);
                double waveheight = waveheightpercent * yheight;
                if (waveheightpercent >= 0)
                {
                    for (int y = 0; y < image.Height; y++)
                    {
                        Point offsetPoint = new Point(x, y + (int)waveheight);
                        if (picRect.Contains(offsetPoint))
                        {
                            newimage.SetPixel(x, y, image.GetPixel(offsetPoint.X, offsetPoint.Y));
                        }
                        else
                        {
                            newimage.SetPixel(x, y, image.GetPixel(offsetPoint.X, image.Height-1));
                        }
                    }
                }
                else if (waveheightpercent < 0)
                {

                    for (int y = image.Height-1; y >=0; y--)
                    {
                        Point offsetPoint = new Point(x, y + (int)waveheight);
                        if (picRect.Contains(offsetPoint))
                        {
                            newimage.SetPixel(x, y, image.GetPixel(offsetPoint.X, offsetPoint.Y));
                        }
                        else
                        {
                            newimage.SetPixel(x, y, image.GetPixel(offsetPoint.X, 0));
                        }
                    }

                }
                
            }

            DirectBitmap tempWave = newimage;
            //Horizontal warble
            
            int xheight = xintensity * 5;
            for (int y = 0; y < image.Height; y++)
            {
                double percentofheight = (double)y / (double)image.Height;
                double waveheightpercent = Math.Sin(percentofheight * Math.PI * 2 * warbamt);
                double waveheight = waveheightpercent * xheight;
                if (waveheightpercent >= 0)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Point offsetPoint = new Point(x + (int)waveheight, y);
                        if (picRect.Contains(offsetPoint))
                        {
                            newimage.SetPixel(x, y, tempWave.GetPixel(offsetPoint.X, offsetPoint.Y));
                        }
                        else
                        {
                            newimage.SetPixel(x, y, tempWave.GetPixel(image.Width - 1, offsetPoint.Y));
                        }
                    }
                }
                else if (waveheightpercent < 0)
                {

                    for (int x = image.Width - 1; x >= 0; x--)
                    {
                        Point offsetPoint = new Point(x + (int)waveheight, y );
                        if (picRect.Contains(offsetPoint))
                        {
                            newimage.SetPixel(x, y, image.GetPixel(offsetPoint.X, offsetPoint.Y));
                        }
                        else
                        {
                            newimage.SetPixel(x, y, image.GetPixel(0, offsetPoint.Y));
                        }
                    }

                }

            }

    
            return newimage;
        }

        public static DirectBitmap Mirror(DirectBitmap image, bool Side, bool WhichOrien)
        {

            //Precondition: image != null
            //Postcondition: The method will return an image with a mirror effect

            //Horizontal. true = left, false = right
            //Vertical. true = up, false = down
            DirectBitmap newimage = image;

            if (WhichOrien)
            {
                if (Side)
                {
                    for (int y = 0; y < newimage.Height; y++)
                    {
                        for (int x = 0; x < newimage.Width / 2; x++)
                        {
                            Color color = image.GetPixel(x, y);
                            newimage.SetPixel(newimage.Width - x-1, y, color);
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < newimage.Height; y++)
                    {
                        for (int x = 0; x < newimage.Width / 2; x++)
                        {
                            Color color = image.GetPixel(newimage.Width - x-1, y);
                            newimage.SetPixel(x, y, color);

                        }
                    }
                } }
            else { 
                if (Side)
                {
                    for (int x = 0; x < newimage.Width; x++)
                    {
                        for (int y = 0; y < newimage.Height / 2; y++)
                        {
                            Color color = image.GetPixel(x, y);
                            newimage.SetPixel(x, newimage.Height - y-1, color);
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < newimage.Width; x++)
                    {
                        for (int y = 0; y < newimage.Height / 2; y++)
                        {
                            Color color = image.GetPixel(x, newimage.Height - y-1);
                            newimage.SetPixel(x, y, color);

                        }
                    }
                }
            }

            return newimage;
        }

        public static DirectBitmap Rotate(DirectBitmap image,int setting)
        {
            //Precondition: image != null,
            //Postcondition: The method will return a rotated image
            DirectBitmap newimage =new DirectBitmap(image.Height, image.Width);
            switch (setting)
            {
                //90
                case 0:
                    for(int x = 0; x < image.Height; x++)
                    {
                        for(int y = 0; y < image.Width; y++)
                        {
                            newimage.SetPixel(x, y, image.GetPixel(Math.Abs(y-image.Width+1), x));
                        }
                    }
                    break;
                //-90
                case 1:
                    for (int x = 0; x < image.Height; x++)
                    {
                        for (int y = 0; y < image.Width; y++)
                        {
                            newimage.SetPixel(x, y, image.GetPixel(y, Math.Abs(x-image.Height+1)));
                        }
                    }
                    break;
            }
            return newimage;
        }

        public static DirectBitmap Sepia(DirectBitmap image)
        {
            //Precondition: image != null
            //Postcondition: The method will return a sepia image

            Color color;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    color = image.GetPixel(x, y);

                    int a = color.A;
                    int r = color.R;
                    int g = color.G;
                    int b = color.B;

                    int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                    int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                    int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);

                    if (tr > 255)
                    {
                        r = 255;
                    }
                    else
                    {
                        r = tr;
                    }

                    if (tg > 255)
                    {
                        g = 255;
                    }
                    else
                    {
                        g = tg;
                    }

                    if (tb > 255)
                    {
                        b = 255;
                    }
                    else
                    {
                        b = tb;
                    }

                    //set the new RGB value in image pixel
                    image.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            return image;
        }
    }
}
