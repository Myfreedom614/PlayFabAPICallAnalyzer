using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;

namespace PlayFabAPICallAnalyzer.Control
{
    public class DataGridEx : DataGrid
    {
        public DataGridEx()
        {

        }

        public Boolean State
        {
            get { return (Boolean)this.GetValue(StateProperty); }
            set { this.SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
            "State", typeof(Boolean), typeof(DataGridEx), new PropertyMetadata(true,
                new PropertyChangedCallback(OnFirstPropertyChanged)));

        private static void OnFirstPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = d as DataGridEx;
            var backup = ctl.ItemsSource;
            ctl.ItemsSource = null;
            ctl.ItemsSource = backup;
        }
    }
}
