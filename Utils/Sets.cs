using System;
using System.Collections.Generic;
using System.Linq;

namespace TAiO.Subgraphs.Utils
{
    using Set = HashSet<Tuple<int, int>>;

    public static class Sets
    {
        public static Set Copy(this Set set)
        {
            return set.ToHashSet();
        }

        public static Set Empty()
        {
            return new Set();
        }
    }
}