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
using WpfAppSultanovaED.View;
using WpfAppSultanovaED.ViewModel;

namespace WpfAppSultanovaED.ViewModel
{
    public class MainViewModel
    {
        public RoleViewModel RoleVM { get; }
        public PersonViewModel PersonVM { get; }

        public MainViewModel()
        {
            // ������ ������ �� JSON
            RoleVM = new RoleViewModel();
            PersonVM = new PersonViewModel(RoleVM);
        }

        // ������� ��� ����
        public ICommand ShowRolesCommand { get; }
        public ICommand ShowEmployeesCommand { get; }

        private void ShowRoles()
        {
            var window = new WindowRole(RoleVM);
            window.ShowDialog();
        }

        private void ShowEmployees()
        {
            var window = new WindowEmployee(PersonVM);
            window.ShowDialog();
        }
    }
}