using System;
using System.Collections.Generic;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Commands.Contacts;
using Client.Commands.Files;
using Client.Commands.Messages;
using Client.Features.Contacts;
using Client.Features.Files;
using Client.Features.Messages;
using Client.Insrastructure;
using Client.Messages;
using Client.Services;
using Client.Utils;
using Common.Contacts;
using System.Linq;
using Common.Files;
using Protocol.FileTransfer;
using Message = Common.Messages.Message;

namespace Client.Features.Communicator
{
    public class CommunicatorViewModel : Screen, IHandle<ContactAdded>,
        IHandle<ContactsDataReceived>, IHandle<ContactsLoaded>, IHandle<MessagesFounded>,
        IHandle<FileOpened>, IHandle<FilesFounded>, IHandle<FileDownloadAccepted>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<int, NewMessagesWindow> _newMessagesWindowFactory;
        private readonly Func<int, RemoveContact> _removeContactFactory;
        private readonly Func<IEnumerable<Message>, ServiceNewMessages> _serviceNewMessagesFactory;
        private readonly Func<int, FileBasicInfo, UploadFile> _uploadFileFactory;
        private readonly Func<IEnumerable<FileHeader>, ServiceNewFiles> _serviceNewFilesFactory;
        private readonly Func<FileHeader, DownloadFile> _downloadFileFactory;
        private readonly IWindowManager _windowManager;
        private readonly IContainer _container;

        public BindableCollection<ContactViewModel> Contacts { get; set; }
        private ContactViewModel _selectedContact;
        public ContactViewModel SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                NotifyOfPropertyChange(() => CanRemoveContact);
                NotifyOfPropertyChange(() => CanNewMessagesWindow);
                NotifyOfPropertyChange(() => CanUploadFile);
                NotifyOfPropertyChange(() => SelectedContact);
            }
        }
        public int SelectedContactIndex { get; set; }

        public bool CanNewMessagesWindow
        {
            get { return SelectedContact != null; }
        }

        public bool CanRemoveContact
        {
            get { return SelectedContact != null; }
        }

        public bool CanUploadFile
        {
            get { return SelectedContact != null; }
        }

        public CommunicatorViewModel(
            IEventAggregator eventAggregator,
            Func<int, NewMessagesWindow> newMessagesWindowFactory,
            Func<int, RemoveContact> removeContactFactory,
            Func<IEnumerable<Message>, ServiceNewMessages> serviceNewMessagesFactory,
            Func<int, FileBasicInfo, UploadFile> uploadFileFactory,
            Func<IEnumerable<FileHeader>, ServiceNewFiles> serviceNewFilesFactory,
            Func<FileHeader, DownloadFile> downloadFileFactory,
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
            _windowManager = windowManager;
            _container = container;

            Contacts = new BindableCollection<ContactViewModel>();
            _eventAggregator.Subscribe(this);

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

        public void NewMessagesWindow()
        {
            ICommand command = _newMessagesWindowFactory(SelectedContact.Number);
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

        public IEnumerable<IResult> RemoveContact()
        {
            ICommand removeContact = _removeContactFactory(SelectedContactIndex);
            yield return new CommandResult(removeContact);
            Contacts.Remove(SelectedContact);
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

            ICommand command = _uploadFileFactory(SelectedContact.Number, message.FileInfo);
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

        private void ExecutePureCommand<T>() where T : ICommand
        {
            ICommand command = _container.Resolve<T>();
            CommandInvoker.Execute(command);
        }
    }
}
