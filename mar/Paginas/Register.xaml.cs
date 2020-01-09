using System;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Net.Http.Headers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ImageCircle.Forms.Plugin.Abstractions;

namespace mar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        MediaFile file = null;
        Stream stream = null;
        string foto = "";
        CircleImage perfil = new CircleImage() { WidthRequest = 80, HeightRequest = 80, Source = ImageSource.FromFile("userperfil.png")};

        public Register()
        {
            InitializeComponent();
            perfil.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    try
                    {
                        Tomar_foto_perfil();
                    }
                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
            cargarfoto.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    try
                    {
                        Tomar_foto_perfil();
                    }
                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbxMujer.IsChecked)
            {
                cbxMujer.IsChecked = true;
                cbxHombre.IsChecked = false;
            } 
            
            if (cbxHombre.IsChecked)
            {
                cbxMujer.IsChecked = false;
                cbxHombre.IsChecked = true;
            }
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            cbxMujer.IsChecked = true;
            cbxHombre.IsChecked = false;
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            cbxMujer.IsChecked = false;
            cbxHombre.IsChecked = true;
        }

        public async void Siguiente_Clicked(object sender, System.EventArgs e)
        {
            string vali = "1";
            string contrasena = txtContrasena.Text;
            string nombre = txtNombre.Text;
            string mail = txtMail.Text;
            string celular = txtCelular.Text;
            if(nombre.Length < 2)
            {
                vali = "0";
                await DisplayAlert("ALERTA", "Nombre incompleto, favor de validar.", "OK");
            }
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
            if (celular.Length < 2)
            {
                vali = "0";
                await DisplayAlert("ALERTA", "Celular incompleto, favor de validar.", "OK");
            }
            string genero = "Mujer";
            if(cbxMujer.IsChecked || cbxHombre.IsChecked)
            {
                if (cbxHombre.IsChecked)
                {
                    genero = "Hombre";
                }
            }
            else
            {
                vali = "0";
                await DisplayAlert("ALERTA", "Genero no especificado.", "OK");
            }
            if(foto == "")
            {
                vali = "0";
                await DisplayAlert("ALERTA", "Foto de perfil no definida o en proceso de carga.", "OK");
            }
            if(vali == "1")
            {
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/registro.php?nombre={0}&contrasena={1}&correo={2}&celular={3}&genero={4}&foto={5}", nombre, contrasena, mail, celular, genero, foto);
                var response2 = await httpRequest(uriString2);
                if (response2 != "" && response2 != "0")
                {
                    Settings.Idusuario = response2; 
                    Application.Current.MainPage = new NavigationPage(new RootPage());
                }

            }
        }

        #region "tomar foto perfil"
        public async void Tomar_foto_perfil()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Sin camara", "No hay una camara disponible.", "OK");
                return;
            }

            file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });
            if (file == null)
                return;
            subir_perfil();

        }

        public async void subir_perfil()
        {
            try
            {
                string now1 = DateTime.Now.ToString().Replace(' ', '_').Replace('/', '_').Replace(':', '_');
                if (file == null) { await DisplayAlert("Alerta", "Falta definir una foto.", "OK"); }
                await Upload(file, "_foto_" + now1 + ".jpg");

                var foto1 = "_foto_" + now1 + ".jpg";
                foto1 = WebUtility.UrlEncode(foto1);
                foto = foto1;
                btnNext.IsEnabled = true;
                perfil.Source = ImageSource.FromStream(() =>
                {
                    stream = file.GetStreamWithImageRotatedForExternalStorage();
                    return stream;
                });
                StackFoto.Children.RemoveAt(1);
                StackFoto.Children.Add(perfil);
                file.Dispose();

            }
            catch (Exception ex)
            {

            }
        }



        public async Task<bool> Upload(MediaFile mediaFile, string filename)
        {
            try
            {
                byte[] bitmapData;
                var stream = new MemoryStream();
                mediaFile.GetStreamWithImageRotatedForExternalStorage().CopyTo(stream);
                bitmapData = stream.ToArray();
                var fileContent = new ByteArrayContent(bitmapData);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "fileUpload",
                    FileName = filename
                };

                string boundary = "---8393774hhy37373773";
                MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
                multipartContent.Add(fileContent);


                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync("http://boveda-creativa.net/laporciondelmar/subir.php", multipartContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

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