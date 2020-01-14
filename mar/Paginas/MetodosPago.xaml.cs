using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MetodosPago : ContentPage
    {
        public MetodosPago()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            cargartarjetas();
            cargardomicilios();
        }

        public async void cargartarjetas()
        {
            stktarjetas.Children.Clear();
            try
            {
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/tarjetas.php?&usuario={0}", Settings.Idusuario);
                var response2 = await httpRequest(uriString2);
                List<class_tarjeta> valor = new List<class_tarjeta>();
                valor = procesartarjeta(response2);
                for (int i = 0; i < valor.Count(); i++)
                {


                    Label lblnombre = new Label() { Text = valor.ElementAt(i).nombre, TextColor = Color.White, FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
                    int inicio = valor.ElementAt(i).numero.Length - 5;
                    Label lblnumero = new Label() { Text = "xxxx xxxx xxxx "+valor.ElementAt(i).numero.Substring(inicio, 5), TextColor = Color.White, FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
                    StackLayout stackdatos = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { lblnombre, lblnumero} };

                    CheckBox checktarjeta = new CheckBox() { HorizontalOptions = LayoutOptions.End, Color = Color.FromHex("#CF7667"), IsChecked= false };
                    Frame framecheck = new Frame() { ClassId=valor.ElementAt(i).id, BackgroundColor = Color.Transparent, Padding = new Thickness(5),  CornerRadius = 50,  Content = checktarjeta };
                    framecheck.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            try
                            {
                                seleccionartarjeta(framecheck.ClassId);
                            }
                            catch (Exception ex)
                            {
                                Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });

                    StackLayout stacktarjeta = new StackLayout() { Orientation = StackOrientation.Horizontal, BackgroundColor = Color.FromHex("#20C4C4C4"), Padding = new Thickness(40, 10, 20, 10), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { stackdatos , framecheck } };
                    stktarjetas.Children.Add(stacktarjeta);
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

                    CheckBox checktarjeta = new CheckBox() { HorizontalOptions = LayoutOptions.End, Color = Color.FromHex("#CF7667"), IsChecked = false };
                    Frame framecheck = new Frame() { ClassId = valor.ElementAt(i).id, BackgroundColor = Color.Transparent, Padding = new Thickness(5), CornerRadius = 50, Content = checktarjeta };
                    framecheck.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            try
                            {
                                seleccionartarjeta(framecheck.ClassId);
                            }
                            catch (Exception ex)
                            {
                                Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });

                    StackLayout stackdomicilio = new StackLayout() { Orientation = StackOrientation.Horizontal, BackgroundColor = Color.FromHex("#20C4C4C4"), Padding = new Thickness(40, 10, 20, 10), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { stackdatos, framecheck } };
                    stkdomicilios.Children.Add(stackdomicilio);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void seleccionartarjeta(string id)
        {
            try
            {

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
            throw new NotImplementedException();
        }

        async void ndomicilio_Clicked(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        async void continuar_Clicked(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}