using PlayFabAPICallAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PlayFabAPICallAnalyzer.ViewModel
{
    public class ResultAPIVM : ViewModelBase
    {
        private string _sourcePath;
        private string _selectedAPI;
        private List<MItemModel> _sourceData;
        private List<ResultModel> _resultSource;
        private MItemModel _selectedController;
        private bool _showTimeRange = false;
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
        public List<ResultModel> ResultSource
        {
            get => _resultSource;
            set => SetProperty(ref _resultSource, value);
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
                                      group sc by sc.result into newGroup
                                      orderby newGroup.Key
                                      select new ResultModel() { Result = newGroup.Key, Series = newGroup.ToList() };
                    ResultSource = resultModel.ToList();
                }
                else
                {
                    MessageBox.Show("There is no valid data, check your json file.");
                }
            }
        }
        public bool ShowTimeRange
        {
            get => SelectedController != null ? true : false;
            set => SetProperty(ref _showTimeRange, value);
        }
        public string SelectedAPI
        {
            get => _selectedAPI;
            set => SetProperty(ref _selectedAPI, value);
        }
        private string _selectedError;
        public string SelectedError
        {
            get => _selectedError;
            set => SetProperty(ref _selectedError, value);
        }

        public ICommand ExportCommand => _exportCommand;

        public ResultAPIVM()
        {
            _exportCommand = new DelegateCommand(OnExport);
        }

        public ResultAPIVM(string sourcePath)
        {
            _exportCommand = new DelegateCommand(OnExport);
            SourcePath = sourcePath;
            DataLoader(sourcePath);
        }

        private void OnExport(object commandParameter) 
        {
            var dg = commandParameter as DataGrid;
            var tbTS = Helper.FindChild<TextBlock>(dg, "tbTS");
            if (tbTS.Text.Contains("UTC"))
            {
                ExportToExcel<PointUTCModel> s = new ExportToExcel<PointUTCModel>();
                ICollectionView view = CollectionViewSource.GetDefaultView(dg.ItemsSource);
                var sc = (List<double?[]>)view.SourceCollection;
                var scfix = from x in sc select new PointUTCModel { TimeStamp_UTC = Helper.UnixTimeStampToDateTime(x[0].NullableDoubleToDouble(), true), Vol = x[1].NullableDoubleToString() };
                s.dataToPrint = scfix.ToList();
                s.GenerateReport();
            }
            else
            {
                ExportToExcel<PointLocalTimeModel> s = new ExportToExcel<PointLocalTimeModel>();
                ICollectionView view = CollectionViewSource.GetDefaultView(dg.ItemsSource);
                var sc = (List<double?[]>)view.SourceCollection;
                var scfix = from x in sc select new PointLocalTimeModel { TimeStamp_LocalTime = Helper.UnixTimeStampToDateTime(x[0].NullableDoubleToDouble()), Vol = x[1].NullableDoubleToString() };
                s.dataToPrint = scfix.ToList();
                s.GenerateReport();
            }
        }

        public void DataLoader(string sourcePath)
        {
            SourceData = Helper.JsonDataLoader(sourcePath);
            SelectedController = SourceData.Find(x => x.A[0].ControlId.StartsWith("api_requests"));
        }
    }
}
