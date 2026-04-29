using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppSultanovaED.Model;
using WpfAppSultanovaED.ViewModel;

namespace WpfAppSultanovaED.View
{
    /// <summary>
    /// Логика взаимодействия для WindowNewEmployee.xaml
    /// </summary>
    public partial class WindowNewEmployee : Window
    {
        private RoleViewModel vmRole;


        public WindowNewEmployee(RoleViewModel roleViewModel)
        {
            InitializeComponent();
            vmRole = roleViewModel;


            CbRole.ItemsSource = vmRole.ListRole;
            CbRole.DisplayMemberPath = "NameRole";
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {

            if (CbRole.SelectedItem == null)
            {
                MessageBox.Show("Выберите должность сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (string.IsNullOrWhiteSpace(TbLastName.Text))
            {
                MessageBox.Show("Поле 'Фамилия' должно быть заполнено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TbLastName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TbFirstName.Text))
            {
                MessageBox.Show("Поле 'Имя' должно быть заполнено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TbFirstName.Focus();
                return;
            }

            DialogResult = true;
        }

        private void tbBirthday_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbBirthday.Visibility == Visibility.Hidden)
            {
                ClBirthday.Visibility = Visibility.Visible;
            }
            else
            {
                ClBirthday.Visibility = Visibility.Hidden;
            }
        }
        private void TbLastName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

            Regex regex = new Regex(@"^[а-яА-ЯёЁ]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void TbFirstName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

            Regex regex = new Regex(@"^[а-яА-ЯёЁ]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}