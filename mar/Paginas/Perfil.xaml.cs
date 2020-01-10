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
    public partial class Perfil : ContentPage
    {
        public Perfil()
        {
            InitializeComponent();
            enabledDisabled(false);
        }

        private void BtnEditar_Clicked(object sender, EventArgs e)
        {
            enabledDisabled(true);
        }

        private void CheckBox_Checked(object sender, CheckedChangedEventArgs e)
        {
            if (cbxMujer.IsChecked)
            {
                cbxMujer.IsChecked = true;
                cbxMujer.Color = Color.FromHex("#cf7667");
                cbxHombre.Color = Color.FromHex("#3D454C");
                cbxHombre.IsChecked = false;
            }

            if (cbxHombre.IsChecked)
            {
                cbxHombre.IsChecked = true;
                cbxHombre.Color = Color.FromHex("#cf7667");
                cbxMujer.Color = Color.FromHex("#3D454C");
                cbxMujer.IsChecked = false;
            }
        }

        private void LblMujer_Tapped(object sender, EventArgs e)
        {
            cbxMujer.IsChecked = true;
            cbxHombre.IsChecked = false;
        }

        private void LblHombre_Tapped(object sender, EventArgs e)
        {
            cbxHombre.IsChecked = true;
            cbxMujer.IsChecked = false;
        }

        private void enabledDisabled(Boolean estado)
        {
            txtNombre.IsEnabled = estado;
            txtCelular.IsEnabled = estado;
            txtContrasena.IsEnabled = estado;
        }
    }
}
