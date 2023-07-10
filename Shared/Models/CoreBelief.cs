using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBeliefsSurvey.Shared.Models
{
    public class CoreBelief
    {
        public CoreBelief() { }

        public bool IsPositive { get; set; }
        public string BeliefName { get; set; }
        public string BeliefDescription { get; set; }
    }
}
