using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using AndroidX.Core.Graphics.Drawable;

namespace AppListadoClientesAzure
{
    [Activity(Label = "DataDetail")]
    public class DataDetailActivity : Activity, IOnMapReadyCallback
    {
        TextView txtNombre, txtDomcilio, txtCorreo, txtSaldo, txtEdad;
        ImageView Imagen, ImagenFondo;
        GoogleMap googleMap;
        string correo, imagen, nombre, domicilio, imagenfondo;
        double saldo, lat, lon;
        int edad;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DataDetail1);
            try
            {

                correo = Intent.GetStringExtra("correo");
                imagen = Intent.GetStringExtra("imagen");
                imagenfondo = Intent.GetStringExtra("imagenfondo");
                domicilio = Intent.GetStringExtra("domicilio");
                nombre = Intent.GetStringExtra("nombre");
                edad = int.Parse(Intent.GetStringExtra("edad"));
                saldo = double.Parse(Intent.GetStringExtra("saldo"));
                lat = double.Parse(Intent.GetStringExtra("latitud"));
                lon = double.Parse(Intent.GetStringExtra("longitud"));
                ImagenFondo = FindViewById<ImageView>(Resource.Id.imageback);
                Imagen = FindViewById<ImageView>(Resource.Id.image);
                txtNombre = FindViewById<TextView>(Resource.Id.txtname);
                txtDomcilio = FindViewById<TextView>(Resource.Id.txtaddress);
                txtCorreo = FindViewById<TextView>(Resource.Id.txtmail);
                txtSaldo = FindViewById<TextView>(Resource.Id.txtrevenues);
                txtEdad = FindViewById<TextView>(Resource.Id.txtage);

                txtCorreo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                txtDomcilio.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                txtCorreo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                txtSaldo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                txtEdad.SetTextColor(Android.Graphics.Color.WhiteSmoke);

                txtNombre.Text = nombre;
                txtDomcilio.Text = domicilio;
                txtCorreo.Text = correo;
                txtEdad.Text = edad.ToString();
                txtSaldo.Text = saldo.ToString();
                var typeface = Typeface.CreateFromAsset(this.Assets, "fonts/V64BoldItalic.ttf");
                txtNombre.SetTypeface(typeface, TypefaceStyle.Normal);
                var RutaImagen = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                    imagen);
                var RutaImagenFondo = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                    imagenfondo);
                var rutauriimagen = Android.Net.Uri.Parse(RutaImagen);
                var rutauriimagenfondo = Android.Net.Uri.Parse(RutaImagenFondo);
                Imagen.SetImageURI(rutauriimagen);
                ImagenFondo.SetImageURI(rutauriimagenfondo);

                var opciones = new BitmapFactory.Options();
                opciones.InPreferredConfig = Bitmap.Config.Argb8888;
                var bitmap = BitmapFactory.DecodeFile(RutaImagen, opciones);
                Imagen.SetImageDrawable(getRoundedCornerImage(bitmap, 20));

                var mapview = FindViewById<MapView>(Resource.Id.mapView);
                mapview.OnCreate(savedInstanceState);
                mapview.GetMapAsync(this);
                MapsInitializer.Initialize(this);

            }
            catch (System.Exception ex)
            {
                throw;
            }
           
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            this.googleMap = googleMap;
            var builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(lat, lon));
            builder.Zoom(17);
            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            this.googleMap.AnimateCamera(cameraUpdate);
        }
        public static RoundedBitmapDrawable
           getRoundedCornerImage(Bitmap image, int cornerRadius)
        {
            var corner = RoundedBitmapDrawableFactory.Create(null, image);
            corner.CornerRadius = cornerRadius;
            return corner;
        }


    }
}