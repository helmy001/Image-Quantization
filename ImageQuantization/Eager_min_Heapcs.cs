using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    

    internal class Eager_min_Heapcs
    {

        public Dictionary<int, edge> pq;
        public List<int> pos_map;               //contains keys of the mst is at any index
        public List<int> inverse_map;       //contains index in the mst at any key         
        private int size=0;

        public Eager_min_Heapcs(int num_vert)
        {
            pq= new Dictionary<int, edge>();
            pos_map = new List<int>(new int[num_vert+1]);
            inverse_map = new List<int>(new int[num_vert+1]);
            pos_map[0] = -1;
        }
        private int parent(int idx)
        {
            return idx >> 1; //  = idx/2
        }
        private int left(int idx)
        {
            return idx << 1; // = idx *2
        }
        private int right(int idx)
        {
            return (idx << 1) + 1; // = idx *2 +1
        }

        private void SwapNum(int idx1, int idx2)
        {

            //swap idx in postition arr

            int temp = pos_map[idx1] ;
            pos_map[idx1] = pos_map[idx2];
            pos_map[idx2] = temp;

            //swap keys in invere array
            inverse_map[pos_map[idx1]] = idx1;
            inverse_map[pos_map[idx2]] = idx2;
        }

        public bool isEmpty()
        {
            if (size == 0)
                return true;

            return false;
        }

        public void insert(int ki,edge e)
        {
            if (size + 1 >= pq.Count())
            {
                pq.Add(ki,new edge());
            }

            pq[ki] = e;
            pos_map[++size] = ki;
            inverse_map[ki] = size;
            shiftUp(size);
            return;


        }
        private void shiftUp(int idx)
        {
            if (idx > size) return; // if the idx passed is not valid

            if (idx == 1) return; //Base case

            if (pq[pos_map[idx]].cost < pq[pos_map[parent(idx)]].cost)
            {
                SwapNum(idx, parent(idx));

            }
            shiftUp(parent(idx));


        }
        private void shiftDown(int idx)
        {

            if (idx > size) return;


            int swapIdx = idx;
            if (left(idx) <= size && pq[pos_map[idx]].cost > pq[pos_map[left(idx)]].cost)
            {
                swapIdx = left(idx);
            }
            if (right(idx) <= size && pq[pos_map[swapIdx]].cost > pq[pos_map[right(idx)]].cost)
            {
                swapIdx = right(idx);
            }

            if (swapIdx != idx)
            {
                SwapNum(idx, swapIdx);
                shiftDown(swapIdx);
            }
            return;

        }

        public edge GetMin()
        {
            edge min = new edge(-1, -1, -1);
            if (size > 0)
            {
                min = pq[pos_map[1]];
                SwapNum(1, size--);
                shiftDown(1);
                return min;
            }
            return min;
        }
        public bool contains(int nodeidx)
        {
            return pq.ContainsKey(nodeidx);  
        }

        public void DecreaseKeyCost(int ki,edge e2)
        {
            edge e1=pq[ki];

            if(e1.cost > e2.cost)
            {
                pq[ki]=e2;
                shiftUp(inverse_map[ki]);

            }
            return;
        }

    }
}
