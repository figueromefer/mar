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
    public partial class Carrito : ContentPage
    {

        List<string> carrito = new List<string>();

        public Carrito()
        {
            Title = "Carrito";
            InitializeComponent();
            cargarcarrito();
        }

        public async void cargarcarrito()
        {
            UserDialogs.Instance.ShowLoading("Cargando carrito");
            stkproductos.Children.Clear();
            try
            {
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/carrito.php?&pedido={0}", Settings.Pedido);
                var response2 = await httpRequest(uriString2);
                List<class_carrito> valor = new List<class_carrito>();
                valor = procesar2(response2);
                double subtotal = 0;
                for (int i = 0; i < valor.Count(); i++)
                {
                    Image imagenproducto = new Image() { Source = ImageSource.FromFile("proddemo.png"), WidthRequest = 100 };
                    Label tituloproducto = new Label() { Text = valor.ElementAt(i).titulo, FontSize = 16, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Bold", "Lato-Bold.ttf#Lato-Bold", null) };
                    Label descripcionproducto = new Label() { Text = valor.ElementAt(i).descripcion, FontSize = 12, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };

                    Label precio = new Label() { Text = "$" + valor.ElementAt(i).precio, ClassId = valor.ElementAt(i).id, FontSize = 12, TextColor = Color.FromHex("#CF7667"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };
                    subtotal = subtotal + double.Parse(valor.ElementAt(i).precio);
                    Label lblcantidad = new Label() { Text="Cantidad: ", FontSize = 12, TextColor=Color.FromHex("#888888"), HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.End };
                    Label cantidad = new Label() { Text = valor.ElementAt(i).cantidad, FontSize = 12, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };

                    Label mas1 = new Label() {  BackgroundColor = Color.FromHex("#CF7667"), HorizontalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 2, WidthRequest = 15, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, 13, 0, 0) };
                    Label mas2 = new Label() {  BackgroundColor = Color.FromHex("#CF7667"), HorizontalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 15, WidthRequest = 2, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, -9, 0, 0) };
                    StackLayout stackmas = new StackLayout() { Spacing = 0, Children = { mas1, mas2 } };
                    Frame frameequis = new Frame() { Rotation = 45,BorderColor = Color.FromHex("#CF7667"), Padding = new Thickness(0), WidthRequest = 30, HeightRequest = 30, CornerRadius = 30, IsClippedToBounds = true, Margin = new Thickness(10, -10, 10, 0), Content = stackmas };
                    frameequis.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            try
                            {
                                eliminarproducto((StackLayout)frameequis.Parent);
                            }
                            catch (Exception ex)
                            {
                                Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });
                    StackLayout stackprecio_cantidad = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(0, 20, 0, 0), Children = { precio, lblcantidad,cantidad, frameequis } };

                    StackLayout stackdetalle = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(15, 20, 0, 15), Children = { tituloproducto, descripcionproducto, stackprecio_cantidad } };
                    StackLayout stackproducto = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { imagenproducto, stackdetalle } };
                    stkproductos.Children.Add(stackproducto);
                }
                LblSubtotal.Text = "$ " + subtotal;
            }
            catch (Exception ex)
            {

            }
            UserDialogs.Instance.HideLoading();
        }

        public async void eliminarproducto(StackLayout stackprecio_cantidad)
        {
            try
            {
                string idproductocarrito = ((Label)stackprecio_cantidad.Children[0]).ClassId;
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/eliminardecarrito.php?&idcarrito={0}", idproductocarrito);
                var response2 = await httpRequest(uriString2);
                if(response2 == "1")
                {
                    cargarcarrito();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<class_carrito> procesar2(string respuesta)
        {
            List<class_carrito> items = new List<class_carrito>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_carrito
                             {
                                 id = WebUtility.UrlDecode((string)r.Element("id")),
                                 titulo = WebUtility.UrlDecode((string)r.Element("titulo")),
                                 descripcion = WebUtility.UrlDecode((string)r.Element("descripcion")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                                 precio = WebUtility.UrlDecode((string)r.Element("precio")),
                                 cantidad = WebUtility.UrlDecode((string)r.Element("cantidad")),
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



        void BtnAceptar_Clicked(object sender, System.EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new MetodoPago());
        }
    }
}