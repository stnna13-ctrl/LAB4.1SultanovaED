using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Input;
using WpfAppSultanovaED.Helper;
using WpfAppSultanovaED.Model;
using WpfAppSultanovaED.View;

namespace WpfAppSultanovaED.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        private readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "DataModels", "RoleData.json");

        public string Error { get; set; }
        private string _jsonRoles = string.Empty;

        private Role selectedRole;

        public Role SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        /// <summary>
        /// Коллекция должностей сотрудников
        /// </summary>
        public ObservableCollection<Role> ListRole { get; set; } = new ObservableCollection<Role>();

        public RoleViewModel()
        {
            ListRole = LoadRole();
        }

        private RelayCommand addRole;
        public RelayCommand AddRole
        {
            get
            {
                return addRole ??= new RelayCommand(_ =>
                {
                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Новая должность"
                    };

                    Role role = new Role { Id = MaxId() + 1 };
                    wnRole.DataContext = role;

                    if (wnRole.ShowDialog() == true)
                    {
                        ListRole.Add(role);
                        SaveChanges(ListRole);
                        SelectedRole = role;
                    }
                }, _ => true);
            }
        }

        private RelayCommand editRole;
        public RelayCommand EditRole
        {
            get
            {
                return editRole ??= new RelayCommand(_ =>
                {
                    if (SelectedRole == null) return;

                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Редактирование должности"
                    };

                    Role role = SelectedRole;
                    Role tempRole = role.ShallowCopy();
                    wnRole.DataContext = tempRole;

                    if (wnRole.ShowDialog() == true)
                    {
                        role.NameRole = tempRole.NameRole;
                        SaveChanges(ListRole);
                    }
                }, _ => SelectedRole != null && ListRole.Count > 0);
            }
        }

        private RelayCommand deleteRole;
        public RelayCommand DeleteRole
        {
            get
            {
                return deleteRole ??= new RelayCommand(_ =>
                {
                    if (SelectedRole == null) return;

                    Role role = SelectedRole;
                    MessageBoxResult result = MessageBox.Show(
                        $"Удалить данные по должности: {role.NameRole}?",
                        "Предупреждение",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.OK)
                    {
                        ListRole.Remove(role);
                        SaveChanges(ListRole);
                    }
                }, _ => SelectedRole != null && ListRole.Count > 0);
            }
        }

        /// <summary>
        /// Загрузка json-файла и десериализация данных в коллекцию ListRole
        /// </summary>
        public ObservableCollection<Role> LoadRole()
        {
            try
            {
                if (!File.Exists(path))
                {
                    Error = "Файл с данными о должностях не найден";
                    return new ObservableCollection<Role>();
                }

                _jsonRoles = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(_jsonRoles))
                {
                    Error = "Файл с данными о должностях пуст";
                    return new ObservableCollection<Role>();
                }

                var serializer = new JavaScriptSerializer();
                var list = serializer.Deserialize<ObservableCollection<Role>>(_jsonRoles);
                return list ?? new ObservableCollection<Role>();
            }
            catch (Exception e)
            {
                Error = "Ошибка чтения json-файла с данными о должностях\n" + e.Message;
                return new ObservableCollection<Role>();
            }
        }

        /// <summary>
        /// Поиск максимального Id в коллекции
        /// </summary>
        public int MaxId()
        {
            int max = 0;
            foreach (var r in ListRole)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                }
            }
            return max;
        }

        /// <summary>
        /// Сохранение json-строки с данными о должностях в файл
        /// </summary>
        private void SaveChanges(ObservableCollection<Role> listRole)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                var jsonRole = serializer.Serialize(listRole);

                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(path, jsonRole);
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json-файла с данными о должностях\n" + e.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}