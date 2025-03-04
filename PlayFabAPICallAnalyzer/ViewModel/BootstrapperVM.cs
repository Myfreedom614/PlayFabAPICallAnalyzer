﻿using System.Reflection;
using System.Windows.Input;

namespace PlayFabAPICallAnalyzer.ViewModel
{
    public class BootstrapperVM : ViewModelBase
    {
        private ViewModelBase _viewModel;
        public ViewModelBase ViewModel {
            get => _viewModel;
            set => SetProperty(ref _viewModel, value);
        }

        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        public BootstrapperVM()
        {
            var assName = Assembly.GetExecutingAssembly().GetName();
            WindowTitle = $"{assName.Name} V{assName.Version}";
            ViewModel = new WelcomeVM();
        }

        private ICommand _displayWelcomeView;
        public ICommand DisplayWelcomeView
        {
            get
            {
                return _displayWelcomeView ?? (_displayWelcomeView = new CommandHandler(param => OnDisplayWelcomeView(param), true));
            }
        }
        private void OnDisplayWelcomeView(object param)
        {
            var sourcePath = param as string;
            ViewModel = new WelcomeVM(sourcePath);
        }

        private ICommand _displayAPIRatioView;
        public ICommand DisplayAPIRatioView
        {
            get
            {
                return _displayAPIRatioView ?? (_displayAPIRatioView = new CommandHandler(param => OnDisplayAPIRatioView(param), true));
            }
        }
        private void OnDisplayAPIRatioView(object param)
        {
            var sourcePath = param as string;
            ViewModel = new APIRatioVM(sourcePath);
        }

        private ICommand _displayAPIResultView;
        public ICommand DisplayAPIResultView
        {
            get
            {
                return _displayAPIResultView ?? (_displayAPIResultView = new CommandHandler(param => OnDisplayAPIResultView(param), true));
            }
        }
        private void OnDisplayAPIResultView(object param)
        {
            var sourcePath = param as string;
            ViewModel = new APIResultVM(sourcePath);
        }

        private ICommand _displayResultAPIView;
        public ICommand DisplayResultAPIView
        {
            get
            {
                return _displayResultAPIView ?? (_displayResultAPIView = new CommandHandler(param => OnDisplayResultAPIView(param), true));
            }
        }
        private void OnDisplayResultAPIView(object param)
        {
            var sourcePath = param as string;
            ViewModel = new ResultAPIVM(sourcePath);
        }
    }
}
