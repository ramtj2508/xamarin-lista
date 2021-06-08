using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Graphics.Drawable;
using System.Collections.Generic;

namespace AppListadoClientesAzure
{
    public class DataAdapter : BaseAdapter<ElementosdelaTabla>
    {
        List<ElementosdelaTabla> items;
        Activity context;


        public DataAdapter(Activity context, List<ElementosdelaTabla> items) : base()
        {
            this.context = context;
            this.items = items;
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override ElementosdelaTabla this[int position]
        {
            get { return items[position]; }
        }


        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            view = context.LayoutInflater.Inflate(Resource.Layout.DataRow, null);
            view.FindViewById<TextView>(Resource.Id.txtcorreo).Text = item.Correo;
            var txtHead = view.FindViewById<TextView>(Resource.Id.txtnombre);
            txtHead.Text = item.Nombre;
            var typeface = Typeface.CreateFromAsset(context.Assets, "fonts/V64BoldItalic.ttf");  
            txtHead.SetTypeface(typeface, TypefaceStyle.Normal);
            var path = System.IO.Path.Combine(System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal), item.Imagen);
            var pathback = System.IO.Path.Combine(System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal), item.ImagenFondo);          
            var Image = BitmapFactory.DecodeFile(path);
            var ImageBack = BitmapFactory.DecodeFile(pathback);
            Image = ResizeBitmap(Image, 100, 100);
            ImageBack = ResizeBitmap(ImageBack, 350, 200);
            view.FindViewById<ImageView>(Resource.Id.imagen).
                SetImageDrawable(getRoundedCornerImage(Image, 5));
            view.FindViewById<ImageView>(Resource.Id.imagenfondo).
                SetImageDrawable(getRoundedCornerImage(ImageBack, 2));
            return view;

        }

        public static RoundedBitmapDrawable 
            getRoundedCornerImage(Bitmap image, int cornerRadius)
        {
            var corner = RoundedBitmapDrawableFactory.Create(null, image);
            corner.CornerRadius = cornerRadius;
            return corner;
        }

        private Bitmap ResizeBitmap(Bitmap imagenoriginal, int withimagenoriginal,
            int heightimagenoriginal)
        {
            Bitmap rezisedImage = Bitmap.CreateBitmap(withimagenoriginal,
                heightimagenoriginal, Bitmap.Config.Argb8888);
            float Width = imagenoriginal.Width;
            float Height = imagenoriginal.Height;
            var canvas = new Canvas(rezisedImage);
            var scala = withimagenoriginal / Width;
            var xTranslation = 0.0f;
            var yTanslation = (heightimagenoriginal - Height * scala) / 2.0f;
            var transformacion = new Matrix();
            transformacion.PostTranslate(xTranslation, yTanslation);
            transformacion.PreScale(scala, scala);
            var paint = new Paint();
            paint.FilterBitmap = true;
            canvas.DrawBitmap(imagenoriginal, transformacion, paint);
            return rezisedImage;
        }


    }


}