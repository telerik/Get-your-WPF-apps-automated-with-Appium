using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.RichTextBoxUI;
using Telerik.Windows.Controls.RichTextBoxUI.Dialogs;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using Telerik.Windows.Documents.Proofing;
using Telerik.Windows.Documents.UI.Extensibility;

namespace MailApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RadRibbonWindow
    {
        static MainWindow()
        {
            StyleManager.ApplicationTheme = new Office2013Theme();
            RadRibbonWindow.IsWindowsThemeEnabled = false;
        }

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += this.MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IconSources.ChangeIconsSet(IconsSet.Modern);
        }
    }
}
