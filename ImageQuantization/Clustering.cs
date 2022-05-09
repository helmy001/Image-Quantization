using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    internal class Clustering
    {
        public List<RGBPixel> Distinct_arr=new List<RGBPixel>();  //list contains the distict colours
        public List<List<double>> Adjmat;

        public void Prims_algo()
        {
             

        }
        public int Num_Distinct_Colors()
        {

            return Distinct_arr.Count; //return the size of distinct colors list
        }
        public void Fill_Adjacency_Matrix()
        {
            Adjmat = new List<List<double>>();
            double temp = double.MaxValue;  

            for (int i = 0; i < Distinct_arr.Count; i++)
            {    
                Adjmat.Add(new List<double>());
                for(int j=0; j<Distinct_arr.Count; j++)
                {
                    Adjmat[i].Add(temp);
                    if (i != j)
                    {
                        Adjmat[i][j]=Euclidean_Distance(Distinct_arr[i], Distinct_arr[j]);
                    }
                    
                }

            }
        }
        public double Euclidean_Distance(RGBPixel p1,RGBPixel p2)
        {
            double dist = Math.Sqrt(((p1.red - p2.red) * (p1.red - p2.red)) + ((p1.blue - p2.blue) * (p1.blue - p2.blue)) + ((p1.green - p2.green) * (p1.green - p2.green)));
            return dist;
        }

        public  void Show_Distinct_arr()
        {
            for(int i = 0;i < Distinct_arr.Count; i++)
            {
                Console.WriteLine(Distinct_arr[i].red);
            }
        }
    }
}
