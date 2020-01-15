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
using Acr.UserDialogs;

namespace mar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        string categoria = "Camarones";
        List<string> carrito = new List<string>();
        

        public Home()
        {
            InitializeComponent();
            NavigationPage.SetTitleIconImageSource(this, "title2.png");
            
            
            vercarrito.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    try
                    {
                        iracarrito();
                    }
                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            Cargar_carrito_anterior();
        }

        public async void Cargar_carrito_anterior()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Actualizando carrito");
                carrito.Clear();
                if (Settings.Pedido != "")
                {
                    string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/carrito.php?&pedido={0}", Settings.Pedido);
                    var response2 = await httpRequest(uriString2);
                    List<class_carrito> valor = new List<class_carrito>();
                    valor = procesarcarrito(response2);
                    for (int i = 0; i < valor.Count(); i++)
                    {
                        string stringproducto = valor.ElementAt(i).productoid + "|" + valor.ElementAt(i).cantidad + "|" + valor.ElementAt(i).precio;
                        carrito.Add(stringproducto);
                    }
                    actualizartotal();
                    
                }
                Cargar_productos();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                Cargar_productos();
            }
        }

        public List<class_carrito> procesarcarrito(string respuesta)
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
                                 productoid = WebUtility.UrlDecode((string)r.Element("productoid")),
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

        public async void Cargar_productos()
        {
            try
            {
                stkproductos.Children.Clear();
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/productos.php?categoria={0}&ver=1", categoria);
                var response2 = await httpRequest(uriString2);
                List<class_productos> valor = new List<class_productos>();
                valor = procesar2(response2);
                for (int i = 0; i < valor.Count(); i++)
                {
                    Image imagenproducto = new Image() { Source = ImageSource.FromFile("proddemo.png"), WidthRequest = 100 };
                    Label tituloproducto = new Label() { Text = valor.ElementAt(i).titulo, FontSize = 16, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Bold", "Lato-Bold.ttf#Lato-Bold", null) };
                    Label descripcionproducto = new Label() { Text = valor.ElementAt(i).descripcion, FontSize = 12, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };

                    Label precio = new Label() { Text = "$" + valor.ElementAt(i).precio, ClassId = valor.ElementAt(i).id, FontSize = 12, TextColor = Color.FromHex("#CF7667"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };
                    Label menos = new Label() { BackgroundColor = Color.FromHex("#CF7667"), HorizontalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 2, WidthRequest = 10, VerticalOptions = LayoutOptions.Center };
                    Frame framemenos = new Frame() { BorderColor = Color.FromHex("#CF7667"), Padding = new Thickness(0), WidthRequest = 20, HeightRequest = 20, CornerRadius = 20, IsClippedToBounds = true, Margin = new Thickness(10, 0, 10, 0), Content = menos };
                    framemenos.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => {
                            try
                            {
                                restarcantidad((StackLayout)framemenos.Parent);
                            }
                            catch (Exception ex)
                            {
                                Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });
                    string cantidadactual = "0";
                    for(int j = 0; j < carrito.Count; j++)
                    {
                        if(carrito[j].Split('|')[0] == valor.ElementAt(i).id)
                        {
                            cantidadactual = carrito[j].Split('|')[1];
                        }
                    }
                    
                    Label cantidad = new Label() { Text = cantidadactual, FontSize = 12, TextColor = Color.FromHex("#888888"), FontFamily = Device.OnPlatform("Lato-Regular", "Lato-Regular.ttf#Lato-Regular", null) };
                    BoxView mas4 = new BoxView() { BackgroundColor = Color.FromHex("#CF7667"), HorizontalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 2, WidthRequest = 10, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, 9, 0, 0) };
                    BoxView mas3 = new BoxView() { BackgroundColor = Color.FromHex("#CF7667"), HorizontalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 10, WidthRequest = 2, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, -6, 0, 0) };
                    
                    StackLayout stackmas = new StackLayout() { Spacing = 0, Children = { mas4, mas3 } };
                    Frame framemas = new Frame() { BorderColor = Color.FromHex("#CF7667"), Padding = new Thickness(0), WidthRequest = 20, HeightRequest = 20, CornerRadius = 20, IsClippedToBounds = true, Margin = new Thickness(10, 0, 10, 0), Content = stackmas };
                    framemas.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => {
                            try
                            {
                                agregarcantidad((StackLayout)framemas.Parent);
                            }
                            catch (Exception ex)
                            {
                                Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });
                    StackLayout stackprecio_cantidad = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(0, 20, 0, 0), Children = { precio, framemenos, cantidad, framemas } };

                    StackLayout stackdetalle = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(15, 20, 0, 15), Children = { tituloproducto, descripcionproducto, stackprecio_cantidad } };
                    StackLayout stackproducto = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { imagenproducto, stackdetalle } };
                    stkproductos.Children.Add(stackproducto);
                }
            }
            catch (Exception ex)
            {

            }
            

        }

        public async void agregarcantidad(StackLayout stackprecio_cantidad)
        {
            try
            {
                string idproducto = ((Label)stackprecio_cantidad.Children[0]).ClassId;
                string precio = ((Label)stackprecio_cantidad.Children[0]).Text.Split('$')[1];
                int cantidad = int.Parse(((Label)stackprecio_cantidad.Children[2]).Text);
                cantidad = cantidad + 1;
                string stringproducto = idproducto + "|" + cantidad + "|" + precio;
                for (int i = 0; i < carrito.Count; i++)
                {
                    if (carrito[i].Split('|')[0] == idproducto)
                    {
                        carrito.RemoveAt(i);
                    }
                }
                carrito.Add(stringproducto);
                ((Label)stackprecio_cantidad.Children[2]).Text = cantidad.ToString();
                actualizartotal();
            }
            catch (Exception ex)
            {

            }
        }

        public async void restarcantidad(StackLayout stackprecio_cantidad)
        {
            try
            {
                string idproducto = ((Label)stackprecio_cantidad.Children[0]).ClassId;
                string precio = ((Label)stackprecio_cantidad.Children[0]).Text.Split('$')[1];
                int cantidad = int.Parse(((Label)stackprecio_cantidad.Children[2]).Text);
                cantidad = cantidad - 1;
                string stringproducto = idproducto + "|" + cantidad + "|" + precio;
                for (int i = 0; i < carrito.Count; i++)
                {
                    if (carrito[i].Split('|')[0] == idproducto)
                    {
                        carrito.RemoveAt(i);
                    }
                }
                if (cantidad <= 0) { cantidad = 0; }
                else
                {
                    carrito.Add(stringproducto);
                }
                ((Label)stackprecio_cantidad.Children[2]).Text = cantidad.ToString();
                actualizartotal();
            }
            catch (Exception ex)
            {

            }
        }

        public async void actualizartotal()
        {
            try
            {
                int cantidadproductos = carrito.Count;
                double preciototal = 0;
                for (int i = 0; i < carrito.Count; i++)
                {
                    int cantidad = int.Parse(carrito[i].Split('|')[1]);
                    double precio = double.Parse(carrito[i].Split('|')[2]);
                    double totalproducto = double.Parse(cantidad.ToString()) * precio;
                    preciototal = preciototal + totalproducto;
                }
                LblNum.Text = cantidadproductos.ToString();
                LblTot.Text = "$" + preciototal.ToString();
            }
            catch (Exception ex)
            {

            }

        }

        public async void iracarrito()
        {
            try
            {
                
                double preciototal = 0;
                string stringproductos = "";
                for (int i = 0; i < carrito.Count; i++)
                {
                    int cantidad = int.Parse(carrito[i].Split('|')[1]);
                    double precio = double.Parse(carrito[i].Split('|')[2]);
                    double totalproducto = double.Parse(cantidad.ToString()) * precio;
                    preciototal = preciototal + totalproducto;
                    if(stringproductos == "")
                    {
                        stringproductos = carrito[i];
                    }
                    else
                    {
                        stringproductos = stringproductos+"*"+ carrito[i];
                    }
                }
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/subircarrito.php?total={0}&productos={1}&usuario={2}&pedidoant={3}", preciototal, stringproductos, Settings.Idusuario, Settings.Pedido);
                var response2 = await httpRequest(uriString2);
                if(response2 != "0" && response2 != "")
                {
                    Settings.Pedido = response2;
                    await Navigation.PushAsync(new Carrito());
                }
                
            }
            catch (Exception ex)
            {

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