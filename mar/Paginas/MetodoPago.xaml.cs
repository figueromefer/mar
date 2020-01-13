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

        /*  --------------------------------------- Métodos Pago Code -------------------------------------  */
        private async void FillComponents()
        {
            int id = int.Parse(Settings.Idusuario);
            string uri = string.Format("http://boveda-creativa.net/laporciondelmar/tarjetas.php?id={0}&opcion={1}", id, "Domicilios");
            var response = await HttpRequest(uri);
            var doc = XDocument.Parse(response).Root.Elements("valor").ElementAt(0);


            //WebUtility.UrlDecode((string)doc.Element("id"));
        }

        public async Task<string> HttpRequest(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            string received;
            using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
            {
                using (var responseStream = response.GetResponseStream())
                { using (var sr = new StreamReader(responseStream)) { received = await sr.ReadToEndAsync(); } }
            }
            return received;
        }
    }
}