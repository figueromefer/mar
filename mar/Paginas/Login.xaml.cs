using System;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
                
            btnRecovery.Clicked += BtnRecovery_Clicked;
        }

        private void BtnRecovery_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new RecoveryPass());
        }

        async void BtnLogin_Clicked(object sender, System.EventArgs e)
        {
            string vali = "1";
            string contrasena = txtPass.Text;
            string mail = txtMail.Text;
            if (contrasena.Length < 2)
            {
                vali = "0";
                await DisplayAlert("ALERTA", "Contraseña incompleta, favor de validar.", "OK");
            }
            if (mail.Length < 2)
            {
                vali = "0";
                await DisplayAlert("ALERTA", "Correo incompleto, favor de validar.", "OK");
            }
            if (vali == "1")
            {
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/login.php?correo={0}&contrasena={1}", mail, contrasena);
                var response2 = await httpRequest(uriString2);
                if (response2 != "" && response2 != "0")
                {
                    Settings.Idusuario = response2;
                    Application.Current.MainPage = new NavigationPage(new RootPage());
                }
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

        async void Facebook_ClickedAsync(object sender, System.EventArgs e)
        {
            await DisplayAlert("Facebook", "En desarrollo... está opción estará disponible pronto.", "OK");
        }
    }
}