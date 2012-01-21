using System;
using System.Collections.Generic;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Commands.Contacts;
using Client.Commands.Files;
using Client.Commands.Messages;
using Client.Commands.User;
using Client.Features.Files;
using Client.Insrastructure;
using Client.Messages;
using System.Linq;
using Common.Files;
using Protocol.Messages;
using Message = Common.Messages.Message;

namespace Client.Features.Communicator
{
    public class CommunicatorViewModel : Screen, IHandle<ContactAdded>,
        IHandle<ContactsDataReceived>, IHandle<ContactsLoaded>, IHandle<MessagesFounded>,
        IHandle<FileOpened>, IHandle<FilesFounded>, IHandle<FileDownloadAccepted>,
        IHandle<FileDownloaded>, IHandle<LoggedOut>, IHandle<ConferencialMessagesFounded>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<int, NewMessagesWindow> _newMessagesWindowFactory;
        private readonly Func<int, RemoveContact> _removeContactFactory;
        private readonly Func<IEnumerable<Message>, ServiceNewMessages> _serviceNewMessagesFactory;
        private readonly Func<int, FileBasicInfo, UploadFile> _uploadFileFactory;
        private readonly Func<IEnumerable<FileHeader>, ServiceNewFiles> _serviceNewFilesFactory;
        private readonly Func<FileHeader, DownloadFile> _downloadFileFactory;
        private readonly Func<File, SaveFile> _saveFileFactory;
        private readonly Func<IEnumerable<int>, NewConferencialMessagesWindow> _newConferencialMessagewWindowFactory;
        private readonly IWindowManager _windowManager;
        private readonly IContainer _container;

        public BindableCollection<ContactViewModel> Contacts { get; set; }

        public int SelectedContactIndex { get; set; }

        public bool CanOpenMessagesWindow
        {
            get { return SelectedContactsNumbers.Count() == 1; }
        }

        public bool CanOpenConferencialMessagesWindow
        {
            get { return SelectedContactsNumbers.Distinct().Count() > 1; }
        }

        public bool CanRemoveContact
        {
            get { return SelectedContactsNumbers.Count() == 1; }
        }

        public bool CanUploadFile
        {
            get { return SelectedContactsNumbers.Count() == 1; }
        }

        public IEnumerable<int> SelectedContactsNumbers
        {
            get
            {
                return Contacts.Where(contact => contact.IsSelected).Select(contact => contact.Number);
            }
        }

        public ContactViewModel SingleSelectedContact
        {
            get { return SelectedContactsNumbers.Count() == 1 ? Contacts.First(c => c.IsSelected) : null; }
        }

        public CommunicatorViewModel(
            IEventAggregator eventAggregator,
            Func<int, NewMessagesWindow> newMessagesWindowFactory,
            Func<int, RemoveContact> removeContactFactory,
            Func<IEnumerable<Message>, ServiceNewMessages> serviceNewMessagesFactory,
            Func<int, FileBasicInfo, UploadFile> uploadFileFactory,
            Func<IEnumerable<FileHeader>, ServiceNewFiles> serviceNewFilesFactory,
            Func<FileHeader, DownloadFile> downloadFileFactory,
            Func<File, SaveFile> saveFileFactory,
            Func<IEnumerable<int>, NewConferencialMessagesWindow> newConferencialMessagewWindowFactory,
            IWindowManager windowManager,
            IContainer container)
        {
            base.DisplayName = "Internet communicator";

            _eventAggregator = eventAggregator;
            _newMessagesWindowFactory = newMessagesWindowFactory;
            _removeContactFactory = removeContactFactory;
            _serviceNewMessagesFactory = serviceNewMessagesFactory;
            _uploadFileFactory = uploadFileFactory;
            _serviceNewFilesFactory = serviceNewFilesFactory;
            _downloadFileFactory = downloadFileFactory;
            _saveFileFactory = saveFileFactory;
            _newConferencialMessagewWindowFactory = newConferencialMessagewWindowFactory;
            _windowManager = windowManager;
            _container = container;

            Contacts = new BindableCollection<ContactViewModel>();
            _eventAggregator.Subscribe(this);

        }

