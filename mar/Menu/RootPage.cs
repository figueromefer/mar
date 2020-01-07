using System;

using Xamarin.Forms;

namespace Toss
{
    public class RootPage : MasterDetailPage
    {
        MenuPage menuPage;

        public RootPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            menuPage = new MenuPage();
            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuItem);
            Master = menuPage;
            try
            {
                Detail = new NavigationPage(new Home());
            }
            catch (Exception ex)
            {
                DisplayAlert("Ayuda", ex.Message, "OK");
            }

        }

        async void NavigateTo(MenuItem menu)
        {
            if (menu == null)
                return;

            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);
            if (displayPage.ToString() != "Olu.Logout")
            {
                try
                {
                    //Detail = new NavigationPage(displayPage);
                    await Application.Current.MainPage.Navigation.PushAsync( displayPage);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error " + ex.Message, "Ok");
                }
            }
            menuPage.Menu.SelectedItem = null;
            IsPresented = false;
        }
    }
}

