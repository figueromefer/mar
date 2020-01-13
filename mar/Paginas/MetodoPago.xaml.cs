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
    public partial class MetodoPago : ContentPage
    {
        public MetodoPago()
        {
            InitializeComponent();
        }

        private void BtnConekta_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new MetodosPago());

            cbxTarjeta.Color = Color.FromHex("#cf7667");
            cbxEfectivo.Color = Color.FromHex("#3D454C");

            cbxTarjeta.IsChecked = true;
            cbxEfectivo.IsChecked = false;
        }

        private void BtnPaypal_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new NewCard());

            cbxEfectivo.Color = Color.FromHex("#cf7667");
            cbxTarjeta.Color = Color.FromHex("#3D454C");

            cbxEfectivo.IsChecked = true;
            cbxTarjeta.IsChecked = false;
        }
    }
}