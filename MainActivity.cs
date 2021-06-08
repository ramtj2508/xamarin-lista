using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Linq;
using Android.Content;
using System.Threading.Tasks;
using Android.Graphics;


namespace AppListadoClientesAzure
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Android.App.ProgressDialog progress;
        string elementoimagen, elementoimagenfondo;
        ListView listado;
        List<Clientes> ListadodeClientes = new List<Clientes>();
        List<ElementosdelaTabla> ElementosTabla = new List<ElementosdelaTabla>();
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            SupportActionBar.Hide();
            listado = FindViewById<ListView>(Resource.Id.lista);
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Cargando datos de Azure ...");
            progress.SetCancelable(false);
            progress.Show();
            await CargarDatosAzure();
            progress.Hide();


        }

        public async Task CargarDatosAzure()
        {
            try
            {
                var typeface = Typeface.CreateFromAsset(this.Assets, "fonts/V64BoldItalic.ttf");
                var Titulo = FindViewById<TextView>(Resource.Id.titulo);
                Titulo.SetTypeface(typeface, TypefaceStyle.Italic);
                var CuentadeAlmacenamiento = CloudStorageAccount.Parse
                     ("DefaultEndpointsProtocol=https;AccountName=programacionmoviles;AccountKey=K4HLGMkMGB87LlncsykIQe5QO85Ges6DDZ1wjK8M7EFpZeR+k+7fKLm3uy3th+R6mvmYeDa6pf2sn62Q3dZkWg==;EndpointSuffix=core.windows.net");
                var ClienteBlob = CuentadeAlmacenamiento.CreateCloudBlobClient();
                var Contenedor = ClienteBlob.GetContainerReference("devs");
                var TablaNoSQL = CuentadeAlmacenamiento.CreateCloudTableClient();
                var Tabla = TablaNoSQL.GetTableReference("devs");
                var Consulta = new TableQuery<Clientes>();
                TableContinuationToken token = null;
                var Datos = await Tabla.ExecuteQuerySegmentedAsync<Clientes>
                    (Consulta, token, null, null);
                ListadodeClientes.AddRange(Datos.Results);
                int iCorreo = 0;
                int iNombre = 0;
                int iImagen = 0;
                int iEdad = 0;
                int iDomicilio = 0;
                int iSaldo = 0;
                int iLatitud = 0;
                int iLongitud = 0;
                int iImagenFondo = 0;
                ElementosTabla = ListadodeClientes.Select(r => new ElementosdelaTabla()
                {
                    Correo = ListadodeClientes.ElementAt(iCorreo++).Correo,
                    Nombre = ListadodeClientes.ElementAt(iNombre++).RowKey,
                    Imagen = ListadodeClientes.ElementAt(iImagen++).Imagen,
                    Domicilio = ListadodeClientes.ElementAt(iDomicilio++).Domicilio,
                    Edad = ListadodeClientes.ElementAt(iEdad++).Edad,
                    Saldo = ListadodeClientes.ElementAt(iSaldo++).Saldo,
                    Longitud = ListadodeClientes.ElementAt(iLongitud++).Longitud,
                    Latitud = ListadodeClientes.ElementAt(iLatitud++).Latitud,
                    ImagenFondo = ListadodeClientes.ElementAt(iImagenFondo++).ImagenFondo,
                }).ToList();

                int contadorimagen = 0;
                while (contadorimagen < ListadodeClientes.Count)
                {
                    elementoimagen = ListadodeClientes.ElementAt(contadorimagen).Imagen;
                    elementoimagenfondo = ListadodeClientes.ElementAt(contadorimagen).ImagenFondo;
                    var ImagenBlob = Contenedor.GetBlockBlobReference(elementoimagen);
                    var ImagenFondoBlob = Contenedor.GetBlockBlobReference(elementoimagenfondo);
                    var rutaimagen = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    var rutaimagenfondo = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    var ArchivoImagen = System.IO.Path.Combine(rutaimagen, elementoimagen);
                    var ArchivoImagenFondo = System.IO.Path.Combine(rutaimagenfondo, elementoimagenfondo);
                    var StreamImagen = File.OpenWrite(ArchivoImagen);
                    var StreamImagenFondo = File.OpenWrite(ArchivoImagenFondo);
                    await ImagenBlob.DownloadToStreamAsync(StreamImagen);
                    await ImagenFondoBlob.DownloadToStreamAsync(StreamImagenFondo);
                    contadorimagen++;

                }
                Toast.MakeText(this, "Imágenes descargadas", ToastLength.Long).Show();
                listado.Adapter = new DataAdapter(this, ElementosTabla);
                listado.ItemClick += OnListItemClick;

            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
           
        }
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var DataSend = ElementosTabla[e.Position];
            var DataIntent = new Intent(this, typeof(DataDetailActivity));
            DataIntent.PutExtra("correo", DataSend.Correo);
            DataIntent.PutExtra("imagen", DataSend.Imagen);
            DataIntent.PutExtra("imagenfondo", DataSend.ImagenFondo);
            DataIntent.PutExtra("nombre", DataSend.Nombre);
            DataIntent.PutExtra("edad", DataSend.Edad.ToString());
            DataIntent.PutExtra("saldo", DataSend.Saldo.ToString());
            DataIntent.PutExtra("domicilio", DataSend.Domicilio);
            DataIntent.PutExtra("latitud", DataSend.Latitud.ToString());
            DataIntent.PutExtra("longitud", DataSend.Longitud.ToString());
            StartActivity(DataIntent);
        }
    }
    
    public class ElementosdelaTabla
    {



        public string Domicilio { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public string ImagenFondo { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Correo { get; set; }
        public double Saldo { get; set; }
        public int Edad { get; set; }
    }

    public class Clientes : TableEntity
    {

        public Clientes(string Categoria, string Nombre)
        {
            PartitionKey = Categoria;
            RowKey = Nombre;
        }
        public Clientes() { }
        public string Domicilio { get; set; }
        public string Imagen { get; set; }
        public string ImagenFondo { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Correo { get; set; }
        public double Saldo { get; set; }
        public int Edad { get; set; }
    }


}