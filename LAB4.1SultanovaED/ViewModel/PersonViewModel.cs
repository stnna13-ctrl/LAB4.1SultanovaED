using System;
using System.Collections.Generic;
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
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "DataModels", "PersonData.json");

        private readonly RoleViewModel _roleVM;

        private string _jsonPersons = string.Empty;
        public string Error { get; set; }
        public string Message { get; set; }

        private PersonDpo _selectedPersonDpo;
        public PersonDpo SelectedPersonDpo
        {
            get => _selectedPersonDpo;
            set
            {
                _selectedPersonDpo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Коллекция сотрудников (модель данных)
        /// </summary>
        public ObservableCollection<Person> ListPerson { get; set; }

        /// <summary>
        /// Коллекция данных для отображения сотрудников
        /// </summary>
        public ObservableCollection<PersonDpo> ListPersonDpo { get; set; }

        public PersonViewModel(RoleViewModel roleViewModel)
        {
            _roleVM = roleViewModel;

            ListPerson = LoadPerson();
            ListPersonDpo = new ObservableCollection<PersonDpo>();

            RefreshPersonDpoList();

            _roleVM.ListRole.CollectionChanged += (_, _) => RefreshPersonDpoList();
        }

        private void RefreshPersonDpoList()
        {
            ListPersonDpo.Clear();
            foreach (var p in ListPerson)
            {
                var role = _roleVM.ListRole.FirstOrDefault(r => r.Id == p.RoleId);
                ListPersonDpo.Add(new PersonDpo
                {
                    Id = p.Id,
                    RoleName = role?.NameRole ?? "—",
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Birthday = p.Birthday
                });
            }
        }

        public ICommand AddPerson => new RelayCommand(_ =>
        {
            var wn = new WindowNewEmployee(_roleVM) { Title = "Новый сотрудник" };
            var per = new PersonDpo
            {
                Id = ListPerson.Any() ? ListPerson.Max(p => p.Id) + 1 : 1,
                Birthday = DateTime.Today.AddYears(-30)
            };
            wn.DataContext = per;

            if (wn.ShowDialog() == true)
            {
                try
                {
                    ListPersonDpo.Add(per);
                    var newPerson = new Person();
                    newPerson.CopyFromPersonDPO(per, _roleVM);
                    ListPerson.Add(newPerson);
                    SaveChanges(ListPerson);
                }
                catch (Exception e)
                {
                    Error = "Ошибка добавления данных в json-файл\n" + e.Message;
                }
            }
        });

        public ICommand EditPerson => new RelayCommand(_ =>
        {
            if (SelectedPersonDpo == null) return;

            var wn = new WindowNewEmployee(_roleVM) { Title = "Редактирование сотрудника" };
            var temp = SelectedPersonDpo.ShallowCopy();
            wn.DataContext = temp;

            if (wn.ShowDialog() == true)
            {
                SelectedPersonDpo.RoleName  = temp.RoleName;
                SelectedPersonDpo.FirstName = temp.FirstName;
                SelectedPersonDpo.LastName  = temp.LastName;
                SelectedPersonDpo.Birthday  = temp.Birthday;

                var person = ListPerson.FirstOrDefault(p => p.Id == SelectedPersonDpo.Id);
                if (person != null)
                {
                    person.CopyFromPersonDPO(SelectedPersonDpo, _roleVM);
                }

                try
                {
                    SaveChanges(ListPerson);
                }
                catch (Exception e)
                {
                    Error = "Ошибка редактирования данных в json-файле\n" + e.Message;
                }
            }
        }, _ => SelectedPersonDpo != null);

        public ICommand DeletePerson => new RelayCommand(_ =>
        {
            if (SelectedPersonDpo == null) return;

            if (MessageBox.Show($"Удалить {SelectedPersonDpo.LastName} {SelectedPersonDpo.FirstName}?",
                "Подтверждение", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                try
                {
                    var personDpo = SelectedPersonDpo;
                    ListPersonDpo.Remove(personDpo);

                    var person = ListPerson.FirstOrDefault(p => p.Id == personDpo.Id);
                    if (person != null)
                    {
                        ListPerson.Remove(person);
                        SaveChanges(ListPerson);
                    }
                }
                catch (Exception e)
                {
                    Error = "Ошибка удаления данных\n" + e.Message;
                }
            }
        }, _ => SelectedPersonDpo != null);

        /// <summary>
        /// Загрузка данных о сотрудниках из json-файла
        /// </summary>
        public ObservableCollection<Person> LoadPerson()
        {
            try
            {
                if (!File.Exists(path))
                {
                    Error = "Файл с данными о сотрудниках не найден";
                    return new ObservableCollection<Person>();
                }

                _jsonPersons = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(_jsonPersons))
                {
                    Error = "Файл с данными о сотрудниках пуст";
                    return new ObservableCollection<Person>();
                }

                var serializer = new JavaScriptSerializer();
                var list = serializer.Deserialize<ObservableCollection<Person>>(_jsonPersons);
                return list ?? new ObservableCollection<Person>();
            }
            catch (Exception e)
            {
                Error = "Ошибка чтения json-файла с данными о сотрудниках\n" + e.Message;
                return new ObservableCollection<Person>();
            }
        }

        /// <summary>
        /// Сохранение json-строки с данными о сотрудниках в файл
        /// </summary>
        private void SaveChanges(ObservableCollection<Person> listPersons)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                var jsonPerson = serializer.Serialize(listPersons);

                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(path, jsonPerson);
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json-файла с данными о сотрудниках\n" + e.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}