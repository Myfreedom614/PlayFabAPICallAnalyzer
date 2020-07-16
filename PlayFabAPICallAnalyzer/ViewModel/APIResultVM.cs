using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using PlayFabAPICallAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PlayFabAPICallAnalyzer.ViewModel
{
    public class APIResultVM : ViewModelBase
    {
        private string _sourcePath;
        private string _selectedAPI;
        private List<MItemModel> _sourceData;
        private List<ResultModel> _resultSource;
        private MItemModel _selectedController;
        private bool _showTimeRange = false;
        private string _selectedError;
        private SeriesItemModel _selectedSeriesItem;
        private List<DataPoint> _dataPoints;
        private bool _isUtc;
        private double _dataMax;
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
            set { 
                SetProperty(ref _selectedController, value);
                ShowTimeRange = value != null ? true : false;

                if (SelectedController.A.Count > 0)
                {
                    var selectedCol = SelectedController.A[0].SeriesCollection;
                    var resultModel = from sc in selectedCol
                                      group sc by sc.apiName into newGroup
                                      orderby newGroup.Key
                                      select new ResultModel() { APIName = newGroup.Key, Series = newGroup.ToList() };
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
        public string SelectedError
        {
            get => _selectedError;
            set => SetProperty(ref _selectedError, value);
        }
        public SeriesItemModel SelectedSeriesItem
        {
            get => _selectedSeriesItem;
            set { SetProperty(ref _selectedSeriesItem, value);
                UpdateChart(_isUtc);
            }
        }
        public List<DataPoint> DataPoints
        {
            get => _dataPoints;
            set => SetProperty(ref _dataPoints, value);
        }
        public bool IsUtc
        {
            get => _isUtc;
            set {
                if(_isUtc != value) UpdateChart(value);
                SetProperty(ref _isUtc, value);
            }
        }
        public double DataMax
        {
            get => _dataMax;
            set => SetProperty(ref _dataMax, value);
        }
        public ICommand ExportCommand => _exportCommand;
        public APIResultVM()
        {
            IsUtc = true;
            _exportCommand = new DelegateCommand(OnExport);
        }

        public APIResultVM(string sourcePath)
        {
            IsUtc = true;
            _exportCommand = new DelegateCommand(OnExport);
            SourcePath = sourcePath;
            DataLoader(sourcePath);
        }

        private void OnExport(object commandParameter)
        {
            var dg = commandParameter as DataGrid;
            if (_isUtc)
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
        private IEnumerable<DataPoint> ConvertPointModel(SeriesItemModel sim, bool isUTC)
        {
            if (sim != null)
            {
                var data = from x in sim.pointlist select new DataPoint(DateTimeAxis.ToDouble(Helper.UnixTimeStampToDateTime(x[0].NullableDoubleToDouble(), isUTC)), x[1].NullableDoubleToDouble());
                var max = data.Max(x => x.Y);
                var min = data.Min(x => x.Y);
                min = double.IsNaN(min) ? 0.0 : min;
                var buff = (max > min) ? (max - min) / 10 : 10;
                DataMax = double.IsNaN(max) ? 100 : max + buff;
                return data;
            }
            return null;
        }

        private void UpdateChart(bool isUTC)
        {
            if (_selectedSeriesItem != null) {
                var cpm = ConvertPointModel(_selectedSeriesItem, isUTC);
                DataPoints = cpm != null ? cpm.ToList() : new List<DataPoint>();
            }
            
        }

        public void DataLoader(string sourcePath)
        {
            SourceData = Helper.JsonDataLoader(sourcePath);
            SelectedController = SourceData.Find(x => x.A[0].ControlId.StartsWith("api_requests"));
        }
    }
}
