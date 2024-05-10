using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Utility
{
    public class Utils
    {
        public static string ShowAlert(Alerts obj, string message)
        {
            string alertDiv = null;

            switch (obj)
            {
                case Alerts.Success:
                    alertDiv = "";
                    break;
                case Alerts.Error:
                    alertDiv = "";
                    break;
                case Alerts.Info:
                    alertDiv = "";
                    break;
                case Alerts.Warning:
                    alertDiv = "";
                    break;
            }

            return alertDiv;
        }
    }
}