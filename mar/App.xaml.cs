using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mar
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if(Settings.Idusuario == "")
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
               MainPage = new NavigationPage(new RootPage());
               //MainPage = new NavigationPage(new Pruebas());
            }
            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
