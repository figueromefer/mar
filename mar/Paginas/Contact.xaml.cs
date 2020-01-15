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
    public partial class Contact : ContentPage
    {
        public Contact()
        {
            InitializeComponent();
        }

        public async void Handle_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                Device.OpenUri(new Uri("mailto:contacto@laporciondelmar.com"));
            }
            catch (Exception ex)
            {

            }
        }
    }
}