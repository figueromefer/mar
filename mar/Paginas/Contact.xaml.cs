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

        void Whatsapp_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                Device.OpenUri(new Uri("https://api.whatsapp.com/send?phone=3316047748&text=Hola, mi usuario es el:"+Settings.Idusuario));
            }
            catch (Exception ex)
            {

            }
        }

        void Correo_Tapped(System.Object sender, System.EventArgs e)
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