using WpfAppSultanovaED.Model;
using WpfAppSultanovaED.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppSultanovaED.View
{
    public partial class WindowEmployee : Window
    {
        public WindowEmployee(PersonViewModel viewModel)
        {
            InitializeComponent();
            DataContext =  viewModel;
        }
    }
}