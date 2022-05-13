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
        public List<bool> visited;              //list of visited vertices
        public List<bool> visited2;
        public List< List<int> > mstEdges;             //mst tree 
        public List<RGBPixel> Palette;
        public Eager_min_Heapcs Ipq;
        public  Dictionary<int,bool> not_visited;
        private Min_Heap pq;
        private max_Heap max_cost;
        RGBLong rgb=new RGBLong(0,0,0);
        int counter = 0;


        public void Properties_Colors(RGBPixel[,] Colored_Image)
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

       
 
        public void show_palette()
        {
            MessageBox.Show("number of colors in the Palette :" +Palette.Count);

            
            foreach(var p in Palette)
            {
                Console.WriteLine(p.red+" "+p.green+" "+p.blue);
            }
        }
        public  float Euclidean_Distance(RGBPixel p1,RGBPixel p2)
        {
            double dis= Math.Sqrt(((p1.red - p2.red) * (p1.red - p2.red)) + ((p1.blue - p2.blue) * (p1.blue - p2.blue)) + ((p1.green - p2.green) * (p1.green - p2.green)));
            return (float)dis;
        }



        public void K_Clusters(int k)
        {
            not_visited = new Dictionary<int, bool>();
            visited2 = new List<bool>(new bool[Detailed_Color.Count]);
            Palette = new List<RGBPixel>();
            int number_of_cuts = k - 1;
            while (number_of_cuts > 0)
            {
                edge e = max_cost.GetMax();
                if (not_visited.ContainsKey(e.start) == false)
                    not_visited.Add(e.start, false);
                if (not_visited.ContainsKey(e.end) == false)
                    not_visited.Add(e.end, false);
                mstEdges[e.end].Remove(e.start);
                mstEdges[e.start].Remove(e.end);
                number_of_cuts--;
            }

 
      
         
            for(int i=0; i<mstEdges.Count;i++)
            {
                rgb.red = 0;
                rgb.green = 0;
                rgb.blue = 0;
                counter = 0;
                if (visited2[i] == false)
                {
                    Next_node(i);
                    RGBPixel color = new RGBPixel((byte)(rgb.red / counter), (byte)(rgb.green / counter), (byte)(rgb.blue / counter));
                    Palette.Add(color);
                }
            }
                

        }
        public void Next_node(int node)
        {
            counter++;
            visited2[node]=true;
            rgb.red += Detailed_Color[node].red;
            rgb.green += Detailed_Color[node].green;
            rgb.blue += Detailed_Color[node].blue;
           

            for(int i=0;i<mstEdges[node].Count;i++)
            {
                if (visited2[mstEdges[node][i]] == false)
                {
                    Next_node(mstEdges[node][i]);
                }
            }
            return;
        }






        ////////////////////Eager Prims  /////////////////////////
        public void Eager_prims(int s)
        {
            int req_num_edges = Detailed_Color.Count - 1;  //number of edges = vertices -1
            Ipq = new Eager_min_Heapcs(Detailed_Color.Count);
            mstEdges = new List<List<int >>();             //initialize empty list for mst 
            visited = new List<bool>(new bool[Detailed_Color.Count]);    //initialize list for visited nodes
            max_cost = new max_Heap();

            for(int i=0;i<Detailed_Color.Count;i++)
                mstEdges.Add(new List<int>());

            int edge_count = 0;
            float mst_cost = 0;

            RelaxEdgesAtNode(s);

            while (!Ipq.isEmpty() && edge_count != req_num_edges)
            {
                edge e = Ipq.GetMin();
                int NodeIndex = e.end;

                if (visited[NodeIndex])
                {
                    continue;
                }

                mstEdges[e.start].Add(e.end);
                mstEdges[e.end].Add(e.start);
                edge_count++;
                max_cost.insert(e);
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

                edge e = new edge();
                e.start = nodeIndex;
                e.end = i;
                e.cost = Euclidean_Distance(Detailed_Color[e.start],Detailed_Color[e.end]);

                if (!visited[i])
                {
                    

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

        /////////////////////////////////////////////////////////


        /**************** Lazy Prims Implementation ********/
        public void Lazy_prims(int s)
        {
            pq = new Min_Heap();
            int req_num_edges = Detailed_Color.Count - 1;  //number of edges = vertices -1
            int edge_count = 0;
            float mst_cost = 0;
            mstEdges=new List<List<int>>();     //initialize empty adjacency list for mst 

            for (int i = 0; i < Detailed_Color.Count; i++)
                mstEdges.Add(new List<int>());

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

                mstEdges[e.start].Add(e.end);
                edge_count++;
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
                if(visited[i] == false)
                {
                    edge e = new edge();
                    e.start = nodeIndex;
                    e.end = i;
                    e.cost = Euclidean_Distance(Detailed_Color[e.start], Detailed_Color[e.end]);
                    pq.insert(e);
                }
            }

        }
        /**************************************************/
    }
}
