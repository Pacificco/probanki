using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartsApi
{
    public class ChartObject
    {
        public int SubjectId { get; set; }
        public DateTime Date { get; set; }
        public decimal close { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal open { get; set; }
        public decimal volume { get; set; }
    }
}
