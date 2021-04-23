using System;
using System.Windows;
using System.Windows.Threading;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.RichTextBoxUI;

namespace MailApp
{
    public partial class MailViewModel
    {
        public DelegateCommand NewMailCommand { get; private set; }
        public DelegateCommand MarkUnreadCommand { get; private set; }
        public DelegateCommand OpenedCommand { get; private set; }
        public DelegateCommand PreviewShowCompassCommand { get; private set; }
        public DelegateCommand ReplyCommand { get; private set; }
        public DelegateCommand ReplyAllCommand { get; private set; }
        public DelegateCommand ForwardCommand { get; private set; }
        public DelegateCommand DiscardCommand { get; set; }
        public DelegateCommand MarkUnreadReadCommand { get; private set; }
        public DelegateCommand MenuOpenStateChangedCommand { get; private set; }
        public DelegateCommand OpenDialogCommand { get; private set; }
        public DelegateCommand PopOutCommand { get; private set; }
        public DelegateCommand ViewUnreadEmailsCommand { get; private set; }
        public DelegateCommand ViewAllEmailsCommand { get; private set; }

        public MailViewModel()
        {
            this.InitializeCommands();
            this.InitializeFilterDescriptor();
        }

        private void InitializeCommands()
        {
            this.MarkUnreadCommand = new DelegateCommand(this.OnMarkUnreadCommandExecuted, this.CanMarkUnreadCommandExecute);
            this.NewMailCommand = new DelegateCommand(this.OnNewMailCommandExecuted);
            this.OpenedCommand = new DelegateCommand(this.OnOpenedCommandExecuted);
            this.PreviewShowCompassCommand = new DelegateCommand(this.OnPreviewShowCompassCommandExecuted);
            this.ReplyCommand = new DelegateCommand(this.OnReplyCommandExecuted, o => this.HasSelectedEmail);
            this.ReplyAllCommand = new DelegateCommand(this.OnReplyAllCommandExecuted, o => this.HasSelectedEmail);
            this.ForwardCommand = new DelegateCommand(this.OnForwardCommandExecuted, o => this.HasSelectedEmail);
            this.DiscardCommand = new DelegateCommand(this.OnDiscardCommandExecute, o => this.HasSelectedEmail);
            this.MarkUnreadReadCommand = new DelegateCommand(this.OnMarkUnreadReadCommandExecuted, o => this.SelectedEmail != null);
            this.MenuOpenStateChangedCommand = new DelegateCommand(this.OnMenuOpenStateChangedCommandExecuted);
            this.OpenDialogCommand = new DelegateCommand(this.OnOpenDialogCommandExecuted);
            this.PopOutCommand = new DelegateCommand(this.OnPopOutCommandExecuted, o => this.SelectedEmail != null);
            this.ViewUnreadEmailsCommand = new DelegateCommand(this.OnViewUnreadEmailsCommandExecuted);
            this.ViewAllEmailsCommand = new DelegateCommand(this.OnViewAllEmailsCommandExecuted);
        }


        private void OnReplyCommandExecuted(object obj)
        {
            this.EditableRecipient = this.SelectedEmail.Sender;
            this.EditableCarbonCopy = this.SelectedEmail.CarbonCopy;
            this.EditableSubject = String.Format("RE: {0}", this.SelectedEmail.Subject);
            this.IsInEditMode = true;

            this.PopOutCommand.InvalidateCanExecute();
        }

        private void OnDiscardCommandExecute(object obj)
        {
            // Exits editing mode and return the original email Content
            this.OriginalContent = this.SelectedEmail.Content;
            this.IsInEditMode = false;
        }

        private void OnReplyAllCommandExecuted(object obj)
        {
            this.EditableCarbonCopy = this.SelectedEmail.CarbonCopy;
            this.EditableRecipient = String.Format("{0};{1};{2}", this.SelectedEmail.Sender, this.SelectedEmail.Recipient, this.SelectedEmail.CarbonCopy);
            this.EditableSubject = String.Format("RE: {0}", this.SelectedEmail.Subject);
            this.IsInEditMode = true;

            this.PopOutCommand.InvalidateCanExecute();
        }

