using System.Windows;

namespace MailApp
{
    public static class ConditionalDockingHelper
    {
        public static readonly DependencyProperty PaneTypeProperty =
           DependencyProperty.RegisterAttached("PaneType", typeof(Enums.PaneType), typeof(ConditionalDockingHelper), new PropertyMetadata(Enums.PaneType.Normal));

        public static Enums.PaneType GetPaneType(DependencyObject obj)
        {
            return (Enums.PaneType)obj.GetValue(PaneTypeProperty);
        }

        public static void SetPaneType(DependencyObject obj, Enums.PaneType value)
        {
            obj.SetValue(PaneTypeProperty, value);
        }
    }
}