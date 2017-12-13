using Xamarin.Forms;

namespace Xamarians.ImageCropper.Controls
{
    public class CustomImage : Image
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(string), typeof(CustomImage), default(string));
      
        public string ImageSource
        {
            get { return GetValue(ImageSourceProperty) as string; }
            set { SetValue(ImageSourceProperty, value); }
        }
        public bool IsResourceFile { get; set; } = false;

        public CustomImage()
        {
        }       
    }

    public class InputImage
    {     
        public double CropWidth { get; set; }
        public double CropHeight { get; set; }
        public double CropX { get; set; }
        public double CropY { get; set; }
        public double ImageWidth { get; set; }
        public double ImageHeight { get; set; }
        public double ImageScale { get; set; }
        public string ImageSource { get; set; }

        public InputImage() { }       
    }
}