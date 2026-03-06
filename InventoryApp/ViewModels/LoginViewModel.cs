using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.ViewModels
{
    public partial class LoginViewModel: ObservableObject
    {
        [RelayCommand]
        public async void Login()
        {
            await Shell.Current.GoToAsync("//MainMenu");
        }
    }
}
