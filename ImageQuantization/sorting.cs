using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    internal class sorting
    {
        public int Compare(edge x, edge y)
        {
            if (x.cost > y.cost)
            {
                return 1;
            }
            else if (x.cost < y.cost)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        }
    }
}
