using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFabAPICallAnalyzer.Model
{
    public class DataDogModel
    {
        public string C { get; set; }
        public List<MItemModel> M { get; set; }
    }

    public class MItemModel
    {
        public string H { get; set; }
        public string M { get; set; }

        public string ControllerName { get; set; }
        public List<MAItemModel> A { get; set; }
    }

    public class MAItemModel
    {
        public string ControlId { get; set; }
        public dynamic Error { get; set; }
        public List<SeriesItemModel> SeriesCollection { get; set; }
        public QueryModel Query { get; set; }
        public dynamic FilterSignature { get; set; }
    }

    public class SeriesItemModel
    {
        public double start { get; set; }
        public double end { get; set; }
        public int interval { get; set; }
        public int length { get; set; }
        public dynamic metric { get; set; }
        public dynamic aggr { get; set; }
        public AttrModel attributes { get; set; }
        public List<double?[]> pointlist { get; set; }
        public dynamic expression { get; set; }
        public string scope { get; set; }
        public dynamic unit { get; set; }
        public dynamic display_name { get; set; }
        public dynamic group { get; set; }
        public string apiName { get; set; }
        public string result { get; set; }
    }
    public class AttrModel
    {
        public string abs_max { get; set; }
    }

    public class QueryModel
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    public class PointStandardModel
    {
        public DateTime TimeStamp { get; set; }
        public string Vol { get; set; }
    }

    public class PointUTCModel
    {
        public DateTime TimeStamp_UTC { get; set; }
        public string Vol { get; set; }
    }

    public class PointLocalTimeModel
    {
        public DateTime TimeStamp_LocalTime { get; set; }
        public string Vol { get; set; }
    }
}
