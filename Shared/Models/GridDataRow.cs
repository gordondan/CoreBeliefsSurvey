using Telerik.SvgIcons;

namespace CoreBeliefsSurvey.Shared.Models
{
    public class GridDataRow
    {
        public string Belief { get; set; }
        public int SelectedValue { get; set; }
        public bool IsPositive { get; set; }
        public string SelectedValueText => GetResponseText();
        public string Icon => GetIconBasedOnSelectedValue();
        public int Positivity => GetPositivity();
        public string BackgroundColor => GetBackgroundColor();

        private int GetPositivity()
        {
            if (IsPositive)
            {
                return SelectedValue;
            }
            else
            {
                return 6 - SelectedValue;
            }
        }
        private string GetIconBasedOnSelectedValue()
        {
            switch (Positivity)
            {
                case 1:
                    return "☹️"; // Disagree
                case 2:
                    return "😐"; // Somewhat Disagree
                case 3:
                    return "😑"; // Neutral
                case 4:
                    return "🙂"; // Somewhat Agree
                case 5:
                    return "😄"; // Agree
                default:
                    return "?"; // Default icon
            }
        }

        private string GetResponseText()
        {
            string text = string.Empty;

            if (SelectedValue == 1)
            {
                text = "Disagree";
            }
            else if (SelectedValue == 2)
            {
                text = "Somewhat Disagree";
            }
            else if (SelectedValue == 3)
            {
                text = "Neutral";
            }
            else if (SelectedValue == 4)
            {
                text = "Somewhat Agree";
            }
            else if (SelectedValue == 5)
            {
                text = "Agree";
            }

            return text;
        }

        private string GetBackgroundColor()
        {
            switch (Positivity)
            {
                case 1:
                    return "red"; // Red background color
                case 2:
                    return "orange"; // Orange background color
                case 3:
                    return "white"; // White background color
                case 4:
                    return "yellow"; // Yellow background color
                case int value when value >= 5:
                    return "green"; // Green background color
                default:
                    return string.Empty;
            }
        }
    }
}
