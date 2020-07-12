using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFabAPICallAnalyzer.Model
{
    public class ResultModel
    {
        public string APIName { get; set; }
        public string Result { get; set; }
        public List<SeriesItemModel> Series { get; set; }
    }
}
