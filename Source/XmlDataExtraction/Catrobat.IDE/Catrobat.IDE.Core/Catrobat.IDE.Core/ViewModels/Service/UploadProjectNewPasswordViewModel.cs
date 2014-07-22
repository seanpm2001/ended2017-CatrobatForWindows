﻿using Catrobat.IDE.Core.Resources.Localization;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.Services.Common;
using Catrobat.IDE.Core.Utilities.JSON;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace Catrobat.IDE.Core.ViewModels.Service
{
    public class UploadProjectNewPasswordViewModel : ViewModelBase
    {
        #region private Members

        private CatrobatContextBase _context;
        private MessageboxResult _missingPasswordDataCallbackResult;
        private MessageboxResult _passwordInvalidCallbackResult;
        private MessageboxResult _recoveryHashNotFoundCallbackResult;
        private string _newPassword;
        private string _repeatedPassword;

        #endregion

        #region Properties

        public CatrobatContextBase Context
        {
            get { return _context; }
            set { _context = value; RaisePropertyChanged(() => Context); }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    RaisePropertyChanged(() => NewPassword);
                }
            }
        }

        public string RepeatedPassword
        {
            get { return _repeatedPassword; }
            set
            {
                if (_repeatedPassword != value)
                {
                    _repeatedPassword = value;
                    RaisePropertyChanged(() => RepeatedPassword);
                }
            }
        }

        #endregion

        #region Commands

        public ICommand NewPasswordCommand { get; private set; }

        #endregion

        #region Actions

        private async void NewPasswordAction()
        {
            if (string.IsNullOrEmpty(_newPassword) || string.IsNullOrEmpty(_repeatedPassword))
            {
                ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectPasswordRecoveryErrorCaption,
                    AppResources.Main_UploadProjectMissingPassword, MissingPasswordDataCallback, MessageBoxOptions.Ok);
            }
            else
            {
                JSONStatusResponse status_response = await CatrobatWebCommunicationService.AsyncChangePassword(_newPassword, _repeatedPassword, Context.LocalSettings.CurrentUserRecoveryHash, ServiceLocator.CultureService.GetCulture().TwoLetterISOLanguageName);

                switch (status_response.statusCode)
                {
                    case StatusCodes.ServerResponseOk:
                        GoBackAction();
                        ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectNewPassword,
                            AppResources.Main_UploadProjectPasswordChangeSucess, PasswordInvalidCallback, MessageBoxOptions.Ok);
                        break;

                    // because of typing-error in server-message
                    case StatusCodes.ServerResponseRecoveryHashNotFound:
                        ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectPasswordRecoveryErrorCaption,
                            AppResources.Main_UploadProjectRecoveryHashError, RecoveryHashNotFoundCallback, MessageBoxOptions.Ok);
                        break;

                    // may be checked locally
                    case StatusCodes.ServerResponsePasswordMatchFailed:
                        ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectPasswordRecoveryErrorCaption,
                            AppResources.Main_UploadProjectRecoveryPasswordMatchError, PasswordInvalidCallback, MessageBoxOptions.Ok);
                        break;

                    case StatusCodes.HTTPRequestFailed:
                        ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectPasswordRecoveryErrorCaption,
                            AppResources.Main_NoInternetConnection, MissingPasswordDataCallback, MessageBoxOptions.Ok);
                        break;

                    default:
                        string messageString = string.IsNullOrEmpty(status_response.answer) ? string.Format(AppResources.Main_UploadProjectUndefinedError, status_response.statusCode.ToString()) : status_response.answer;
                        ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectPasswordRecoveryErrorCaption,
                            messageString, MissingPasswordDataCallback, MessageBoxOptions.Ok);
                        break;
                }
            }
        }

        protected override void GoBackAction()
        {
            ResetViewModel();
            base.GoBackAction();
        }

        #endregion

        #region MessageActions
        private void ContextChangedAction(GenericMessage<CatrobatContextBase> message)
        {
            Context = message.Content;
        }
        #endregion

        public UploadProjectNewPasswordViewModel()
        {
            NewPasswordCommand = new RelayCommand(NewPasswordAction);

            Messenger.Default.Register<GenericMessage<CatrobatContextBase>>(this,
                 ViewModelMessagingToken.ContextListener, ContextChangedAction);
        }

        #region Callbacks

        private void MissingPasswordDataCallback(MessageboxResult result)
        {
            _missingPasswordDataCallbackResult = result;
        }

        private void PasswordInvalidCallback(MessageboxResult result)
        {
            _passwordInvalidCallbackResult = result;
        }

        private void RecoveryHashNotFoundCallback(MessageboxResult result)
        {
            _recoveryHashNotFoundCallbackResult = result;
            GoBackAction();
        }

        #endregion

        private void ResetViewModel()
        {
            NewPassword = "";
            RepeatedPassword = "";
        }
    }
}