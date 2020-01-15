using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecoveryPass : ContentPage {
        public RecoveryPass() {
            InitializeComponent();
        }

        private void BtnNext_Clicked(object sender, EventArgs e)
        {
            if(txtCorreo.Text == txtCCorreo.Text)
            {
                recuperar();
            }
            else
            {
                lblMensaje.TextColor = Color.FromHex("#F00");
                lblMensaje.Text = "No coinciden los correos!";
            }
            
        }

        public async void recuperar()
        {
            try
            {
                Random rnd = new Random();
                int numero = rnd.Next(1000);
                string correo = txtCorreo.Text;
                string uri = string.Format("http://boveda-creativa.net/laporciondelmar/recuperar.php?correo={0}&rnd={1}", correo, numero);
                var response = await HttpRequest(uri);

                if (response == "1")
                {
                    lblMensaje.TextColor = Color.FromHex("#FFF");
                    lblMensaje.Text = "Se ha enviado un mensaje a tu correo, en unos momentos recibirás un mail de confirmación";
                    BtnNext.IsEnabled = false;
                }
                else
                {
                    lblMensaje.TextColor = Color.FromHex("#F00");
                    lblMensaje.Text = "No fue pósible enviar el mensaje a tu correo, comprueba que este bien escrito";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =ex.Message;
            }
            
        }

        public async Task<string> HttpRequest(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            string received;
            using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
            {
                using (var responseStream = response.GetResponseStream())
                { using (var sr = new StreamReader(responseStream)) {
                        received = await sr.ReadToEndAsync();
                    } }
            }
            return received;
        }
    }
}