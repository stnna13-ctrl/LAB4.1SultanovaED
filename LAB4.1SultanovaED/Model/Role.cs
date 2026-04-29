using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAppSultanovaED.Model
{
    public class Role : INotifyPropertyChanged
    {
        /// <summary>
        /// код должности
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// наименование должности
        /// </summary>
        private string nameRole;

        /// <summary>
        /// наименование должности
        /// </summary>
        public string NameRole
        {
            get { return nameRole; }
            set
            {
                nameRole = value;
                OnPropertyChanged("NameRole");
            }
        }

        public Role() { }

        public Role(int id, string nameRole)
        {
            this.Id = id;
            this.NameRole = nameRole;
        }

        /// <summary>
        /// Метод поверхностного копирования
        /// </summary>
        /// <returns></returns>
        public Role ShallowCopy()
        {
            return (Role)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
