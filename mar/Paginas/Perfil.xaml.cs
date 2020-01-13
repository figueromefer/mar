using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace mar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Perfil : ContentPage
    {
        public int clicks = 0;
        string foto = "";
        MediaFile file = null;
        Stream stream = null;

        public Perfil()
        {
            InitializeComponent();
            EnabledDisabled(false);
        }

        private void BtnEditar_Clicked(object sender, EventArgs e)
        {

            if (clicks % 2 == 0)
            {
                EnabledDisabled(true);
                btnEditar.IconImageSource = "whitechecked.png";
                //FillComponents();
                imgPerfil.GestureRecognizers.Add(new TapGestureRecognizer
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
            else
            {
                btnEditar.IconImageSource = "whiteedit.png";
                EnabledDisabled(false);
                Siguiente_Clicked();
                
            }
            clicks++;
        }

        private async void FillComponents()
        {
            try
            {
                int id = int.Parse(Settings.Idusuario);
                string uri = string.Format("http://boveda-creativa.net/laporciondelmar/usuario.php?id={0}", id);
                var response = await HttpRequest(uri);
                _ = new class_usuarios();
                class_usuarios valores = Procesar(response);

                txtNombre.Text = valores.nombre;
                txtCelular.Text = "" + valores.celular;
                txtContrasena.Text = valores.contrasena;
                //imgFoto.Source = valores.foto;
                imgFoto.Source = new UriImageSource
                {
                    Uri = new Uri(valores.foto),
                    CachingEnabled = false,
                };
                if (valores.genero == "Hombre")
                {
                    cbxHombre.IsChecked = true;
                    cbxMujer.IsChecked = false;
                }
                else
                {
                    cbxMujer.IsChecked = true;
                    cbxHombre.IsChecked = false;
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        public class_usuarios Procesar(string respuesta)
        {
            class_usuarios usuarios = new class_usuarios();
            var doc = XDocument.Parse(respuesta).Root.Elements("valor").ElementAt(0);
            if (respuesta != "0")
            {
                usuarios.id = WebUtility.UrlDecode((string)doc.Element("id"));
                usuarios.nombre = WebUtility.UrlDecode((string)doc.Element("nombre"));
                usuarios.celular = WebUtility.UrlDecode((string)doc.Element("celular"));
                usuarios.foto = WebUtility.UrlDecode((string)doc.Element("foto"));
                usuarios.correo = WebUtility.UrlDecode((string)doc.Element("correo"));
                usuarios.genero = WebUtility.UrlDecode((string)doc.Element("genero"));
                usuarios.contrasena = WebUtility.UrlDecode((string)doc.Element("contrasena"));
            }
            return usuarios;
        }

        private void LblMujer_Tapped(object sender, EventArgs e)
        {
            cbxHombre.Color = Color.FromHex("#cf7667");
            cbxHombre.Color = Color.FromHex("#3D454C");

            cbxMujer.IsChecked = true;
            cbxHombre.IsChecked = false;
        }

        private void LblHombre_Tapped(object sender, EventArgs e)
        {
            cbxHombre.Color = Color.FromHex("#cf7667");
            cbxHombre.Color = Color.FromHex("#3D454C");

            cbxHombre.IsChecked = true;
            cbxMujer.IsChecked = false;
        }

        private String GetFormatDate(string date)
        {
            string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            string[] fecha = date.Split('-');
            string fecha_completa = meses[int.Parse(fecha[0])] + ", " + meses[2] + " de ";
            return fecha_completa;
        }

        private void EnabledDisabled(Boolean estado)
        {
            txtNombre.IsEnabled = estado;
            txtCelular.IsEnabled = estado;
            txtContrasena.IsEnabled = estado;
            cbxHombre.IsEnabled = estado;
            cbxMujer.IsEnabled = estado;
            lblHombre.IsEnabled = estado;
            lblMujer.IsEnabled = estado;
            imgPerfil.IsEnabled = estado;

            if (estado)
            {
                txtNombre.TextColor = Color.FromHex("#979797");
                txtCelular.TextColor = Color.FromHex("#979797");
                txtContrasena.TextColor = Color.FromHex("#979797");

                lblHombre.TextColor = Color.FromHex("#3D454C");
                lblMujer.TextColor = Color.FromHex("#3D454C");
            }
            else
            {
                txtNombre.TextColor = Color.FromHex("#000");
                txtCelular.TextColor = Color.FromHex("#000");
                txtContrasena.TextColor = Color.FromHex("#000");

                lblHombre.TextColor = Color.FromHex("#979797");
                lblMujer.TextColor = Color.FromHex("#979797");
                FillComponents();
            }
        }

        private void CbxHombre_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbxHombre.IsChecked)
            {
                cbxMujer.IsChecked = false;
            }
        }

        private void CbxMujer_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbxMujer.IsChecked)
            {
                cbxHombre.IsChecked = false;
            }
        }

        public async Task<string> HttpRequest(string url)
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

        /*  -------------------------- Code Editar Usuario -----------------------------------  */

        public async void Siguiente_Clicked()
        {
            string vali = "1";
            string contrasena = txtContrasena.Text;
            string nombre = txtNombre.Text;
            string celular = txtCelular.Text;
            string genero = cbxHombre.IsChecked ? "Hombre" : "Mujer";
            imgPerfil.GestureRecognizers.RemoveAt(0);

            if (vali == "1")
            {
                string uriString2 = string.Format("http://boveda-creativa.net/laporciondelmar/actualizar.php?nombre={0}&contrasena={1}&celular={2}&genero={3}&foto={4}&id={5}", nombre, contrasena, celular, genero, foto, Settings.Idusuario);
                var response2 = await HttpRequest(uriString2);
                if (response2 != "" && response2 != "0")
                {
                    await DisplayAlert("Completado", "Se actualizaron los datos del usuario", "OK");
                    FillComponents();
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
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                CompressionQuality = 45

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
                btnEditar.IsEnabled = true;
                imgFoto.Source = ImageSource.FromStream(() =>
                {
                    stream = file.GetStreamWithImageRotatedForExternalStorage();
                    return stream;
                });
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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Tomar_foto_perfil();
        }
    }
}
