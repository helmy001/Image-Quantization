using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    internal class Min_Heap
    {
        private int size;
        private List<int> vect=new List<int>() {-1} ;

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

            int temp = vect[idx1];
            vect[idx1] = vect[idx2];
            vect[idx2] = temp;
        }

        public bool isEmpty()
        {
            return size == 0;
        }
       
        public void insert(int val)
        {
            if(size+1 >= vect.Count())
            {
                vect.Add(0);
            }

            vect[++size] = val;
            shiftUp(size);
            return;


        }
        public void shiftUp(int idx)
        {
            if (idx > size) return; // if the idx passed is not valid

            if (idx == 1) return; //Base case

            if (vect[idx] < vect[parent(idx)] )
            {
                SwapNum(idx,parent(idx));
                
            }
            shiftUp(parent(idx));


        }
        public void shiftDown(int idx)
        {

            if(idx > size) return;
           

            int swapIdx = idx;
            if(left(idx) <= size && vect[idx]>vect[left(idx)]) 
            {
                swapIdx = left(idx);
            }
            if (right(idx) <= size && vect[swapIdx] > vect[right(idx)])
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
         
        public int GetMin()
        {
            if (size > 0)
            {
                int min = vect[1];
                SwapNum(1, size--);
                shiftDown(1);
                return min;
            }
            return -1;
        }

        



    }
}
