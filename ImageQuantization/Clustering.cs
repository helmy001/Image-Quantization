using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    internal class Clustering
    {
        public static List<RGBPixel> Detailed_Color;

        public static void Properties_Colors(RGBPixel[,] Colored_Image)
        {
            bool[,,] Appeared_Color = new bool[256, 256, 256];

            Detailed_Color = new List<RGBPixel>();

            int Image_Height = ImageOperations.GetHeight(Colored_Image);
            int Image_Width = ImageOperations.GetWidth(Colored_Image);
            int Height_Count = 0;
            int Width_Count = 0;

            while (Height_Count < Image_Height)
            {
                while (Width_Count < Image_Width)
                {
                    byte Red_Colored = Colored_Image[Height_Count, Width_Count].red;
                    byte Green_Colored = Colored_Image[Height_Count, Width_Count].green;
                    byte Blue_Colored = Colored_Image[Height_Count, Width_Count].blue;
                    if (Appeared_Color[Red_Colored, Green_Colored, Blue_Colored] == false)
                    {
                        Appeared_Color[Red_Colored, Green_Colored, Blue_Colored] = true;
                        Detailed_Color.Add(Colored_Image[Height_Count, Width_Count]);
                    }
                    Width_Count++;
                }
                Width_Count = 0;
                Height_Count++;
            }
        }
    }
}
