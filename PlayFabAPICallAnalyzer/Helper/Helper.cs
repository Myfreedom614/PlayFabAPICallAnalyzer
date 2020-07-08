using Newtonsoft.Json;
using PlayFabAPICallAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace PlayFabAPICallAnalyzer
{
    public static class Helper
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp, bool isUTC = false)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
            dtDateTime = isUTC == false ? dtDateTime.ToLocalTime() : dtDateTime;
            return dtDateTime;
        }

        public static double NullableDoubleToDouble(this double? d)
        {
            return d.HasValue ? d.Value : Double.NaN;
        }

        public static string NullableDoubleToString(this double? d)
        {
            return d.HasValue ? d.Value.ToString() : string.Empty;
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;

                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);

                    if (foundChild != null) break;
                }
                else
                    if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;

                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                    else
                    {
                        foundChild = FindChild<T>(child, childName);

                        if (foundChild != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static bool IsValidJsonData(string sourcePath)
        {
            var sourceContent = File.ReadAllText(sourcePath).Replace(Environment.NewLine, " ");

            var model = JsonConvert.DeserializeObject<DataDogModel>(sourceContent);

            return model.M !=null && model.M.Count > 0 ? true : false;
        }

        public static List<MItemModel> JsonDataLoader(string sourcePath)
        {
            var sourceContent = File.ReadAllText(sourcePath).Replace(Environment.NewLine, " ");

            var model = JsonConvert.DeserializeObject<DataDogModel>(sourceContent);

            model.M.ForEach(x => x.ControllerName = x.A.FirstOrDefault().ControlId);

            foreach (var mi in model.M)
            {
                foreach (var ma in mi.A)
                {
                    foreach (var sc in ma.SeriesCollection)
                    {
                        Regex rx = new Regex(@"api:(.*),error_code:(.*),",
                            RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        var matches = rx.Matches(sc.scope);
                        if (matches.Count > 0)
                        {
                            sc.apiName = matches[0].Groups[1].Value;
                            sc.result = matches[0].Groups[2].Value;
                        }
                    }
                }
            }

            return model.M
                .Where(x => x.A[0].ControlId.StartsWith("api_requests"))
                .ToList();

            //foreach (var mi in apiResult)
            //{
            //    foreach (var ma in mi.A)
            //    {
            //        Debug.WriteLine("---------------------------------------------");
            //        Debug.WriteLine($"Control: {ma.ControlId}");
            //        Debug.WriteLine($"Query Time Range: {ma.Query.From} - {ma.Query.To}");
            //        Debug.WriteLine($"SeriesCollection:");
            //        foreach (var sc in ma.SeriesCollection)
            //        {
            //            Debug.WriteLine("-------------------");
            //            Debug.WriteLine($"Start: {Helper.UnixTimeStampToDateTime(sc.start, true)}");
            //            Debug.WriteLine($"End: {Helper.UnixTimeStampToDateTime(sc.end, true)}");
            //            Debug.WriteLine($"Internal: {sc.interval}");
            //            Debug.WriteLine($"Length: {sc.length}");
            //            Debug.WriteLine($"Abs Max: {sc.attributes.abs_max}");
            //            Debug.WriteLine($"Scope: {sc.scope}");
            //            Debug.WriteLine($"Points:");
            //            foreach (var pl in sc.pointlist)
            //            {
            //                Debug.WriteLine($"{Helper.UnixTimeStampToDateTime(pl[0].Value, true)} - {pl[1]}");
            //            }
            //            Debug.WriteLine("-------------------");
            //        }
            //    }
            //}
        }
    }
}
