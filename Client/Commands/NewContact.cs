﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Features.Contacts;

namespace Client.Commands
{
    public class NewContact : ICommand
    {
        private readonly IWindowManager _windowManager;
        private readonly NewContactViewModel _newContactViewModel;

        public NewContact(
            IWindowManager windowManager, 
            NewContactViewModel newContactViewModel)
        {
            _windowManager = windowManager;
            _newContactViewModel = newContactViewModel;
        }

        public void Execute()
        {
            _windowManager.ShowDialog(_newContactViewModel);
        }
    }
}