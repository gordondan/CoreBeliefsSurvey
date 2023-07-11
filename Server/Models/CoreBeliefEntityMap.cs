using CsvHelper.Configuration;
using System;

namespace CoreBeliefsSurvey.Server.Models
{
    public class CoreBeliefEntityMap : ClassMap<CoreBeliefEntity>
    {
        public CoreBeliefEntityMap()
        {
            Map(m => m.IsPositive).Convert(args =>
            {
                var isPositiveStr = args.Row.GetField("Orientation").Trim();
                return isPositiveStr.Equals("Positive", StringComparison.OrdinalIgnoreCase);
            });

            Map(m => m.BeliefName).Name("BeliefName");
            Map(m => m.BeliefDescription).Name("Belief Description");
            Map(m => m.PartitionKey).Name("PartitionKey");
            Map(m => m.RowKey).Name("RowKey");
        }
    }


}
