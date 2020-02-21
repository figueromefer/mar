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
    public partial class NewDomicilio : ContentPage
    {
        public NewDomicilio()
        {
            InitializeComponent();
            cargarcp();
        }

        public async void cargarcp()
        {
            colonia.Items.Clear();
            try
            {
                string uriString2 = "http://boveda-creativa.net/laporciondelmar/cp.php";
                var response2 = await httpRequest(uriString2);
                List<class_unico> valor = new List<class_unico>();
                valor = procesarunico(response2);
                for (int i = 0; i < valor.Count(); i++)
                {
                    colonia.Items.Add(valor.ElementAt(i).cadena);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<class_unico> procesarunico(string respuesta)
        {
            List<class_unico> items = new List<class_unico>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_unico
                             {
                                 cadena = WebUtility.UrlDecode((string)r.Element("cp")),
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

        async void cp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cp.Items.Clear();
            try
            {
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/colonias.php?&cp={0}", colonia.SelectedItem.ToString());
                var response2 = await httpRequest(uriString2);
                List<class_unico> valor = new List<class_unico>();
                valor = procesarunico(response2);
                for (int i = 0; i < valor.Count(); i++)
                {
                    cp.Items.Add(valor.ElementAt(i).cadena);
                }
            }
            catch (Exception ex)
            {

            }
        }

        async void Guardar_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                int continuar = 1;
                string domicilio = entrydomicilio.Text;
                string numero = entrynumero.Text;
                string codigo = cp.SelectedItem.ToString();
                string col = colonia.SelectedItem.ToString();
                if(domicilio.Length < 3)
                {
                    continuar = 0;
                    UserDialogs.Instance.Alert("Domicilio incompleto");
                }
                if (numero.Length < 3)
                {
                    continuar = 0;
                    UserDialogs.Instance.Alert("Numero incompleto");
                }
                if(continuar == 1)
                {
                    string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/ndomicilio.php?&usuario={0}&domicilio={1}&numero={2}&codigo={3}&colonia={4}", Settings.Idusuario, domicilio, numero, codigo, col);
                    var response2 = await httpRequest(uriString2);
                    UserDialogs.Instance.Alert("Dirección registrada correctamente");
                    await Navigation.PopAsync();
                }
                
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Código postal o colonia no definida");
            }
        }
    }
}
