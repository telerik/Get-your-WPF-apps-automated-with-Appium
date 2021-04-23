using System.Windows;
using System.Windows.Controls;

namespace MailApp
{
    public class EmailRowStyleSelector : StyleSelector
    {
        public Style BoldStyle { get; set; }

        public Style NormalStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var email = item as Email;
            if (email != null && email.Status == Enums.EmailStatus.Unread)
            {
                return this.BoldStyle;
            }

            return this.NormalStyle;
        }
    }
}