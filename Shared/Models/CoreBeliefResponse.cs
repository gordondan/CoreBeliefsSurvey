using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBeliefsSurvey.Shared.Models
{
    public class CoreBeliefResponse
    {
        public int SelectedValue { get; set; }
        public CoreBelief Belief { get; set; }
    }
}
