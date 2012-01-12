using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Commands;
using Client.Messages;
using Client.Utils;
using Common.Contacts;

namespace Client.Features.Contacts
{
    public class NewContactViewModel : Screen
    {
        private readonly AddContact _addContact;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public bool CanAdd
        {
            get { return !string.IsNullOrEmpty(Name) && 
                !string.IsNullOrEmpty(Number); }
        }

        public NewContactViewModel(AddContact addContact)
        {
            base.DisplayName = "Add new contact";
            _addContact = addContact;
        }

        public void Add()
        {
            var contactData = new ContactStoredData
                                  {
                                      Name = Name,
                                      Number = Int32.Parse(Number)
                                  };

            try
            {
                _addContact.Execute(contactData);
            }
            catch (Exception exception)
            {
                ErrorMessageBox.Show(exception);
                return;
            }

            TryClose();
        }
    }
}
