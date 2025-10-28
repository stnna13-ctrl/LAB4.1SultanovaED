using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class RoleViewModel
{
    public ObservableCollection<Role> ListRole { get; set; } = new ObservableCollection<Role>();

    public RoleViewModel()
    {
        this.ListRole.Add(new Role { Id = 1, NameRole = "Менеджер" });
        this.ListRole.Add(new Role { Id = 2, NameRole = "Разработчик" });
        this.ListRole.Add(new Role { Id = 3, NameRole = "Директор" });
    }
}
