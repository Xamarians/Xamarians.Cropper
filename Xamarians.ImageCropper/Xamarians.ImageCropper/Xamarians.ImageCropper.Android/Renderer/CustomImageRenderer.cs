using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Threading.Tasks;
using Android.Graphics;
using System.ComponentModel;
using Xamarians.ImageCropper.Controls;
using Xamarians.ImageCropper.Droid.Renderer;

[assembly: ExportRenderer(typeof(CustomImage), typeof(CustomImageRenderer))]

namespace Xamarians.ImageCropper.Droid.Renderer
{
    public class CustomImageRenderer : ImageRenderer
        {
        CustomImage element;
        int width, height;
        bool _isLoaded = false;
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            element = (CustomImage)Element;
            if (Control == null)
                return;
            LoadImageAsync();           
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (!_isLoaded && CustomImage.ImageSourceProperty.PropertyName.Equals(e.PropertyName))
            {
                LoadImageAsync();
            }
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
           
        }

        private async void LoadImageAsync()
        {
            if (string.IsNullOrWhiteSpace(element.ImageSource))
                return;

            var Width = Resources.DisplayMetrics.WidthPixels;
            var Height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;
          
            width =  (int)(Width / density);
            height = (int)(Height / density);

            _isLoaded = true;
            if (element.IsResourceFile)
            {
                int resourceId = DS.BitmapManager.GetDrawableResourceId(element.ImageSource);
                if (resourceId == 0)
                    return;
                
                using (var bitmap = DS.BitmapManager.ResizeImage(resourceId, width, height, inMemoryOnly: true))
                {
                    Control.SetImageBitmap(bitmap);
                }
                Control.Invalidate();
            }
            else
            {
                BitmapFactory.Options options = await DS.BitmapManager.GetBitmapOptionsOfImage(element.ImageSource);
                Bitmap bitmapToDisplay = await DS.BitmapManager.LoadScaledDownBitmapForDisplayAsync(element.ImageSource, options, width, height);
                Control.SetImageBitmap(bitmapToDisplay);
            }
        }

        static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }

            }
            return (int)inSampleSize;
        }
        public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(int resourceId, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeResourceAsync(Resources, resourceId, options);
        }   

        public async Task<BitmapFactory.Options> GetBitmapOptionsOfImage(int resourceId)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.
            Bitmap result = await BitmapFactory.DecodeResourceAsync(Resources, resourceId, options);
            return options;
        }

    }

}