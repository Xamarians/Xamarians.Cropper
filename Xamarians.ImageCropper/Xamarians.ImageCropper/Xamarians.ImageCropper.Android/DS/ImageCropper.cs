using Xamarin.Forms;
using Android.Graphics;
using System.IO;
using static Android.Graphics.Bitmap;
using System.Threading.Tasks;
using Xamarians.ImageCropper.Controls;
using Android.Content;
using System;

[assembly: Dependency(typeof(Xamarians.ImageCropper.Droid.DS.ImageCropper))]

namespace Xamarians.ImageCropper.Droid.DS
{
    public class ImageCropper : IImageCropper
    {
        static Context _context;
        public Bitmap _bitmap;
        public ImageCropper()
        {

        }
        //public static void Initialize(Context context)
        //{
        //    _context = context;
        //    ImageCropperService.Init(new ImageCropper());
        //}
        //public Task<string> CropImage(string Image, int Width, int Height)
        //{
        //    var tcs = new TaskCompletionSource<string>();
        //    var result = GetBitmap(Image);
        //    Bitmap bitmap = CreateScaledBitmap(result, Width, Height, true);
        //    var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
        //    var filename = System.IO.Path.Combine(dir.Path, $"image.jpg");
        //    using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
        //    {
        //        bitmap.Compress(CompressFormat.Jpeg, 100, fs);
        //        bitmap.Recycle();
        //        bitmap.Dispose();
        //    }
        //    tcs.SetResult(filename);
        //    return tcs.Task;
        //}
        public Task<string> CropImage(InputImage inputImage)
        {
            var tcs = new TaskCompletionSource<string>();
            var result = WriteImage(inputImage);
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            var filename = System.IO.Path.Combine(dir.Path, $"image.jpg");
            using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                result.Compress(CompressFormat.Jpeg, 100, fs);
                result.Recycle();
                result.Dispose();
            }         
            tcs.SetResult(filename);
            return tcs.Task;
        }    
        private Bitmap WriteImage(InputImage input)
        {
            try
            {
                var bitmap = GetBitmap(input.ImageSource);
                double rw = bitmap.Width / (input.ImageWidth * input.ImageScale);
                double rh = bitmap.Height / (input.ImageHeight * input.ImageScale);
                double cropX = input.CropX * rw;
                double cropY = input.CropY * rh;
                double cropW = input.CropWidth * rw;
                double cropH = input.CropHeight * rh;
                return CreateBitmap(bitmap, (int)cropX, (int)cropY, (int)cropW, (int)cropH);
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                _bitmap.Recycle();
                _bitmap.Dispose();
            }
            return null;
        }

        private Android.Net.Uri getImageUri(String path)
        {
            return Android.Net.Uri.FromFile(new Java.IO.File(path));
        }
        private Bitmap GetBitmap(string path)
        {
            var uri = getImageUri(path);              
            Stream ins = null;
            try
            {
                ins = Android.App.Application.Context.ContentResolver.OpenInputStream(uri);
                // Decode image size
                BitmapFactory.Options o = new BitmapFactory.Options
                {
                    InJustDecodeBounds = true
                };
                BitmapFactory.DecodeStream(ins, null, o);
                ins.Close();
                int scale = 1;
                BitmapFactory.Options o2 = new BitmapFactory.Options
                {
                    InSampleSize = scale
                };
                ins = Android.App.Application.Context.ContentResolver.OpenInputStream(uri);
                _bitmap = BitmapFactory.DecodeStream(ins, null, o2);
                ins.Close();
                return _bitmap;              
            }
            catch (Exception e)
            {                
                throw new Exception(e.Message, e);
            }     
        }
    }
}