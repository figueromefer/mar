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
    public partial class Gracias : ContentPage
    {
        string dom = "";
        string tar = "";
        string token1 = "";
        string pedido1 = "";

        public Gracias(string domicilio, string fecha, string hora, string referencia, string pedido, string orden= "")
        {

            Title = "GRACIAS";
            InitializeComponent();
            if(orden != "")
            {
                Label tituloOxxo = new Label() { Text = "Realiza tu pago en el OXXO más cercano presentando el siguiente número de referencia:", FontSize = 16, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Bold", "Lato-Bold.ttf#Lato-Bold", null) };
                header.Children.Add(tituloOxxo);
                lblreferencia.Text = "Referencia OXXO: " + referencia;
            }
            else
            {
                lblreferencia.Text = "Referencia: " + referencia;
            }
           
            lblfecha.Text = "Fecha de entrega: " + fecha;
            lblhora.Text = "Hora de etrega: " + hora;
            pedido1 = pedido;
            cargardomicilio();
            cargarpedido();
            
        }

        public async void cargardomicilio()
        {
            try
            {
                Random random = new Random();
                int num = random.Next(1000);
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/domicilios.php?usuario={0}&rnd={1}&id={2}", Settings.Idusuario, num.ToString(), dom);
                var response2 = await httpRequest(uriString2);
                List<class_domicilios> valor = new List<class_domicilios>();
                valor = procesardomicilio(response2);
                for (int i = 0; i < valor.Count(); i++)
                {
                    lbldomicilio.Text = valor.ElementAt(i).domicilio;
                    lblnumero.Text = valor.ElementAt(i).numero;
                    lblcp.Text = valor.ElementAt(i).cp;
                    lblcolonia.Text = valor.ElementAt(i).colonia;

                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void cargarpedido()
        {
            try
            {
                Random random = new Random();
                int num = random.Next(1000);
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/carrito.php?&pedido={0}&rnd={1}", pedido1, num);
                var response2 = await httpRequest(uriString2);
                List<class_carrito> valor = new List<class_carrito>();
                valor = procesarpedido(response2);
                double subtotal = 0;
                for (int i = 0; i < valor.Count(); i++)
                {
                    Image imagenproducto = new Image() { Source = ImageSource.FromFile("proddemo.png"), WidthRequest = 35 };
                    Label tituloproducto = new Label() { Text = valor.ElementAt(i).titulo, FontSize = 16, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Bold", "Lato-Bold.ttf#Lato-Bold", null) };
                    Label descripcionproducto = new Label() { Text = valor.ElementAt(i).descripcion, FontSize = 12, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };

                    Label precio = new Label() { Text = "$" + valor.ElementAt(i).precio, ClassId = valor.ElementAt(i).id, FontSize = 12, TextColor = Color.FromHex("#CF7667"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };
                    subtotal = subtotal + (double.Parse(valor.ElementAt(i).precio) * double.Parse(valor.ElementAt(i).cantidad));
                    Label lblcantidad = new Label() { Text = "Cantidad: ", FontSize = 12, TextColor = Color.FromHex("#888888"), HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.End };
                    Label cantidad = new Label() { Text = valor.ElementAt(i).cantidad, FontSize = 12, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };


                    StackLayout stackprecio_cantidad = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(0, 20, 0, 0), Children = { precio, lblcantidad, cantidad } };

                    StackLayout stackdetalle = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(15, 20, 0, 15), Children = { tituloproducto, descripcionproducto, stackprecio_cantidad } };
                    StackLayout stackproducto = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { imagenproducto, stackdetalle } };
                    stkpedidos.Children.Add(stackproducto);
                }
            }
            catch (Exception ex)
            {

            }
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

        public List<class_carrito> procesarpedido(string respuesta)
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

        void btnContinuar_Clicked(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new RootPage());
        }
    }
}