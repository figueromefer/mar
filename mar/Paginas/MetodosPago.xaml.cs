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
    public partial class MetodosPago : ContentPage
    {
        string domicilioseleccionado = "";
        string oxxoseleccionado = "";

        public MetodosPago()
        {
            InitializeComponent();
            cargaroxxo();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            cargardomicilios();
            
        }

        public async void cargaroxxo()
        {
            try
            {
                Label lblnombre = new Label() { Text = "Pago en oxxo", TextColor = Color.White, FontSize = 18, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
                Label lblnumero = new Label() { Text = "Sin entrega el mismo día", TextColor = Color.White, FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
                StackLayout stackdatos = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { lblnombre, lblnumero } };

                CheckBox checkdomicilio = new CheckBox() { ClassId = "OXXO", HorizontalOptions = LayoutOptions.End, Color = Color.FromHex("#CF7667"), IsChecked = false };
                checkdomicilio.CheckedChanged += (sender, e) =>
                {
                    if (checkdomicilio.IsChecked)
                    {
                        oxxoseleccionado = checkdomicilio.ClassId;
                        limpiartarjeta();
                    }
                };
                Frame framecheck = new Frame() { ClassId = "OXXO", BackgroundColor = Color.Transparent, Padding = new Thickness(5), CornerRadius = 50, Content = checkdomicilio };
                StackLayout stackdomicilio = new StackLayout() { Orientation = StackOrientation.Horizontal, BackgroundColor = Color.FromHex("#20C4C4C4"), Padding = new Thickness(40, 10, 20, 10), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { stackdatos, framecheck } };
                stktarjetas.Children.Add(stackdomicilio);
            }
            catch (Exception ex)
            {

            }
        }

        public async void limpiartarjeta()
        {
            try
            {
                entrynumero.Text = "";
                entrycvv.Text = "";
                entrynombre.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

        public async void limpiarchecksdomicilios()
        {
            try
            {
                for (int i = 0; i < stkdomicilios.Children.Count; i++)
                {
                    StackLayout stackdomicilio = (StackLayout)stkdomicilios.Children[i];
                    CheckBox checkbox1 = (CheckBox)((Frame)stackdomicilio.Children[1]).Content;
                    if (checkbox1.ClassId != domicilioseleccionado)
                    {
                        checkbox1.IsChecked = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void cargardomicilios()
        {
            stkdomicilios.Children.Clear();
            try
            {
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/domicilios.php?&usuario={0}", Settings.Idusuario);
                var response2 = await httpRequest(uriString2);
                List<class_domicilios> valor = new List<class_domicilios>();
                valor = procesardomicilio(response2);
                for (int i = 0; i < valor.Count(); i++)
                {


                    Label lblnombre = new Label() { Text = valor.ElementAt(i).domicilio, TextColor = Color.White, FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
                    Label lblnumero = new Label() { Text = "#"+valor.ElementAt(i).numero, TextColor = Color.White, FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
                    Label lblcolonia = new Label() { Text = valor.ElementAt(i).colonia, TextColor = Color.White, FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
                    Label lblcp = new Label() { Text = valor.ElementAt(i).cp, TextColor = Color.White, FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
                    StackLayout stackdatos = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { lblnombre, lblnumero, lblcolonia, lblcp } };

                    CheckBox checkdomicilio = new CheckBox() { ClassId = valor.ElementAt(i).id, HorizontalOptions = LayoutOptions.End, Color = Color.FromHex("#CF7667"), IsChecked = false };
                    checkdomicilio.CheckedChanged += (sender, e) =>
                    {
                        if (checkdomicilio.IsChecked)
                        {
                            domicilioseleccionado = checkdomicilio.ClassId;
                            limpiarchecksdomicilios();
                        }
                    };
                    Frame framecheck = new Frame() { ClassId = valor.ElementAt(i).id, BackgroundColor = Color.Transparent, Padding = new Thickness(5), CornerRadius = 50, Content = checkdomicilio };
                    

                    StackLayout stackdomicilio = new StackLayout() { Orientation = StackOrientation.Horizontal, BackgroundColor = Color.FromHex("#20C4C4C4"), Padding = new Thickness(40, 10, 20, 10), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { stackdatos, framecheck } };
                    stkdomicilios.Children.Add(stackdomicilio);
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

        public List<class_tarjeta> procesartarjeta(string respuesta)
        {
            List<class_tarjeta> items = new List<class_tarjeta>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_tarjeta
                             {
                                 id = WebUtility.UrlDecode((string)r.Element("id")),
                                 nombre = WebUtility.UrlDecode((string)r.Element("nombre")),
                                 numero = WebUtility.UrlDecode((string)r.Element("numero")),
                                 fecha = WebUtility.UrlDecode((string)r.Element("fecha")),
                             }).ToList();
                }
            }
            return items;
        }

        public List<class_domicilios> procesardomicilio(string respuesta)
        {
            List<class_domicilios> items = new List<class_domicilios>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_domicilios
                             {
                                 id = WebUtility.UrlDecode((string)r.Element("id")),
                                 domicilio = WebUtility.UrlDecode((string)r.Element("domicilio")),
                                 colonia = WebUtility.UrlDecode((string)r.Element("colonia")),
                                 numero = WebUtility.UrlDecode((string)r.Element("numero")),
                                 cp = WebUtility.UrlDecode((string)r.Element("cp")),
                             }).ToList();
                }
            }
            return items;
        }

        async void ntarjeta_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NewCard());
        }

        async void ndomicilio_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NewDomicilio());
        }

        async void continuar_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                if(domicilioseleccionado != "")
                {
                    if(oxxoseleccionado != "")
                    {
                        UserDialogs.Instance.ShowLoading("Autenticando método de pago con Conekta");
                        
                            UserDialogs.Instance.HideLoading();
                            await Navigation.PushAsync(new Resumen("OXXO", domicilioseleccionado,"",""));
                        
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading("Autenticando método de pago con Conekta");
                        int continuar = 1;
                        string numero = entrynumero.Text;
                        numero = numero.Replace(" ", "");
                        string ano1 = "20" + datefecha.Date.Year.ToString().Substring(2);
                        string mes1 = datefecha.Date.Month.ToString();
                        string cvv = entrycvv.Text;
                        string fecha = datefecha.Date.Year.ToString().Substring(2) + "/" + datefecha.Date.Month.ToString();
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
                            if (token != "")
                            {
                                UserDialogs.Instance.HideLoading();
                                await Navigation.PushAsync(new Resumen(token, domicilioseleccionado, nombre, numero));
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                UserDialogs.Instance.Alert("Algo salió mal. Revisa la información ingresada.");
                            }

                        }
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("Debes seleccionar o ingresar un domicilio de entrega.");
                }
                
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Algo salió mal. Revisa la información ingresada.");
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}