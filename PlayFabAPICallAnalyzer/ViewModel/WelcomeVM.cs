using PlayFabAPICallAnalyzer.Model;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace PlayFabAPICallAnalyzer.ViewModel
{
    public class WelcomeVM : ViewModelBase
    {
        private string _selectedFilePath;
        private string _selectedFileName;
        private MessageModel _mm;
        private bool _isValidData;
        private readonly DelegateCommand _fileBrowseCommand;

        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set => SetProperty(ref _selectedFilePath, value);
        }

        public string SelectedFileName
        {
            get => _selectedFileName;
            set => SetProperty(ref _selectedFileName, value);
        }

        public MessageModel MessageModel
        {
            get => _mm;
            set => SetProperty(ref _mm, value);
        }

        public bool isValidData
        {
            get => _isValidData;
            set => SetProperty(ref _isValidData, value);
        }

        public ICommand FileBrowseCommand => _fileBrowseCommand;
       
        public WelcomeVM()
        {
            _fileBrowseCommand = new DelegateCommand(OnFileBrowse);
        }

        public WelcomeVM(string sourcePath)
        {
            _fileBrowseCommand = new DelegateCommand(OnFileBrowse);
            SelectedFilePath = sourcePath;
            SelectedFileName = Path.GetFileName(sourcePath);
            isValidData = Helper.IsValidJsonData(sourcePath);
        }

        private void OnFileBrowse(object commandParameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog() { AutoUpgradeEnabled = false };
            fileDialog.DefaultExt = ".json";
            fileDialog.Filter = "JSON (.json)|*.json|Text documents (.txt)|*.txt|All files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                isValidData = Helper.IsValidJsonData(fileDialog.FileName);
                if (!isValidData)
                {
                    //MessageBox.Show("This is not a valid PlayFab API Call JSON data file!");
                    MessageModel = new MessageModel("This is not a valid PlayFab API Call JSON data file!", MessageType.Error);
                }
                else
                {
                    SelectedFilePath = fileDialog.FileName;
                    SelectedFileName = Path.GetFileName(fileDialog.FileName);
                    MessageModel = new MessageModel("Valid, please move on:)");
                }
            }
            else
            {
                MessageModel = new MessageModel("Please select a valid PlayFab API Call JSON data file", MessageType.Warning);
            }
        }

        private RelayCommand mSendFeedbackCommand;
        public ICommand SendFeedbackCommand
        {
            get
            {
                return mSendFeedbackCommand ?? (mSendFeedbackCommand = new RelayCommand(SendFeedback));
            }
        }
        private void SendFeedback(object url)
        {
            Process proc = new Process();
            var assName = Assembly.GetExecutingAssembly().GetName();
            proc.StartInfo.FileName = $"mailto:yapchen@microsoft.com?subject=[Feedback] {assName.Name} V{assName.Version}&body=Hi Franklin,";
            proc.Start();
        }

        private RelayCommand mHackathonCommand;
        public ICommand HackathonCommand
        {
            get
            {
                return mHackathonCommand ?? (mHackathonCommand = new RelayCommand(GoToHackathon));
            }
        }
        private void GoToHackathon(object url)
        {
            Process.Start(url.ToString());
        }
    }
}
