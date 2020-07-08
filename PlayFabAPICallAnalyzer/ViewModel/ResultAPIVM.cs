using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFabAPICallAnalyzer.ViewModel
{
    public class ResultAPIVM : ViewModelBase
    {
        private string _sourcePath;
        private readonly DelegateCommand _exportCommand;

        public string SourcePath
        {
            get => _sourcePath;
            set => SetProperty(ref _sourcePath, value);
        }

        public ResultAPIVM()
        {
            _exportCommand = new DelegateCommand(OnExport);
        }

        public ResultAPIVM(string sourcePath)
        {
            _exportCommand = new DelegateCommand(OnExport);
            SourcePath = sourcePath;
            //DataLoader(sourcePath);
        }

        private void OnExport(object commandParameter) 
        { 
            //ToDo
        }
    }
}
