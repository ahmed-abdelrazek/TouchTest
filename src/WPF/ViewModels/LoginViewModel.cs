using Caliburn.Micro;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TouchTest.WPF.Helpers;

namespace TouchTest.WPF.ViewModels
{
    public class LoginViewModel : Screen
    {
        string _userName;
        string _password;
        string _errorMsg;
        Visibility _isErrorMsgVisible;
        bool _isLogging;
        IAPIHelper _apiHelper;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool IsLogging
        {
            get => _isLogging;
            set
            {
                _isLogging = value;
                NotifyOfPropertyChange(() => IsLogging);
            }
        }

        public string ErrorMsg
        {
            get => _errorMsg;
            set
            {
                _errorMsg = value;
                NotifyOfPropertyChange(() => ErrorMsg);
            }
        }

        public Visibility IsErrorMsgVisible
        {
            get => _isErrorMsgVisible;
            set
            {
                _isErrorMsgVisible = value;
                NotifyOfPropertyChange(() => IsErrorMsgVisible);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        Views.ShellView Message = Application.Current.Windows.OfType<Views.ShellView>().FirstOrDefault();

        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
            IsErrorMsgVisible = Visibility.Collapsed;
        }

        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && !IsLogging;
            }
        }

        public async Task Login()
        {
            IsLogging = true;
            IsErrorMsgVisible = Visibility.Collapsed;
            try
            {
                var result = await _apiHelper.AuthAsync(UserName, Password);
                var conductor = this.Parent as IConductor;
                conductor.ActivateItem(new MainViewModel(_apiHelper));
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                IsErrorMsgVisible = Visibility.Visible;
            }
            IsLogging = false;
        }

        public void Register()
        {
            var conductor = this.Parent as IConductor;
            conductor.ActivateItem(new RegisterViewModel(_apiHelper));
        }

    }
}