using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlayFabAPICallAnalyzer.View
{
    /// <summary>
    /// Interaction logic for Bootstrapper.xaml
    /// </summary>
    public partial class Bootstrapper : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Bootstrapper()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
