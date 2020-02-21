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
    public partial class Resumen : ContentPage
    {
        string dom = "";
        string tar = "";
        string token1 = "";
        string fechaselect = "";
        string horaselect = "";
        string descuento = "";
        string oxxo = "";

        public Resumen(string tarjeta, string domicilio, string nombre, string numero)
        {
            
            Title = "Resumen";
            InitializeComponent();
            token1 = tarjeta;
            dom = domicilio;
            if(tarjeta != "OXXO")
            {
                nombretarjeta.Text = nombre;
                int inicio = numero.Length - 5;
                numerotarjeta.Text = "xxxx xxxx xxxx " + numero.Substring(inicio, 5);
                efecha.MinimumDate = DateTime.Now;
            }
            else
            {
                oxxo = "OXXO";
                nombretarjeta.Text = "OXXO PAY";
                numerotarjeta.Text = "Fecha de entrega dependerá del pago.";
                efecha.MinimumDate = DateTime.Now.AddDays(1);
            }
            
            cargardomicilio();
            cargarpedido();
            
            DateTime pickerDate = new DateTime(efecha.Date.Year, efecha.Date.Month, efecha.Date.Day);
            filtrarhoras(pickerDate);
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
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/carrito.php?&pedido={0}&rnd={1}", Settings.Pedido, num);
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
                lbltotal.Text = subtotal.ToString();
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

        public async void filtrarhoras(DateTime pickerDate)
        {
            try
            {
                DateTime febDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                
                if (pickerDate == febDate)
                {
                    phora.Items.Clear();
                    if (pickerDate.DayOfWeek.ToString() == "Saturday")
                    {
                        if (DateTime.Now.Hour < 10)
                        {
                            phora.Items.Add("11:30 a 14:00 hrs");
                        }
                    }
                    else if (pickerDate.DayOfWeek.ToString() == "Sunday")
                    {

                    }
                    else
                    {
                        if (DateTime.Now.Hour < 9)
                        {
                            phora.Items.Add("11:00 a 13:00 hrs");
                            phora.Items.Add("13:00 a 15:00 hrs");
                            phora.Items.Add("15:00 a 17:00 hrs");
                        }
                        else if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 11)
                        {
                            phora.Items.Add("13:00 a 15:00 hrs");
                            phora.Items.Add("15:00 a 17:00 hrs");
                        }
                        else if (DateTime.Now.Hour >= 11 && DateTime.Now.Hour < 13)
                        {
                            phora.Items.Add("15:00 a 17:00 hrs");
                        }
                    }

                }
                else
                {
                    phora.Items.Clear();
                    if (pickerDate.DayOfWeek.ToString() == "Saturday")
                    {
                        phora.Items.Add("9:30 a 11:30 hrs");
                        phora.Items.Add("11:30 a 14:00 hrs");
                    }
                    else if (pickerDate.DayOfWeek.ToString() == "Sunday")
                    {

                    }
                    else
                    {
                        phora.Items.Add("9:00 a 11:00 hrs");
                        phora.Items.Add("11:00 a 13:00 hrs");
                        phora.Items.Add("13:00 a 15:00 hrs");
                        phora.Items.Add("15:00 a 17:00 hrs");
                    }

                }
            }
            catch (Exception ex)
            {

            }
            
        }

        void OnDateSelected(object sender, DateChangedEventArgs args)
        {
            try
            {
                DateTime febDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime pickerDate = new DateTime(args.NewDate.Year, args.NewDate.Month, args.NewDate.Day);
                filtrarhoras(pickerDate);
                fechaselect = febDate.Date.ToLongDateString();
                TimeSpan tspan = febDate - pickerDate;

                int differenceInDays = tspan.Days;
                differenceInDays = differenceInDays * -1;
                if(differenceInDays >= 0)
                {
                    if (differenceInDays == 2)
                    {
                        lblDesc.Text = "Total (-10%): ";
                        double cantidad = double.Parse(lbltotal.Text);
                        cantidad = cantidad * 0.9;
                        lbltotal.Text = cantidad.ToString();
                        descuento = "10";
                    }
                    else if (differenceInDays >= 3)
                    {
                        lblDesc.Text = "Total (-15%): ";
                        double cantidad = double.Parse(lbltotal.Text);
                        cantidad = cantidad * 0.85;
                        lbltotal.Text = cantidad.ToString();
                        descuento = "15";
                    }
                    else
                    {
                        lblDesc.Text = "Total: ";
                        descuento = "0";
                    }
                    string differenceAsString = differenceInDays.ToString();
                }
            }
            catch (Exception ex)
            {

            }
            

        }

        public async void continuar_Clicked(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Realizando pedido");
                if(phora.SelectedItem.ToString() != "")
                {
                    horaselect = phora.SelectedItem.ToString();
                }
                if(fechaselect != "" && horaselect != "")
                {
                    if(oxxo != "")
                    {
                        string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/nordenoxxo.php?pedido={0}&usuario={1}&domicilio={2}&descuento={3}&fechaentrega={4}&horaentrega={5}", Settings.Pedido, Settings.Idusuario, dom, descuento, fechaselect, horaselect);
                        var response2 = await httpRequest(uriString2);
                        string referencia = response2.Split('|')[1];
                        string orden = response2.Split('|')[2];
                        if (referencia != "")
                        {
                            string pedido = Settings.Pedido;
                            Settings.Pedido = "";
                            UserDialogs.Instance.HideLoading();
                            await Navigation.PushAsync(new Gracias(dom, fechaselect, horaselect, referencia, pedido, orden));
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert("Algo salió mal al procesar el pago");
                        }
                    }
                    else
                    {
                        string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/norden.php?token={0}&pedido={1}&usuario={2}&domicilio={3}&descuento={4}&fechaentrega={5}&horaentrega={6}", token1, Settings.Pedido, Settings.Idusuario, dom, descuento, fechaselect, horaselect);
                        var response2 = await httpRequest(uriString2);
                        string referencia = response2.Split('|')[1];
                        if (referencia != "")
                        {
                            string pedido = Settings.Pedido;
                            Settings.Pedido = "";
                            UserDialogs.Instance.HideLoading();
                            await Navigation.PushAsync(new Gracias(dom, fechaselect, horaselect, referencia, pedido));
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert("Algo salió mal al procesar el pago");
                        }
                    }
                    
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Alert("Selecciona una fecha y hora de entrega. Horarios de entrega L.- V 9:00 - 17:00 Sabado 9:30-14:00");
                }
                
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Selecciona una fecha y hora de entrega. Horarios de entrega L.- V 9:00 - 17:00 Sabado 9:30-14:00");
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