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
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
            lineasiguiente.WidthRequest = btnNext.Width;
        }


        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbxMujer.IsChecked)
            {
                cbxMujer.IsChecked = true;
                cbxHombre.IsChecked = false;
            } 
            
            if (cbxHombre.IsChecked)
            {
                cbxMujer.IsChecked = false;
                cbxHombre.IsChecked = true;
            }
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            cbxMujer.IsChecked = true;
            cbxHombre.IsChecked = false;
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            cbxMujer.IsChecked = false;
            cbxHombre.IsChecked = true;
        }
    }
}