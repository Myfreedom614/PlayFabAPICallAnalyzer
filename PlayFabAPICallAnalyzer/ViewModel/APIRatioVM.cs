using OxyPlot;
using OxyPlot.Series;
using PlayFabAPICallAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PlayFabAPICallAnalyzer.ViewModel
{
    public class APIRatioVM : ViewModelBase
    {
        private string _sourcePath;
        private List<MItemModel> _sourceData;
        private List<ResultModel> _resultModel;
        private MItemModel _selectedController;
        private bool _showTimeRange = false;
        private PlotModel _apiRatioData;
        private IList<PieSlice> _slices;
        private string _titleId;
        private readonly DelegateCommand _exportCommand;

        public string SourcePath
        {
            get => _sourcePath;
            set => SetProperty(ref _sourcePath, value);
        }
        public List<MItemModel> SourceData
        {
            get => _sourceData;
            set => SetProperty(ref _sourceData, value);
        }
        public List<ResultModel> ResultModel
        {
            get => _resultModel;
            set => SetProperty(ref _resultModel, value);
        }
        public string TitleId
        {
            get => _titleId;
            set => SetProperty(ref _titleId, value);
        }
        public MItemModel SelectedController
        {
            get => _selectedController;
            set
            {
                SetProperty(ref _selectedController, value);
                ShowTimeRange = value != null ? true : false;

                if (SelectedController.A.Count > 0)
                {
                    var selectedCol = SelectedController.A[0].SeriesCollection;
                    var resultModel = from sc in selectedCol
                                      group sc by sc.apiName into newGroup
                                      orderby newGroup.Key
                                      select new ResultModel() { APIName = newGroup.Key, TitleId = newGroup.ToList()[0].titleid, TotalCount = newGroup.Select(m => m.pointlist.Select(n => n[1].NullableDoubleToDouble2()).Sum()).Sum() } into selection
                                      orderby selection.TotalCount descending
                                      select selection;
                    
                    ResultModel = resultModel.ToList();
                    TitleId = ResultModel[0].TitleId;
                    APIRatioData = CraftData(ResultModel);
                }
                else
                {
                    MessageBox.Show("There is no valid data, check your json file.");
                }
            }
        }
        public bool ShowTimeRange
        {
            get => _sourceData != null ? true : false;
            set => SetProperty(ref _showTimeRange, value);
        }

        public PlotModel APIRatioData
        {
            get => _apiRatioData;
            set => SetProperty(ref _apiRatioData, value);
        }

        public APIRatioVM()
        {
            _exportCommand = new DelegateCommand(OnExport);
        }

        public APIRatioVM(string sourcePath)
        {
            _exportCommand = new DelegateCommand(OnExport);
            SourcePath = sourcePath;
            DataLoader(sourcePath);
        }

        private void OnExport(object commandParameter)
        {
            ExportToExcel<ResultReportModel> s = new ExportToExcel<ResultReportModel>();
            s.dataToPrint = (from x in _resultModel select new ResultReportModel { TitleId=x.TitleId, APIName=x.APIName, TotalCount=x.TotalCount }).ToList();
            s.GenerateReport();
        }

        public void DataLoader(string sourcePath)
        {
            SourceData = Helper.JsonDataLoader(sourcePath);
            SelectedController = _sourceData.Find(x => x.A[0].ControlId.StartsWith("api_requests"));
        }

        private PlotModel CraftData(List<ResultModel> rm)
        {
            var model = new PlotModel { Title = $"PlayFab Title {TitleId} API Request Ratio", TitlePadding=10 };

            var ps = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                InsideLabelColor = OxyColors.White,
                AngleSpan = 360,
                StartAngle = 0
            };
            _slices = new List<PieSlice>();
            foreach(var i in rm)
            {
                _slices.Add(new PieSlice(i.APIName, i.TotalCount) { IsExploded=true });
            }

            ps.Slices = _slices;

            model.Series.Add(ps);
            return model;
        }
        public ICommand ExportCommand => _exportCommand;

    }
}
