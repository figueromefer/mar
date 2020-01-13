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
    public partial class Carrito : ContentPage
    {
        public Carrito()
        {
            InitializeComponent();
        }

        void BtnAceptar_Clicked(object sender, System.EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new MetodoPago());
        }
    }
}