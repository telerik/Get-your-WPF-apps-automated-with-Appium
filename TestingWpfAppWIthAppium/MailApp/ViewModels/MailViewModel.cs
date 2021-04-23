using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.RichTextBoxUI;
using Telerik.Windows.Data;

namespace MailApp
{
    public partial class MailViewModel : ViewModelBase
    {
        private bool _isInEditMode;
        private bool _hasSelectedEmail;
        private Email _selectedEmail;
        private Email _newEmail;
        private string _originalContent;
        private string _editableSubject;
        private string _editableRecipient;
        private string _editableCarbonCopy;
        private FilterDescriptor _unreadFilterDescriptor;
        private List<Folder> _folders;
        private QueryableCollectionView _emails;
        private ObservableCollection<OutlookSection> _outlookSections;
        private OutlookSection _selectedOutlookSection;

        private void InitializeFilterDescriptor()
        {
            this._unreadFilterDescriptor = new FilterDescriptor
            {
                Member = "Status",
                Operator = FilterOperator.IsEqualTo,
                Value = Enums.EmailStatus.Unread,
            };
        }

        /// <summary>
        /// Gets or sets Emails and notifies for changes
        /// </summary>
        public QueryableCollectionView Emails
        {
            get
            {
                if (this.SelectedFolder != null)
                {
                    return this.SelectedFolder.Emails;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the Folders and notifies for changes
        /// </summary>
        public List<Folder> Folders
        {
            get
            {
                if (this._folders == null)
                {
                    this._folders = SampleContentService.GetRootEmailFolders();
                }

                return this._folders;
            }
        }

        /// <summary>
        /// Gets or sets SelectedOutlookSection and notifies for changes
        /// </summary>
        public OutlookSection SelectedOutlookSection
        {
            get
            {
                if (this._selectedOutlookSection == null)
                {
                    this._selectedOutlookSection = this.OutlookSections.FirstOrDefault();
                }
                return this._selectedOutlookSection;
            }

            set
            {
                if (this._selectedOutlookSection != value)
                {
                    this._selectedOutlookSection = value;

                    this.OnPropertyChanged(() => this.SelectedOutlookSection);
                }
            }
        }

        /// <summary>
        /// Gets or sets OutlookSections and notifies for changes
        /// </summary>
        public ObservableCollection<OutlookSection> OutlookSections
        {
            get
            {
                if (this._outlookSections == null)
                {
                    this._outlookSections = SampleContentService.GetOutlookSections();
                }
                return this._outlookSections;
            }
        }

        /// <summary>
        /// Gets or sets OriginalContent and notifies for changes
        /// </summary>
        public string OriginalContent
        {
            get
            {
                return this._originalContent;
            }
            set
            {
                if (this._originalContent != value)
                {
                    this._originalContent = value;
                    this.OnPropertyChanged(() => this.OriginalContent);
                }
            }
        }

        /// <summary>
        /// Gets or sets EditableCarbonCopy and notifies for changes
        /// </summary>
        public string EditableCarbonCopy
        {
            get
            {
                return this._editableCarbonCopy;
            }

            set
            {
                if (this._editableCarbonCopy != value)
                {
                    this._editableCarbonCopy = value;
                    this.OnPropertyChanged(() => this.EditableCarbonCopy);
                }
            }
        }

        /// <summary>
        /// Gets or sets EditableRecipient and notifies for changes
        /// </summary>
        public string EditableRecipient
        {
            get
            {
                return this._editableRecipient;
            }

            set
            {
                if (this._editableRecipient != value)
                {
                    this._editableRecipient = value;
                    this.OnPropertyChanged(() => this.EditableRecipient);
                }
            }
        }

        /// <summary>
        /// Gets or sets EditableSubject and notifies for changes
        /// </summary>
        public string EditableSubject
        {
            get
            {
                return this._editableSubject;
            }

            set
            {
                if (this._editableSubject != value)
                {
                    this._editableSubject = value;
                    this.OnPropertyChanged(() => this.EditableSubject);
                }
            }
        }

        /// <summary>
        /// Gets or sets HasSelectedEmail and notifies for changes
        /// </summary>
        public bool HasSelectedEmail
        {
            get
            {
                return this._hasSelectedEmail;
            }
            set
            {
                if (this._hasSelectedEmail != value)
                {
                    this._hasSelectedEmail = value;
                    this.ReplyCommand.InvalidateCanExecute();
                    this.ReplyAllCommand.InvalidateCanExecute();
                    this.ForwardCommand.InvalidateCanExecute();
                    this.DiscardCommand.InvalidateCanExecute();
                    this.OnPropertyChanged(() => this.HasSelectedEmail);
                }
            }
        }

        /// <summary>
        /// Gets or sets SelectedFolder and notifies for changes
        /// </summary>
        public Folder SelectedFolder
        {
            get
            {
                return this.SelectedOutlookSection != null ? this.SelectedOutlookSection.SelectedItem as Folder : null;
            }
        }

        /// <summary>
        /// Gets or sets SelectedEmail and notifies for changes
        /// </summary>
        public Email SelectedEmail
        {
            get
            {
                return this._selectedEmail;
            }
            set
            {
                if (this._selectedEmail != value)
                {
                    this._selectedEmail = value;
                    if (value != null && this._selectedEmail.Sender != null)
                    {
                        this.OriginalContent = this._selectedEmail.Content;
                        this.HasSelectedEmail = true;
                    }
                    else
                    {
                        this.HasSelectedEmail = false;
                        this.IsInEditMode = false;
                    }

                    if (this._selectedEmail != null)
                    {
                        this.UpdateSelectedEmailStatus();
                        this.UpdateFolderUnreadEmailsCount(this.SelectedFolder);
                    }

                    this.MarkUnreadReadCommand.InvalidateCanExecute();
                    this.OnPropertyChanged(() => this.SelectedEmail);
                }
            }
        }

        /// <summary>
        /// Gets or sets NewEmail and notifies for changes
        /// </summary>
        public Email NewEmail
        {
            get
            {
                return this._newEmail;
            }
            set
            {
                if (this._newEmail != value)
                {
                    this._newEmail = value;
                    this.OnPropertyChanged("NewEmail");
                }
            }
        }

        /// <summary>
        /// Gets or sets IsInEditMode and notifies for changes
        /// </summary>
        public bool IsInEditMode
        {
            get
            {
                return this._isInEditMode;
            }

            set
            {
                if (this._isInEditMode != value)
                {
                    this._isInEditMode = value;
                    this.OnPropertyChanged(() => this.IsInEditMode);
                }
            }
        }

        private Enums.PaneType GetPaneType(RadPane pane)
        {
            return ConditionalDockingHelper.GetPaneType(pane);
        }

        /// <summary>
        /// Determines if the Docking's Top, Bottom, Left and Right compasses should be shown for the Dragged Pane
        /// </summary>
        private bool CanDock(RadPane paneToDock, DockPosition position)
        {
            var paneToDockType = GetPaneType(paneToDock);
            switch (paneToDockType)
            {
                case Enums.PaneType.Normal:
                    return true;
                case Enums.PaneType.Restricted:
                    return position != DockPosition.Center && position != DockPosition.Top && position != DockPosition.Bottom;
                default:
                    return false;
            }
        }

        private bool CanDock(object dragged, DockPosition position)
        {
            var splitContainer = dragged as RadSplitContainer;

            return !splitContainer.EnumeratePanes().Any(p => !CanDock(p, position));
        }

        private static bool CompassNeedsToShow(Compass compass)
        {
            return compass.IsLeftIndicatorVisible
                || compass.IsTopIndicatorVisible
                || compass.IsRightIndicatorVisible
                || compass.IsBottomIndicatorVisible
                || compass.IsCenterIndicatorVisible;
        }

        private void UpdateFolderUnreadEmailsCount(Folder folder)
        {
            folder.UpdateUnreadEmailsCount();
        }

        private void UpdateSelectedEmailStatus()
        {
            if (this.SelectedEmail != null && this.SelectedEmail.Status == Enums.EmailStatus.Unread)
            {
                this.SelectedEmail.Status = Enums.EmailStatus.Read;

            }
        }
    }
}
