using Android.Graphics;
using System.Threading.Tasks;

namespace Xamarians.ImageCropper.Droid.DS
{
    internal static class BitmapManager
    {
        public static int GetDrawableResourceId(string name)
        {
            try
            {
                if (name.IndexOf(".") > 0)
                    name = name.Substring(0, name.LastIndexOf("."));
                return (int)typeof(Resource.Drawable).GetField(name).GetValue(null);
            }
            catch
            {
                return 0;
            }
        }
        public static Bitmap ResizeImage(int resId, int reqWidth, int reqHeight, bool inMemoryOnly = false)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                // by setting this field as true, the actual bitmap pixels are not loaded in the memory. Just the bounds are loaded. If
                // you try the use the bitmap here, you will get null.
                InJustDecodeBounds = true
            };
            Bitmap bmp = BitmapFactory.DecodeResource(Xamarin.Forms.Forms.Context.Resources, resId, options);
            int imageHeight = options.OutHeight;
            int imageWidth = options.OutWidth;

            GetRatio(imageWidth, imageHeight, ref reqWidth, ref reqHeight);

            // setting inSampleSize value allows to load a scaled down version of the original image
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);
            // inJustDecodeBounds set to false to load the actual bitmap
            options.InJustDecodeBounds = false;
            // this options allow android to claim the bitmap memory if it runs low on memory
            options.InPurgeable = true;
            options.InInputShareable = true;
            options.InTempStorage = new byte[16 * 1024];

            try
            {
                // load the bitmap from its path
                bmp = BitmapFactory.DecodeResource(Xamarin.Forms.Forms.Context.Resources, resId, options);
            }
            catch (Java.Lang.OutOfMemoryError exception)
            {
                exception.PrintStackTrace();
            }
            if (inMemoryOnly)
                return bmp;
            try
            {
                bmp = Bitmap.CreateScaledBitmap(bmp, reqWidth, reqHeight, false);
            }
            catch (Java.Lang.OutOfMemoryError exception)
            {
                exception.PrintStackTrace();
            }
            return bmp;
        }

        public static void GetImageSize(int resId, out int width, out int height)
        {
            BitmapFactory.Options options = new BitmapFactory.Options();
            // by setting this field as true, the actual bitmap pixels are not loaded in the memory. Just the bounds are loaded. If
            // you try the use the bitmap here, you will get null.
            options.InJustDecodeBounds = true;
            Bitmap bmp = BitmapFactory.DecodeResource(Xamarin.Forms.Forms.Context.Resources, resId, options);
            height = options.OutHeight;
            width = options.OutWidth;
        }

        public static void GetRatio(int actualWidth, int actualHeight, ref int reqWidth, ref int reqHeight)
        {
            if (reqWidth > reqHeight)
            {
                if (reqWidth > actualWidth)
                    reqWidth = actualWidth;
                reqHeight = reqWidth * actualHeight / actualWidth;
            }
            else
            {
                if (reqHeight > actualHeight)
                    reqHeight = actualHeight;
                reqWidth = reqHeight * actualWidth / actualHeight;
            }
        }

        #region private methods
        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
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
        public static async Task<BitmapFactory.Options> GetBitmapOptionsOfImage(string filePath)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.
            Bitmap result = await BitmapFactory.DecodeFileAsync(filePath, options);
            return options;
        }
        public static async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(string filePath, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);
            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;
            return await BitmapFactory.DecodeFileAsync(filePath, options);
        }
        #endregion
    }
}