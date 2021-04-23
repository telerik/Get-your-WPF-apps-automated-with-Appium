using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MailApp
{
    public class Email : INotifyPropertyChanged, ICloneable
    {
        private string _carbonCopy;
        private string _content;
        private DateTime _received;
        private string _recipient;
        private string _sender;
        private Enums.EmailStatus _status;
        private string _subject;

        public Email()
        { }

        public Email(string sender, string recipient, string subject, DateTime received)
        {
            this.Sender = sender;
            this.Recipient = recipient;
            this.Subject = subject;
            this.Received = received;
        }

        /// <summary>
        /// Gets or sets the CarbonCopy of the email.
        /// </summary>
        public string CarbonCopy
        {
            get
            {
                return this._carbonCopy;
            }
            set
            {
                if (this._carbonCopy != value)
                {
                    this._carbonCopy = value;
                    this.OnPropertyChanged("CarbonCopy");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Content of the email.
        /// </summary>
        public string Content
        {
            get
            {
                return this._content;
            }
            set
            {
                if (this._content != value)
                {
                    this._content = value;
                    this.OnPropertyChanged("Content");
                }
            }
        }

        /// <summary>
        /// Gets or sets date the email has been received.
        /// </summary>
        public DateTime Received
        {
            get
            {
                return this._received;
            }
            set
            {
                if (this._received != value)
                {
                    this._received = value;
                    this.OnPropertyChanged("Received");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Recipient address of the email.
        /// </summary>
        public string Recipient
        {
            get
            {
                return this._recipient;
            }
            set
            {
                if (this._recipient != value)
                {
                    this._recipient = value;
                    this.OnPropertyChanged("Recipient");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Sender address of the email.
        /// </summary>
        public string Sender
        {
            get
            {
                return this._sender;
            }
            set
            {
                if (this._sender != value)
                {
                    this._sender = value;
                    this.OnPropertyChanged("Sender");
                }
            }
        }

        /// <summary>
        /// Gets or sets Status of the Email object.
        /// </summary>
        public Enums.EmailStatus Status
        {
            get
            {
                return this._status;
            }

            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.OnPropertyChanged("Status");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Subject of the email.
        /// </summary>
        public string Subject
        {
            get
            {
                return this._subject;
            }
            set
            {
                if (this._subject != value)
                {
                    this._subject = value;
                    this.OnPropertyChanged("Subject");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            var otherEmail = new Email();

            this.SetPropertyValues(otherEmail);

            return otherEmail;
        }

        private void SetPropertyValues(Email otherEmail)
        {
            var propertyInfo = this.GetType().GetProperties().Where(p => p.CanWrite && (p.PropertyType.IsValueType || p.PropertyType.IsEnum || p.PropertyType.Equals(typeof(System.String))));

            foreach (PropertyInfo property in propertyInfo)
            {
                if (property.CanWrite)
                {
                    property.SetValue(otherEmail, property.GetValue(this, null), null);
                }
            }
        }
    }
}