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
        public  List<List<float>> Adjmat;       //Adjancency matrix containing all weights of edges
        public List<bool> visited;              //list of visited vertices
        public List<edge> mstEdges;             //mst tree 

        public Eager_min_Heapcs Ipq;

        public void Eager_prims(int s)
        {
            Ipq = new Eager_min_Heapcs(Detailed_Color.Count);
            int req_num_edges = Detailed_Color.Count - 1;  //number of edges = vertices -1
            int edge_count = 0;
            float mst_cost = 0;
            mstEdges = new List<edge>(new edge[req_num_edges]);             //initialize empty list for mst 
            visited = new List<bool>(new bool[Detailed_Color.Count]);    //initialize list for visited nodes

            RelaxEdgesAtNode(s);

            while (!Ipq.isEmpty() && edge_count != req_num_edges)
            {
                edge e = Ipq.GetMin();
                int NodeIndex = e.end;

                if (visited[NodeIndex])
                {
                    continue;
                }

                mstEdges[edge_count++] = e;
                mst_cost += e.cost;

                RelaxEdgesAtNode(NodeIndex);
            }

            if (edge_count != req_num_edges)
            {
                Console.WriteLine("number of edges in mst not sufficint");
            }
            else
            {
                Console.WriteLine("Eager Prims passed");
                MessageBox.Show(mst_cost.ToString());
            }
        }

        private void RelaxEdgesAtNode(int nodeIndex)
        {
            visited[nodeIndex] = true;
            
            
            for (int i = 0; i < Detailed_Color.Count; i++)
            {
                if (visited[i])
                {
                    continue;
                }

              
                if (Adjmat[nodeIndex][i] != float.MaxValue)
                {
                    edge e = new edge();
                    e.start = nodeIndex;
                    e.end = i;
                    e.cost = Adjmat[nodeIndex][i];

                    if (!Ipq.contains(e.end))
                    {
                        Ipq.insert(e.end,e);
                    }
                    else
                    {
                        Ipq.DecreaseKeyCost(e.end, e);
                    }


                }
            }
        }
        

        public  void Fill_Adjacency_Matrix()
        {
            Adjmat = new List<List<float>>();
            float temp = float.MaxValue;  

            for (int i = 0; i < Detailed_Color.Count; i++)
            {    
                Adjmat.Add(new List<float>());
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

        public  float Euclidean_Distance(RGBPixel p1,RGBPixel p2)
        {
            double dis= Math.Sqrt(((p1.red - p2.red) * (p1.red - p2.red)) + ((p1.blue - p2.blue) * (p1.blue - p2.blue)) + ((p1.green - p2.green) * (p1.green - p2.green)));
            return (float)dis;
        }
        public  void Show_Distinct_arr()
        {
            for(int i = 0;i < Detailed_Color.Count; i++)
            {
                Console.WriteLine(Detailed_Color[i].red);
            }
        }
        public void Show_mst_edges()
        {
            foreach(var e in mstEdges)
            {
                Console.WriteLine(e.start +" "+e.end);
            }
        }

        /*  public void Lazy_prims(int s)
        {
            pq = new Min_Heap();
            int req_num_edges = Detailed_Color.Count - 1;  //number of edges = vertices -1
            int edge_count = 0;
            float mst_cost = 0;
            mstEdges=new List<edge>(new edge[req_num_edges]);             //initialize empty list for mst 
            visited = new List<bool>(new bool[Detailed_Color.Count]);    //initialize list for visited nodes

            Add_Edge_to_pq(s);

            while(!pq.isEmpty() && edge_count!=req_num_edges)
            {
                edge e = pq.GetMin();
                int NodeIndex = e.end;

                if (visited[NodeIndex])
                {
                    continue;
                }

                mstEdges[edge_count++] = e;
                mst_cost+=e.cost;

                Add_Edge_to_pq(NodeIndex);                
            }

            if (edge_count != req_num_edges)
            {
                Console.WriteLine("number of edges in mst not sufficint");
            }
            else
            {
                Console.WriteLine("Lazy Prims passed");
                MessageBox.Show(mst_cost.ToString());
            }
        }
        private void Add_Edge_to_pq(int nodeIndex)
        {
            visited[nodeIndex]=true;

            for(int i = 0; i < Detailed_Color.Count; i++)
            {
                if(Adjmat[nodeIndex][i]!=float.MaxValue && visited[i] == false)
                {
                    edge e = new edge();
                    e.start = nodeIndex;
                    e.end = i;
                    e.cost = Adjmat[nodeIndex][i];
                    pq.insert(e);
                }
            }

        }*/

    }
}
