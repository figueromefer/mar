using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
                
            btnRecovery.Clicked += BtnRecovery_Clicked;
        }

        private void BtnRecovery_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new RecoveryPass());
        }

        void BtnLogin_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new RootPage());
        }
    }
}