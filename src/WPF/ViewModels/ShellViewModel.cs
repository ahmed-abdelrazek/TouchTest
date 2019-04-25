using Caliburn.Micro;

namespace TouchTest.WPF.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        LoginViewModel _loginVM;

        IWindowManager _windowManager;

        public ShellViewModel(LoginViewModel loginVM, IWindowManager windowManager)
        {
            _loginVM = loginVM;
            _windowManager = windowManager;
            ActivateItem(_loginVM);
        }
    }
}
