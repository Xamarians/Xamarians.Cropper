using System;
using UIKit;
using Xamarin.Forms;
using CoreGraphics;
using Xamarians.ImageCropper.iOS.DS;
using Xamarians.ImageCropper.Controls;
using Xamarians.ImageCropper.iOS.Renderer;
using System.Threading.Tasks;
using System.Runtime.Remoting.Contexts;

[assembly: Dependency(typeof(ImageCropper))]

namespace Xamarians.ImageCropper.iOS.DS
{
    class ImageCropper:IImageCropper
    {
        static Context _context;

        public ImageCropper()
        {

        }
        public static void Initialize(Context context)
        {
            _context = context;
            ImageCropperService.Init(new ImageCropper());
        }
        public Task<string> CropImage(InputImage inputImages)
        {
            var tcs = new TaskCompletionSource<string>();

            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (!System.IO.Directory.Exists(documentsDirectory))
            {
                System.IO.Directory.CreateDirectory(documentsDirectory);
            }

            var finalImage = WriteImage(inputImages);
            var filename = System.IO.Path.Combine(documentsDirectory, $"image1.jpg");
            finalImage.AsPNG().Save(filename, false);
            tcs.SetResult(filename);
            return tcs.Task;
        }

        private UIImage WriteImage(InputImage input)
        {
            var image = UIImage.FromBundle(input.ImageSource);
            image = ScaleImageInRatio(image, (nfloat)input.ImageWidth, (nfloat)input.ImageHeight);

            double rw = image.Size.Width / (input.ImageWidth * input.ImageScale);
            double rh = image.Size.Height / (input.ImageHeight * input.ImageScale);

            double cropX = input.CropX * rw;
            double cropY = input.CropY * rh;
            double cropW = input.CropWidth * rw;
            double cropH = input.CropHeight * rh;

            var cgImage = image.CGImage.WithImageInRect(new CGRect((nfloat)cropX, (nfloat)cropY, (nfloat)(cropW), (nfloat)(cropH)));
            image = new UIImage(cgImage);
            return image;
        }

        private static UIImage ScaleImageInRatio(UIImage image, nfloat reqWidth, nfloat reqHeight)
        {
            var width = image.Size.Width;
            var height = image.Size.Height;
           CustomImageRenderer.GetRatio(width, height, ref reqWidth, ref reqHeight);

            image = image.Scale(new CGSize(reqWidth, reqHeight));
            return image;
        }        
    }
}