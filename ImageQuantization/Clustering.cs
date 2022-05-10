using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    internal class Clustering
    {

        public  List<RGBPixel> Detailed_Color;    //list contains the distict colours
        public  List<List<double>> Adjmat;       //Adjancency matrix containing all weights of edges

        public  void Prims_algo()
        {
            
        }


        public  void Fill_Adjacency_Matrix()
        {
            Adjmat = new List<List<double>>();
            double temp = double.MaxValue;  

            for (int i = 0; i < Detailed_Color.Count; i++)
            {    
                Adjmat.Add(new List<double>());
                for(int j=0; j< Detailed_Color.Count; j++)
                {
                    Adjmat[i].Add(temp);
                    if (i != j)
                    {
                        Adjmat[i][j]=Euclidean_Distance(Detailed_Color[i], Detailed_Color[j]);
                    }
                   
                }
                
            }
        }
        public  void Properties_Colors(RGBPixel[,] Colored_Image)
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

            MessageBox.Show($"Number of distinct colors : {Detailed_Color.Count.ToString()}");
            
            
        }

        public  double Euclidean_Distance(RGBPixel p1,RGBPixel p2)
        {
            double dist = Math.Sqrt(((p1.red - p2.red) * (p1.red - p2.red)) + ((p1.blue - p2.blue) * (p1.blue - p2.blue)) + ((p1.green - p2.green) * (p1.green - p2.green)));
            return dist;
        }

        public  void Show_Distinct_arr()
        {
            for(int i = 0;i < Detailed_Color.Count; i++)
            {
                Console.WriteLine(Detailed_Color[i].red);
            }
        }

    }
}
