using System;
using System.Windows;
using Telerik.Windows.Controls;

namespace MailApp
{
    public class OutlookBarExtensions
    {
        /// <summary>
        /// Determines if the Width of the RadOutlookBar instance to which it is set is lined to the Width of its parent RadSplitContainer.
        /// </summary>
        public static readonly DependencyProperty IsWidthLinkedProperty =
            DependencyProperty.RegisterAttached("IsWidthLinked", typeof(bool), typeof(OutlookBarExtensions), new PropertyMetadata(false, OnIsWidthLinkedChanged));

        public static bool GetIsWidthLinked(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsWidthLinkedProperty);
        }

        public static void SetIsWidthLinked(DependencyObject obj, bool value)
        {
            obj.SetValue(IsWidthLinkedProperty, value);
        }

        private static void OnIsWidthLinkedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var outlookBar = d as RadOutlookBar;
            if ((bool)e.NewValue)
            {
                outlookBar.Restored += OnOutlookBarRestored;
                outlookBar.Minimized += OnOutlookBarMinimized;
            }
        }

        private static void OnOutlookBarMinimized(object sender, RoutedEventArgs e)
        {
            var outLookBar = (sender as RadOutlookBar);
            if (outLookBar != null)
            {
                var splitContainer = outLookBar.ParentOfType<RadSplitContainer>();
                if (splitContainer != null)
                {
                    splitContainer.Width = outLookBar.Width;
                }

                UpdateRestrictedPanes(outLookBar, false);
            }
        }

        private static void OnOutlookBarRestored(object sender, RoutedEventArgs e)
        {
            var outLookBar = (sender as RadOutlookBar);
            if (outLookBar != null)
            {
                var splitContainer = outLookBar.ParentOfType<RadSplitContainer>();
                if (splitContainer != null)
                {
                    // Sets the default size of a RadPane in the RadDocking control and default RadOutlookBar Width
                    splitContainer.Width = 240;
                    outLookBar.Width = Double.NaN;
                }

                UpdateRestrictedPanes(outLookBar, true);
            }
        }

        private static void SyncWidth(object sender)
        {
            var outLookBar = (sender as RadOutlookBar);
            if (outLookBar != null)
            {
                var splitContainer = outLookBar.ParentOfType<RadSplitContainer>();
                if (splitContainer != null)
                {
                    splitContainer.Width = outLookBar.Width;
                }
            }
        }

        private static void UpdateRestrictedPanes(RadOutlookBar outLookBar, bool value)
        {
            var paneGroup = outLookBar.ParentOfType<RadPaneGroup>();
            if (paneGroup != null)
            {
                foreach (var pane in paneGroup.EnumeratePanes())
                {
                    if (ConditionalDockingHelper.GetPaneType(pane) == Enums.PaneType.Restricted)
                    {
                        pane.CanUserPin = value;
                        pane.CanFloat = value;
                    }
                }
            }
        }
    }
}