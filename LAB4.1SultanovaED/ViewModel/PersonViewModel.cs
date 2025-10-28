using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    public class PersonViewModel
{
    public ObservableCollection<Person> ListPerson { get; set; } = new ObservableCollection<Person>();

    public PersonViewModel()
    {
        this.ListPerson.Add(new Person
        {
            Id = 1,
            RoleId = 1,
            FirstName = "Иван",
            LastName = "Иванов",
            Birthday = new DateTime(1980, 02, 28)
        });
        this.ListPerson.Add(new Person
        {
            Id = 2,
            RoleId = 2,
            FirstName = "Петр",
            LastName = "Петров",
            Birthday = new DateTime(1981, 03, 20)
        });
        this.ListPerson.Add(new Person
        {
            Id = 3,
            RoleId = 1,
            FirstName = "Мария",
            LastName = "Сидорова",
            Birthday = new DateTime(1990, 05, 15)
        });
    }
}
