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
                    alertDiv = $"<div class='alert alert-success alert-dismissible' id='alert'><button type='button' class='close' data-dismiss='alert'>X</button><strong> </ strong > {message}</a>.</div>";
                    break;
                case Alerts.Error:
                    alertDiv = $"<div class='alert alert-danger alert-dismissible' id='alert'><button type='button' class='close' data-dismiss='alert'>X</button><strong> </ strong > {message}</a>.</div>"; ;
                    break;
                case Alerts.Info:
                    alertDiv = $"<div class='alert alert-info alert-dismissible' id='alert'><button type='button' class='close' data-dismiss='alert'>X</button><strong> </ strong > {message}</a>.</div>";
                    break;
                case Alerts.Warning:
                    alertDiv = $"<div class='alert alert-warning alert-dismissible' id='alert'><button type='button' class='close' data-dismiss='alert'>X</button><strong> </ strong > {message}</a>.</div>"; ;
                    break;
            }

            return alertDiv;
        }
    }
}