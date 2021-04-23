using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Telerik.Windows.Data;

namespace MailApp
{
    public class Folder : INotifyPropertyChanged
    {
        private QueryableCollectionView _emails;
        private IEnumerable<Folder> _folders;
        private string _name;
        private int _unreadEmailsCount = -1;

        public Folder()
        {
            this.Folders = new List<Folder>();
        }

        /// <summary>
        /// Gets or sets Folders and notifies for changes
        /// </summary>
        public IEnumerable<Folder> Folders
        {
            get
            {
                return this._folders;
            }

            set
            {
                if (this._folders != value)
                {
                    this._folders = value;
                    this.OnPropertyChanged("Folders");
                }
            }
        }

        /// <summary>
        /// Gets or sets Name and notifies for changes
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets or sets Emails and notifies for changes
        /// </summary>
        public QueryableCollectionView Emails
        {
            get
            {
                return this._emails;
            }

            set
            {
                if (this._emails != value)
                {
                    this._emails = value;
                    this.OnPropertyChanged("Emails");
                }
            }
        }

        /// <summary>
        /// Gets the number of unread Email objects of the Folder.
        /// </summary>
        public int UnreadEmailsCount
        {
            get
            {
                if (this._unreadEmailsCount == -1)
                {
                    this._unreadEmailsCount = this.GetUnreadEmailsCount();
                }
                return this._unreadEmailsCount;
            }
            private set
            {
                if (this._unreadEmailsCount != value)
                {
                    this._unreadEmailsCount = value;
                    this.OnPropertyChanged("UnreadEmailsCount");
                }
            }
        }

        /// <summary>
        /// Updates the count of the unread email object of the Folder object.
        /// </summary>
        public void UpdateUnreadEmailsCount()
        {
            this.UnreadEmailsCount = this.GetUnreadEmailsCount();
        }

        private int GetUnreadEmailsCount()
        {
            if (this.Emails != null)
            {
                var source = this.Emails.SourceCollection as IEnumerable<Email>;
                if (source != null)
                {
                    return source.Count(i => i.Status == Enums.EmailStatus.Unread);
                }
            }
            return 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}