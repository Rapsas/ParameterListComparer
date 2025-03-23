using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterListComparer
{
    internal struct CompareResults
    {
        public CompareResults()
        {
        }

        public int Score { get; set; } = 0;
        public List<string> Differences { get; set; } = new();
    }
}
