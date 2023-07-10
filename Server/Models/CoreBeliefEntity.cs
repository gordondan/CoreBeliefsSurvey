namespace CoreBeliefsSurvey.Server.Models
{
    public class CoreBeliefEntity : AzureTableRow
    {
        public bool IsPositive { get; set; }
        public string BeliefName { get; set; }
        public string BeliefDescription { get; set; }
    }
}
