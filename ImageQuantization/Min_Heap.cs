using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{

    public struct edge
    {
        public RGBPixel start;
        public RGBPixel end;
        public int cost;
        public edge(RGBPixel start, RGBPixel end, int cost)
        {
            this.start = start;
            this.end = end;
            this.cost = cost;
        }
    }

    internal class Min_Heap
    {
       
        private int size;
        private List<edge> pq=new List<edge>() {new edge(new RGBPixel(0,0,0), new RGBPixel(0,0,0),-1)};
        
        private int parent(int idx) 
        {
            return idx>>1; //  = idx/2
        }
        private int left(int idx)
        {
            return idx << 1; // = idx *2
        }
        private int right(int idx)
        {
            return (idx << 1 )+1; // = idx *2 +1
        }

        public void SwapNum(int idx1,int idx2)
        {

            edge temp = pq[idx1];
            pq[idx1] = pq[idx2];
            pq[idx2] = temp;
        }

        public bool isEmpty()
        {
            return size == 0;
        }
       
        public void insert(edge e)
        {
            if(size+1 >= pq.Count())
            {
                pq.Add(e);
            }

            pq[++size] = e;
            shiftUp(size);
            return;


        }
        public void shiftUp(int idx)
        {
            if (idx > size) return; // if the idx passed is not valid

            if (idx == 1) return; //Base case

            if (pq[idx].cost < pq[parent(idx)].cost )
            {
                SwapNum(idx,parent(idx));
                
            }
            shiftUp(parent(idx));


        }
        public void shiftDown(int idx)
        {

            if(idx > size) return;
           

            int swapIdx = idx;
            if(left(idx) <= size && pq[idx].cost>pq[left(idx)].cost) 
            {
                swapIdx = left(idx);
            }
            if (right(idx) <= size && pq[swapIdx].cost > pq[right(idx)].cost)
            {
                swapIdx = right(idx);
            }

            if (swapIdx != idx)
            {
                SwapNum(idx,swapIdx);
                shiftDown(swapIdx);
            }
            return;

        }
         
        public edge GetMin()
        {
            edge min=new edge(new RGBPixel(0,0,0), new RGBPixel(0, 0, 0), -1);
            if (size > 0)
            {
                min = pq[1];
                SwapNum(1, size--);
                shiftDown(1);
                return min;
            }
            return min ;
        }

        



    }
}
