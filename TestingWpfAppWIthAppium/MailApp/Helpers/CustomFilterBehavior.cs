using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace MailApp
{
    public class CustomFilterBehavior
    {
        private static DispatcherTimer _timer;
        private readonly RadGridView _gridView;
        private readonly RadWatermarkTextBox _textBlock;
        private readonly RadBusyIndicator _busyIndicator;
        private CustomFilterDescriptor _filterDescriptor;

        static CustomFilterBehavior()
        {
            CustomFilterBehavior._timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
            };
        }

        public static readonly DependencyProperty TextBoxProperty =
            DependencyProperty.RegisterAttached("TextBox", typeof(TextBox), typeof(CustomFilterBehavior),
            new PropertyMetadata(new PropertyChangedCallback(OnTextBoxPropertyChanged)));

        public CustomFilterDescriptor FilterDescriptor
        {
            get
            {
                if (this._filterDescriptor == null)
                {
                    this._filterDescriptor = new CustomFilterDescriptor(this._gridView.Columns.OfType<Telerik.Windows.Controls.GridViewColumn>());
                    this._gridView.FilterDescriptors.Add(this._filterDescriptor);
                }
                return this._filterDescriptor;
            }
        }

        public static void SetTextBox(DependencyObject dependencyObject, TextBox tb)
        {
            dependencyObject.SetValue(TextBoxProperty, tb);
        }

        public static TextBox GetTextBox(DependencyObject dependencyObject)
        {
            return (TextBox)dependencyObject.GetValue(TextBoxProperty);
        }

        public static void OnTextBoxPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var gridView = dependencyObject as RadGridView;
            var textBlock = e.NewValue as RadWatermarkTextBox;
            var busyIndicator = gridView.ParentOfType<RadBusyIndicator>();

            if (gridView != null && textBlock != null)
            {
                var behavior = new CustomFilterBehavior(gridView, textBlock, busyIndicator);
            }
        }

        public CustomFilterBehavior(RadGridView gridView, RadWatermarkTextBox textBlock, RadBusyIndicator busyIndicator)
        {
            this._gridView = gridView;
            this._textBlock = textBlock;
            this._busyIndicator = busyIndicator;

            this._textBlock.TextChanged -= this.OnTextBlockTextChanged;
            this._textBlock.TextChanged += this.OnTextBlockTextChanged;
        }

        private void SetStatusBusyIndicator(bool isBusy)
        {
            if (this._busyIndicator != null)
            {
                this._busyIndicator.IsBusy = isBusy;
            }
        }

        private void OnTextBlockTextChanged(object sender, TextChangedEventArgs e)
        {
            if (CustomFilterBehavior._timer != null && CustomFilterBehavior._timer.IsEnabled)
            {
                CustomFilterBehavior._timer.Stop();
                CustomFilterBehavior._timer.Start();
            }
            else
            {
                if (CustomFilterBehavior._timer != null)
                {
                    CustomFilterBehavior._timer.Start();
                    CustomFilterBehavior._timer.Tick += this.OnTimerTick;
                }
            }

            this.SetStatusBusyIndicator(true);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            CustomFilterBehavior._timer.Stop();
            this.SetStatusBusyIndicator(false);
            this.FilterDescriptor.FilterValue = this._textBlock.Text;
            this._textBlock.Focus();
        }
    }
}