        public void ContactsListChanged()
        {
            NotifyOfPropertyChange(() => CanRemoveContact);
            NotifyOfPropertyChange(() => CanOpenMessagesWindow);
            NotifyOfPropertyChange(() => CanUploadFile);
            NotifyOfPropertyChange(() => CanOpenConferencialMessagesWindow);
            NotifyOfPropertyChange(() => SingleSelectedContact);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ExecutePureCommand<LoadContacts>();
            ExecutePureCommand<StartRequestingForContacts>();
            ExecutePureCommand<StartRequestingForMessages>();
            ExecutePureCommand<StartRequestingForFiles>();
        }

        protected override void OnDeactivate(bool close)
        {
            ExecutePureCommand<StopRequestingForContacts>();
            ExecutePureCommand<StopRequestingForMessages>();
            ExecutePureCommand<StopRequestingForFiles>();
            base.OnDeactivate(close);
        }

        public void OpenMessagesWindow()
        {
            ICommand command = _newMessagesWindowFactory(SelectedContactsNumbers.First());
            CommandInvoker.Execute(command);
        }

        public void OpenConferencialMessagesWindow()
        {
            ICommand command = _newConferencialMessagewWindowFactory(SelectedContactsNumbers.Distinct());
            CommandInvoker.Execute(command);
        }

        public void UploadFile()
        {
            ExecutePureCommand<OpenFile>();
        }

        public void NewContact()
        {
            ExecutePureCommand<NewContact>();
        }

        public void Logout()
        {
            ExecutePureCommand<Logout>();
        }

        public void Exit()
        {
            TryClose();
        }

        public IEnumerable<IResult> RemoveContact()
        {
            ICommand removeContact = _removeContactFactory(SelectedContactIndex);
            yield return new CommandResult(removeContact);
            Contacts.RemoveAt(SelectedContactIndex);
        }

        public void Handle(ContactAdded message)
        {
            Contacts.Add(new ContactViewModel(message.Contact));
        }

        public void Handle(ContactsLoaded message)
        {
            Contacts.AddRange(message.Contacts.Select(contact => new ContactViewModel(contact)));
        }

        public void Handle(ContactsDataReceived message)
        {
            foreach (var contactViewModel in Contacts)
            {
                contactViewModel.IsAvailable =
                    message.Contacts.First(c => c.ContactStoredData.Number == contactViewModel.Number).IsAvailable;
            }
        }

        public void Handle(FileOpened message)
        {
            var uploadFileViewModel = _container.Resolve<UploadFileViewModel>();
            _windowManager.ShowWindow(uploadFileViewModel);

            ICommand command = _uploadFileFactory(SelectedContactsNumbers.First(), message.FileInfo);
            CommandInvoker.InvokeBusy(command, uploadFileViewModel, e => uploadFileViewModel.TryClose());
        }

        public void Handle(MessagesFounded message)
        {
            ICommand command = _serviceNewMessagesFactory(message.Messages);
            CommandInvoker.Invoke(command);
        }

        public void Handle(FilesFounded message)
        {
            ICommand command = _serviceNewFilesFactory(message.FilesHeaders);
            CommandInvoker.Invoke(command);
        }

        public void Handle(FileDownloadAccepted message)
        {
            var downloadFileViewModel = _container.Resolve<DownloadFileViewModel>();
            _windowManager.ShowWindow(downloadFileViewModel);

            ICommand command = _downloadFileFactory(message.FileHeader);
            CommandInvoker.InvokeBusy(command, downloadFileViewModel, e => downloadFileViewModel.TryClose());
        }

        public void Handle(FileDownloaded message)
        {
            ICommand command = _saveFileFactory(message.File);
            CommandInvoker.Invoke(command);
        }

        public void Handle(LoggedOut message)
        {
            _eventAggregator.Unsubscribe(this);
            _container.BeginLifetimeScope();

            var shellViewModel = _container.Resolve<ShellViewModel>();
            _windowManager.ShowWindow(shellViewModel);
            TryClose();
        }

        public void Handle(ConferencialMessagesFounded message)
        {
            throw new NotImplementedException();
        }

        private void ExecutePureCommand<T>() where T : ICommand
        {
            ICommand command = _container.Resolve<T>();
            CommandInvoker.Execute(command);
        }
    }
}
