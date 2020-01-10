using System;

using Xamarin.Forms;

namespace mar
{
    public class Cerrar : ContentPage
    {
        public Cerrar()
        {
            try
            {
                Settings.Idusuario = "";
                App.Current.MainPage = new Xamarin.Forms.NavigationPage(new MainPage());
                Navigation.PopAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}

