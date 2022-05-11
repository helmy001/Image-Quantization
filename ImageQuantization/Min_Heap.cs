using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{

    public struct edge
    {
        public int start;
        public int end;
        public float cost;
        public edge(int start, int end, float cost)
        {
            this.start = start;
            this.end = end;
            this.cost = cost;
        }
    }

    internal class Min_Heap
    {
       
        private int size;
        private List<edge> pq=new List<edge>() {new edge(-1,-1,-1)};
        
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

        private void SwapNum(int idx1,int idx2)
        {

            edge temp = pq[idx1];
            pq[idx1] = pq[idx2];
            pq[idx2] = temp;
        }

        public bool isEmpty()
        {
            if(size==0)
                return true;

            return false;
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
        private void shiftUp(int idx)
        {
            if (idx > size) return; // if the idx passed is not valid

            if (idx == 1) return; //Base case

            if (pq[idx].cost < pq[parent(idx)].cost )
            {
                SwapNum(idx,parent(idx));
                
            }
            shiftUp(parent(idx));


        }
        private void shiftDown(int idx)
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
            edge min=new edge(-1,-1, -1);
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
