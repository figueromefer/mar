using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.IO;
using System.Net;
using System.Xml.Linq;
using Acr.UserDialogs;

namespace mar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Promocion : ContentPage
    {
        public Promocion()
        {
            InitializeComponent();

            Cargar_promociones();
        }

        public async void Cargar_promociones()
        {
            try
            {
                Random rnd = new Random();
                int numero = rnd.Next(1, 100);
                stkpromociones.Children.Clear();
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/promociones.php?"+numero);
                var response2 = await httpRequest(uriString2);
                List<class_promociones> valor = new List<class_promociones>();
                valor = Procesar2(response2);

                for (int i = 0; i < valor.Count(); i++){
                    Image imagen = new Image(){
                        Source = new UriImageSource
                        {
                            Uri = new Uri(valor.ElementAt(i).foto),
                            CachingEnabled = true,
                        },
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    
                    Image ico = new Image()
                    {
                        Source = new UriImageSource
                        {
                            Uri = new Uri("http://boveda-creativa.net/laporciondelmar/Administracion/static/view-controller/img/menutreepoints.png"),
                            CachingEnabled = true,

                        },
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        WidthRequest = 23
                    };

                    Label titulo = new Label()
                    {
                        Text = valor.ElementAt(i).titulo,
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        Padding = new Thickness(20, 15, 20, 15),
                        TextColor = Color.FromHex("#888888"),
                        FontFamily = Device.OnPlatform("Lato-Bold", "Lato-Bold.ttf#Lato-Bold", null)
                    };

                    StackLayout titulohead = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        Children = { titulo, ico }
                    };
                   
                    Label descripcion = new Label() {
                        Text = valor.ElementAt(i).descripcion,
                        HorizontalTextAlignment = TextAlignment.Start,
                        Padding = new Thickness(20, 5, 20, 5),
                        FontSize = 12, TextColor = Color.FromHex("#888888"),
                        FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) 
                    };

                    StackLayout stackbaner = new StackLayout() { 
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        Children = { titulohead, imagen, descripcion } 
                    };

                    stkpromociones.Children.Add(stackbaner);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<class_promociones> Procesar2(string respuesta)
        {
            List<class_promociones> items = new List<class_promociones>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_promociones
                             {
                                 id = WebUtility.UrlDecode((string)r.Element("id")),
                                 titulo = WebUtility.UrlDecode((string)r.Element("titulo")),
                                 descripcion = WebUtility.UrlDecode((string)r.Element("descripcion")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                             }).ToList();
                }
            }
            return items;
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