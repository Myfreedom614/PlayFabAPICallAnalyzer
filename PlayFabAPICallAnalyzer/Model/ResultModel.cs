using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFabAPICallAnalyzer.Model
{
    public class ResultModel
    {
        public string TitleId { get; set; }
        public string APIName { get; set; }
        public string Result { get; set; }
        public double TotalCount { get; set; }
        public List<SeriesItemModel> Series { get; set; }
    }

    public class ResultReportModel
    {
        public string TitleId { get; set; }
        public string APIName { get; set; }
        public double TotalCount { get; set; }
    }
}
