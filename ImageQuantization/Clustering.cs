using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageQuantization
{


    internal class Clustering
    {


        public  List<RGBPixel> Detailed_Color;           //list contains the distict colours
        public List<bool> visited;                      //list of visited vertices
        public List<bool> Added_In_Cluster;    
        public List< List<int> > Mst;                 // Mst Adjacency List 
        public List<RGBPixel> Palette;                //list of new colours
        public Eager_min_Heap Ipq;                   //Eager min Indexed Prority queue
        private max_Heap max_cost;                  //Max Prority Queue
        RGBPixel[,,] colors_mapping;
        RGBLong rgb=new RGBLong(0,0,0);
        int nodes_count = 0;



        ////// Distinct Colours //////////////////////////////
        public int Properties_Colors(RGBPixel[,] Colored_Image)
        {
            bool[,,] Appeared_Color = new bool[256, 256, 256];

            Detailed_Color = new List<RGBPixel>();

            int Image_Height =Colored_Image.GetLength(0);
            int Image_Width = Colored_Image.GetLength(1);
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
                        Detailed_Color.Add(new RGBPixel(Red_Colored,Green_Colored,Blue_Colored));
                    }
                    
                    Width_Count++;
                }
                Width_Count = 0;
                Height_Count++;
            }

            return Detailed_Color.Count;
            
            
        }
      
        ///////////////////////////////////////////////////////
 
       

        /********************K Clusters **********************/
        public void K_Clusters(int k)
        {

            colors_mapping= new RGBPixel[256, 256, 256];
            Added_In_Cluster = new List<bool>(new bool[Detailed_Color.Count]);
            Palette = new List<RGBPixel>();
            int number_of_cuts = k - 1;

            while (number_of_cuts > 0 && k<Detailed_Color.Count)                //O(K-1)
            {
                edge e = max_cost.GetMax();                                     //O(1)
                Mst[e.end].Remove(e.start);                                     //O(1)
                Mst[e.start].Remove(e.end);                                     //O(1)
                number_of_cuts--;                                               //O(1)
            }

            List<int> vertices;
            for(int i=0; i<Mst.Count;i++)                                       //O(D)    
            {
                rgb.red = 0;
                rgb.green = 0;
                rgb.blue = 0;
                nodes_count = 0;
                if (Added_In_Cluster[i] == false)                               //O(1)                          
                {
                    vertices = new List<int>();                                 //O(1)
                    Next_node(i,vertices);                                      

                    RGBPixel color = new RGBPixel((byte)(rgb.red / nodes_count), (byte)(rgb.green / nodes_count), (byte)(rgb.blue / nodes_count));

                    Palette.Add(color);                                         //O(1)                                                                                     
                    mapping_k_colors(vertices, color);                             
                }
            }
                
            
        }
        public void Next_node(int node,List <int > vs)
        {
            nodes_count++;                                                      //O(1)
            Added_In_Cluster[node]=true;                                        //O(1)
            rgb.red += Detailed_Color[node].red;                                //O(1)        
            rgb.green += Detailed_Color[node].green;                            //O(1)
            rgb.blue += Detailed_Color[node].blue;                              //O(1)
            vs.Add(node);                                                       //O(1)

            for (int i=0;i<Mst[node].Count;i++)                                 //O(D)
            {
                if (Added_In_Cluster[Mst[node][i]] == false)                    //O(1)
                {
                    Next_node(Mst[node][i],vs);
                }
            }
            return;
        }
        public void mapping_k_colors(List<int> vs,RGBPixel color)
        {
            foreach(int i in vs)
            {
                colors_mapping[Detailed_Color[i].red,Detailed_Color[i].green,Detailed_Color[i].blue] = color;
            }
        }

        ////////////////////////////////////////////////////////
 




        ////////////////////Eager Prims  /////////////////////////
        public float Eager_prims(int s)
        {
            int req_num_edges = Detailed_Color.Count - 1;                 //number of edges = vertices -1
            Ipq = new Eager_min_Heap(Detailed_Color.Count);               //initialize Indxed Priority Queue
            Mst = new List<List<int >>();                            //initialize empty list for mst 
            visited = new List<bool>(new bool[Detailed_Color.Count]);     //initialize list for visited nodes
            max_cost = new max_Heap();                                    //initialize max Priority Queue for Clustering
            int edge_counter = 0;
            float mst_cost = 0;


            for(int i=0;i<Detailed_Color.Count;i++)                      // O(D) 
                Mst.Add(new List<int>());


            Add_Edges(s);                                        // O(DLog(V))


            while (!Ipq.isEmpty() && edge_counter != req_num_edges)     // O(E+DLog(V))
            {
                edge e = Ipq.GetMin();
                int NodeIndex = e.end;

                if (visited[NodeIndex])                                   //O(1)
                {
                    continue;
                }                                      

                Mst[e.start].Add(e.end);                             // O(1)
                Mst[e.end].Add(e.start);                             // O(1)
                edge_counter++;                                            
                max_cost.insert(e);                                 //O(Log(V))
                mst_cost += e.cost;                                //O(1)

                Add_Edges(NodeIndex);                            //O(DLog(V))
            }

            if (edge_counter != req_num_edges)
            {
                Console.WriteLine("number of edges in mst not sufficint");
            }
            else
            {
                Console.WriteLine("Eager Prims passed");
                return mst_cost;
            }
            return 0;
        }

        private void Add_Edges(int nodeIndex)
        {
            visited[nodeIndex] = true;                                    //O(1)  

            for (int i = 0; i < Detailed_Color.Count; i++)              //O(DLog(V))
            {

                edge e = new edge();
                e.start = nodeIndex;
                e.end = i;
                e.cost = Euclidean_Distance(Detailed_Color[e.start],Detailed_Color[e.end]);             //O(1)

                if (!visited[i])                                //O(1)
                {
                    

                    if (Ipq.inverse_map[e.end]==0)              //O(1)
                    {
                        Ipq.insert(e.end,e);                    //O(Log(V))
                    }
                    else
                    {
                        Ipq.DecreaseKeyCost(e.end, e);          //O(Log(V))
                    }


                }
            }
        }


        /////////////////////////////////////////////////////////


        public RGBPixel[,] Quantization(RGBPixel[,] Org_Image)
        {
            int Image_Height = Org_Image.GetLength(0);
            int Image_Width = Org_Image.GetLength (1);
            
            for (int i = 0; i < Image_Height; i++)
            {
                for (int j = 0; j < Image_Width; j++)
                {
                    Org_Image[i,j]= colors_mapping[Org_Image[i,j].red, Org_Image[i, j].green, Org_Image[i, j].blue];
                }
            }
            return Org_Image;
        }


        public  float Euclidean_Distance(RGBPixel p1,RGBPixel p2)
        {
            double dis= Math.Sqrt(((p1.red - p2.red) * (p1.red - p2.red)) + ((p1.blue - p2.blue) * (p1.blue - p2.blue)) + ((p1.green - p2.green) * (p1.green - p2.green)));
            return (float)dis;
        }


    }
}
