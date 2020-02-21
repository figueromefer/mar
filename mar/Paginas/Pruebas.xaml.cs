using System;
using Xamarin.Forms;
using Conekta.Xamarin;
using System.IO;
using System.Net;
using System.Threading.Tasks;


namespace mar
{
    public partial class Pruebas : ContentPage
    {
        public Pruebas()
        {
            InitializeComponent();
            pruebas1();
        }

        public async void pruebas1()
        {
            try
            {
                
                string token = "";
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        token = await new ConektaTokenizer("key_EU3RzXHR7T279WQsq7LN7ig", RuntimePlatform.iOS).GetTokenAsync("4242424242424242", "Fernando", "123", int.Parse("2021"), int.Parse("11"));
                        break;
                    case Device.Android:
                        token = await new ConektaTokenizer("key_EU3RzXHR7T279WQsq7LN7ig", RuntimePlatform.Android).GetTokenAsync("4242424242424242", "Fernando", "123", int.Parse("2021"), int.Parse("11"));
                        break;
                }
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/norden.php?token={0}", token);
                var response2 = await httpRequest(uriString2);

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
    }
}
