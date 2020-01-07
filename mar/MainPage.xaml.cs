using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mar
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Go to the Login page
            btnLogin.Clicked += BtnLogin_Clicked;

            // Go the Register page
            btnRegister.Clicked += BtnRegister_Clicked;
        }

        private void BtnRegister_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new Register());
        }

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new Login());
        }
    }
}
