using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TouchTest.WPF.Helpers;

namespace TouchTest.WPF.ViewModels
{
    public class RegisterViewModel : Screen
    {
        string _userName;
        string _email;
        string _password;
        string _cPassword;
        string _errorMsg;
        Visibility _isErrorMsgVisible;
        bool _isRegistering;
        IAPIHelper _apiHelper;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public string CPassword
        {
            get => _cPassword;
            set
            {
                _cPassword = value;
                NotifyOfPropertyChange(() => CPassword);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public bool IsRegistering
        {
            get => _isRegistering;
            set
            {
                _isRegistering = value;
                NotifyOfPropertyChange(() => IsRegistering);
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
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public RegisterViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
            IsErrorMsgVisible = Visibility.Collapsed;
        }

        public bool CanRegister
        {
            get
            {
                return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && Password?.Length >= 5 && Password == CPassword && !IsRegistering;
            }
        }

        public async Task Register()
        {
            IsRegistering = true;
            IsErrorMsgVisible = Visibility.Collapsed;
            try
            {
                var date = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", Email),
                    new KeyValuePair<string, string>("UserName", UserName),
                    new KeyValuePair<string, string>("Password", Password),
                    new KeyValuePair<string, string>("ConfirmPassword", CPassword),
                });

                using (HttpResponseMessage response = await _apiHelper.APIClient.PostAsync("api/Account/Register", date))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("تم اضافة المستخدم بنجاح ", UserName);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                IsErrorMsgVisible = Visibility.Visible;
            }
            IsRegistering = false;
        }

        public void Back()
        {
            var conductor = this.Parent as IConductor;
            conductor.ActivateItem(new LoginViewModel(_apiHelper));
        }
    }
}
