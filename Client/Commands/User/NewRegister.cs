using Caliburn.Micro;
using Client.Features.Register;

namespace Client.Commands.User
{
    public class NewRegister : ICommand
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly IWindowManager _windowManager;
        private readonly Screen _returnWindow;

        public NewRegister(
            RegisterViewModel registerViewModel, 
            IWindowManager windowManager,
            Screen returnWindow)
        {
            _registerViewModel = registerViewModel;
            _windowManager = windowManager;
            _returnWindow = returnWindow;
        }

        public void Execute()
        {
            _registerViewModel.SetReturnViewModel(_returnWindow);
            _windowManager.ShowWindow(_registerViewModel);
        }
    }
}
