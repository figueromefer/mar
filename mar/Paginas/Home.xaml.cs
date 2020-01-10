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
    public partial class Home : ContentPage
    {
        string categoria = "Camarones";
        public Home()
        {
            InitializeComponent();
            Cargar_productos();
        }

        public async void Cargar_productos()
        {
            string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/productos.php?categoria={0}", categoria);
            var response2 = await httpRequest(uriString2);
            List<class_productos> valor = new List<class_productos>();
            valor = procesar2(response2);
            for (int i = 0; i < valor.Count(); i++)
            {
                Image imagenproducto = new Image() { Source = ImageSource.FromFile("proddemo.png"), WidthRequest = 100 };
                Label tituloproducto = new Label() { Text = valor.ElementAt(i).titulo, FontSize=16, TextColor = Color.FromHex("#888888"),FontFamily = Device.OnPlatform("Lato-Bold", "Lato-Bold.ttf#Lato-Bold", null) };
                Label descripcionproducto = new Label() { Text = valor.ElementAt(i).descripcion, FontSize = 12, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };

                Label precio = new Label() { Text = valor.ElementAt(i).precio, FontSize = 12, TextColor = Color.FromHex("#CF7667"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };
                Label menos = new Label() { BackgroundColor = Color.FromHex("#CF7667"), HorizontalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 2, WidthRequest = 10, VerticalOptions = LayoutOptions.Center };
                Frame framemenos = new Frame() { BorderColor = Color.FromHex("#CF7667"), Padding = new Thickness(0), WidthRequest = 20, HeightRequest = 20, CornerRadius = 20, IsClippedToBounds = true, Margin = new Thickness(10, 0, 10, 0), Content = menos };

                Label cantidad = new Label() { Text = "0", FontSize = 12, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };

                Label mas1 = new Label() { BackgroundColor = Color.FromHex("#CF7667"), HorizontalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 2, WidthRequest = 10, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, 9, 0, 0) };
                Label mas2 = new Label() { BackgroundColor = Color.FromHex("#CF7667"), HorizontalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 10, WidthRequest = 2, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, -6, 0, 0) };
                StackLayout stackmas = new StackLayout() { Spacing = 0, Children = { mas1, mas2 } };
                Frame framemas = new Frame() { BorderColor = Color.FromHex("#CF7667"), Padding = new Thickness(0), WidthRequest = 20, HeightRequest = 20, CornerRadius = 20, IsClippedToBounds = true, Margin = new Thickness(10, 0, 10, 0), Content = stackmas };

                StackLayout stackprecio_cantidad = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(0, 20, 0, 0), Children = { precio, framemenos, cantidad,framemas } };

                StackLayout stackdetalle = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(15, 20, 0, 15), Children = { tituloproducto, descripcionproducto, stackprecio_cantidad } };
                StackLayout stackproducto = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { imagenproducto, stackdetalle } };
                stkproductos.Children.Add(stackproducto);
            }

        }

        public List<class_productos> procesar2(string respuesta)
        {
            List<class_productos> items = new List<class_productos>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_productos
                             {
                                 id = WebUtility.UrlDecode((string)r.Element("id")),
                                 titulo = WebUtility.UrlDecode((string)r.Element("titulo")),
                                 descripcion = WebUtility.UrlDecode((string)r.Element("descripcion")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                                 precio = WebUtility.UrlDecode((string)r.Element("precio")),
                                 inventario = WebUtility.UrlDecode((string)r.Element("inventario")),
                                 categoria = WebUtility.UrlDecode((string)r.Element("categoria")),
                                 tags = WebUtility.UrlDecode((string)r.Element("tags")),
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