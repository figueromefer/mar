using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Acr.UserDialogs;
using Conekta.Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCard : ContentPage
    {
        public NewCard()
        {
            InitializeComponent();
        }

        async void Guardar_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Registrando tarjeta");
                int continuar = 1;
                string numero = entrynumero.Text;
                numero = numero.Replace(" ", "");
                string ano1 = "20"+datefecha.Date.Year.ToString().Substring(2);
                string mes1 = datefecha.Date.Month.ToString();
                string cvv = entrycvv.Text;
                string fecha = datefecha.Date.Year.ToString().Substring(2)+"/"+ datefecha.Date.Month.ToString();
                string nombre = entrynombre.Text;
                if (numero.Length < 16)
                {
                    continuar = 0;
                    UserDialogs.Instance.Alert("Número incompleto");
                }
                if (nombre.Length < 5)
                {
                    continuar = 0;
                    UserDialogs.Instance.Alert("Nombre incompleto");
                }
                if (continuar == 1)
                {
                    string token = "";
                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            token = await new ConektaTokenizer("key_EU3RzXHR7T279WQsq7LN7ig", RuntimePlatform.iOS).GetTokenAsync(numero, nombre, cvv, int.Parse(ano1), int.Parse(mes1));
                            break;
                        case Device.Android:
                            token = await new ConektaTokenizer("key_EU3RzXHR7T279WQsq7LN7ig", RuntimePlatform.Android).GetTokenAsync(numero, nombre, cvv, int.Parse(ano1), int.Parse(mes1));
                            break;
                    }
                    if(token != "")
                    {
                        string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/ntarjeta.php?&usuario={0}&numero={1}&fecha={2}&nombre={3}&token={4}", Settings.Idusuario, numero, fecha, nombre, token);
                        var response2 = await httpRequest(uriString2);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Tarjeta registrada correctamente");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Algo salió mal con el registro de la tarjeta. Revisa la información ingresada.");
                    }
                    
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        public async Task<string> httpRequest(string url)
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