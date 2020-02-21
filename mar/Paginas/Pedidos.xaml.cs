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
    public partial class Pedidos : ContentPage
    {
        public Pedidos()
        {

            Title = "Pedidos";
            InitializeComponent();
            cargarpedidos();

        }

        
        public async void cargarpedidos()
        {
            try
            {
                Random random = new Random();
                int num = random.Next(1000);
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/pedidos.php?&usuario={0}&rnd={1}", Settings.Idusuario, num);
                var response2 = await httpRequest(uriString2);
                List<class_pedidos> valor = new List<class_pedidos>();
                valor = procesarpedido(response2);
                double subtotal = 0;
                for (int i = 0; i < valor.Count(); i++)
                {
                    
                    Label lblreferencia = new Label() { Text = "Referencia: "+valor.ElementAt(i).referencia, FontSize = 16, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Bold", "Lato-Bold.ttf#Lato-Bold", null) };
                    Label lblcantidad = new Label() { Text = "$"+valor.ElementAt(i).cantidad, FontSize = 12, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };
                    Label lblestatus = new Label() { Text =  valor.ElementAt(i).estatus, ClassId = valor.ElementAt(i).id, FontSize = 12, TextColor = Color.FromHex("#CF7667"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };
                    Label lblfecha = new Label() { Text = "Fecha: " + valor.ElementAt(i).fecha, FontSize = 12, TextColor = Color.FromHex("#888888"), HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.End };
                    Label lblfechaentrega = new Label() { Text = "Fecha de entrega: " + valor.ElementAt(i).fechaentrega, FontSize = 12, TextColor = Color.FromHex("#888888"), HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.End };
                    Label lblhoraentrega = new Label() { Text = "Hora de engrega: " + valor.ElementAt(i).horaentrega, FontSize = 12, TextColor = Color.FromHex("#888888"), HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.End };





                    BoxView boxview1 = new BoxView() { Margin = new Thickness(0,0,0,15) ,HeightRequest = 1, BackgroundColor = Color.FromHex("#ccc"), HorizontalOptions = LayoutOptions.FillAndExpand};
                    StackLayout stackproducto = new StackLayout() {Children = { lblreferencia, lblcantidad, lblestatus, lblfecha, lblfechaentrega, lblhoraentrega, boxview1 } };
                    stkpedidos.Children.Add(stackproducto);
                }
            }
            catch (Exception ex)
            {

            }
        }

       
        public List<class_pedidos> procesarpedido(string respuesta)
        {
            List<class_pedidos> items = new List<class_pedidos>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_pedidos
                             {
                                 id = WebUtility.UrlDecode((string)r.Element("id")),
                                 referencia = WebUtility.UrlDecode((string)r.Element("referencia")),
                                 cantidad = WebUtility.UrlDecode((string)r.Element("cantidad")),
                                 estatus = WebUtility.UrlDecode((string)r.Element("estatus")),
                                 fecha = WebUtility.UrlDecode((string)r.Element("fecha")),
                                 fechaentrega = WebUtility.UrlDecode((string)r.Element("fechaentrega")),
                                 horaentrega = WebUtility.UrlDecode((string)r.Element("horaentrega")),
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
            App.Current.MainPage = new NavigationPage(new Home());
        }
    }
}