using Caliburn.Micro;
using Client.Features.Register;

namespace Client.Commands
{
    public class NewRegister : ICommand<Screen>
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly IWindowManager _windowManager;

        public NewRegister(
            RegisterViewModel registerViewModel, 
            IWindowManager windowManager)
        {
            _registerViewModel = registerViewModel;
            _windowManager = windowManager;
        }

        public void Execute(Screen returnWindow)
        {
            _registerViewModel.SetReturnViewModel(returnWindow);
            _windowManager.ShowWindow(_registerViewModel);
        }
    }
}
