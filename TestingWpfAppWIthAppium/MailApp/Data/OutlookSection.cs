using System.Collections.Generic;
using Telerik.Windows.Controls;

namespace MailApp
{
    public class OutlookSection : ViewModelBase
    {
        private object _selectedItem;

        public DelegateCommand Command { get; set; }

        public IEnumerable<object> Content { get; set; }

        public string Name { get; set; }

        public string IconPath { get; set; }

        public string MinimizedIconPath { get; set; }

        /// <summary>
        /// Gets or sets SelectedItem and notifies for changes
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return this._selectedItem;
            }

            set
            {
                if (this._selectedItem != value)
                {
                    this._selectedItem = value;
                    this.OnPropertyChanged(() => this.SelectedItem);
                }
            }
        }
    }
}