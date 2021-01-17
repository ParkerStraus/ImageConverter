using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageConverterDemo
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Bitmap newbitmap = new Bitmap(@"C:/Users/Strau307/Pictures/jar_jar_7e1736a9.jpeg");
            Bitmap bnwimage;
            bnwimage = BlackNWhite(newbitmap);
            bnwimage.Save(@"C:/Users/Strau307/Pictures/jar_jar_bnw.jpeg");
            Console.ReadLine();
        }

        public static Bitmap BlackNWhite (Bitmap image)
        {
            Bitmap newimage = image;
            int x, y;
            for(x =0; x < newimage.Height; x++)
            {
                for(y = 0; y < newimage.Width; y++)
                {
                    Color pixelColor = newimage.GetPixel(y, x);
                    double b = 0.2126 * pixelColor.R + 0.7152 * pixelColor.G + 0.0722 * pixelColor.B;

                    Color monoColor = Color.FromArgb(255,(int)Math.Round(b), (int)Math.Round(b), (int)Math.Round(b));
                    newimage.SetPixel(y, x, monoColor);
                }
            }
            return newimage;
        } 
    }
}