        private void OnForwardCommandExecuted(object obj)
        {
            this.EditableRecipient = this.SelectedEmail.Sender;
            this.EditableCarbonCopy = this.SelectedEmail.CarbonCopy;
            this.EditableSubject = String.Format("FW: {0}", this.SelectedEmail.Subject);
            this.IsInEditMode = true;

            this.PopOutCommand.InvalidateCanExecute();
        }

        private void OnMarkUnreadReadCommandExecuted(object param)
        {
            this.SelectedEmail.Status = this.SelectedEmail.Status == Enums.EmailStatus.Unread ? Enums.EmailStatus.Read : Enums.EmailStatus.Unread;
        }

        private void OnMenuOpenStateChangedCommandExecuted(object param)
        {
            var ribbon = param as RadRichTextBoxRibbonUI;
            if (ribbon.IsApplicationMenuOpen)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    this.DiscardCommand.Execute(null);
                }));
            }
        }

        private void OnOpenDialogCommandExecuted(object obj)
        {
            RadWindow.Alert(
                new DialogParameters
                {
                    Content = string.Format("{0}'s command executed.", obj.ToString()),
                    Header = "Telerik"
                });
        }

        private void OnPopOutCommandExecuted(object obj)
        {
            this.NewEmail = this.SelectedEmail.Clone() as Email;
            this.ShowNewEmailWindow();
            this.IsInEditMode = false;
        }
        private bool CanMarkUnreadCommandExecute(object obj)
        {
            return this.SelectedEmail != null && this.SelectedEmail.Status == Enums.EmailStatus.Read;
        }

        private void OnMarkUnreadCommandExecuted(object param)
        {
            this.SelectedEmail.Status = Enums.EmailStatus.Unread;
        }

        private void OnViewAllEmailsCommandExecuted(object parameter)
        {
            if (this.Emails != null)
            {
                this.Emails.FilterDescriptors.Remove(this._unreadFilterDescriptor);
            }
        }

        private void OnViewUnreadEmailsCommandExecuted(object parameter)
        {
            if (this.Emails != null)
            {
                this.Emails.FilterDescriptors.Add(this._unreadFilterDescriptor);
            }
        }

        private void ShowNewEmailWindow()
        {
            var window = new NewEmailWindow(this);
            window.Owner = Application.Current.MainWindow;
            window.Show();
        }

        private void OnNewMailCommandExecuted(object param)
        {
            this.NewEmail = null;
            var window = new NewEmailWindow { DataContext = this };
            window.Owner = Application.Current.MainWindow;
            window.Show();
        }

        private void OnOpenedCommandExecuted(object param)
        {
            var args = (RadRoutedEventArgs)param;
            var menu = (RadContextMenu)args.OriginalSource;
            var row = menu.GetClickedElement<GridViewRow>();
            if (row != null)
            {
                row.IsSelected = row.IsCurrent = true;
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {
                    cell.IsCurrent = true;
                }

                this.MarkUnreadCommand.InvalidateCanExecute();
            }
            else
            {
                menu.IsOpen = false;
            }
        }

        private void OnPreviewShowCompassCommandExecuted(object param)
        {
            var args = (PreviewShowCompassEventArgs)param;
            if (args.TargetGroup != null)
            {
                args.Compass.IsCenterIndicatorVisible = false;
                args.Compass.IsLeftIndicatorVisible = false;
                args.Compass.IsTopIndicatorVisible = false;
                args.Compass.IsRightIndicatorVisible = false;
                args.Compass.IsBottomIndicatorVisible = false;
            }
            else
            {
                args.Compass.IsCenterIndicatorVisible = CanDock(args.DraggedElement, DockPosition.Center);
                args.Compass.IsLeftIndicatorVisible = CanDock(args.DraggedElement, DockPosition.Left);
                args.Compass.IsTopIndicatorVisible = CanDock(args.DraggedElement, DockPosition.Top);
                args.Compass.IsRightIndicatorVisible = CanDock(args.DraggedElement, DockPosition.Right);
                args.Compass.IsBottomIndicatorVisible = CanDock(args.DraggedElement, DockPosition.Bottom);
            }

            args.Canceled = !(CompassNeedsToShow(args.Compass));
        }
    }
}
