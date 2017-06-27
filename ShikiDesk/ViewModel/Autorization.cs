using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShikiDesk.ViewModel
{
    public class AutorizationVM : ViewModel
    {
        public AutorizationVM()
        {
            Nickname = Properties.Settings.Default.nickname;
            TryLogin = new RelayCommand(onTryLogin);
        }

        string _nickname;
        public string Nickname
        {
            get { return _nickname; }
            set { Set(ref _nickname, value); }
        }

        string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        string _errorString;
        public string ErrorString //Разве не надо сделать так, чтобы изменять поле можно было только внутри класса?
        {
            get { return _errorString; }
            set { Set(ref _errorString, value); }
        }

        bool _savePassword;
        public bool SavePassword
        {
            get { return _savePassword; }
            set { Set(ref _savePassword, value); }
        }

        TaskCompletionSource<int> taskCompletionSource;

        public ICommand TryLogin { get; }

        public async Task RunAuthorization()
        {
            if (taskCompletionSource != null)
            {
                throw new InvalidOperationException("Авторизация уже идёт.");
            }
            taskCompletionSource = new TaskCompletionSource<int>();
            await taskCompletionSource.Task;
            taskCompletionSource = null;
        }

        void onTryLogin()
        {
            if (String.IsNullOrEmpty(Nickname) || String.IsNullOrEmpty(Password))
            {
                ErrorString = "Пожалуйста введите логин/пароль!";
                return;
            }

            try
            {
                User = new ShikiApiLib.ShikiApi(Nickname, Password);

                if (SavePassword)
                {
                    Properties.Settings.Default.nickname = User.Nickname;
                    Properties.Settings.Default.access_token = User.AccessToken;
                    Properties.Settings.Default.curren_user_id = User.CurrentUserId;
                    Properties.Settings.Default.firstStart = false;
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                ErrorString = "Ошибка авторизации: " + ex.Message;
                return;
            }

            taskCompletionSource.SetResult(0); // только если всё прошло успешно, завершить задачу, что приведёт к возврату управления главному окну.
        }
    }
}
