using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Xamarians.ImageCropper.Controls
{
    public class CropperLayout : AbsoluteLayout
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(string), typeof(CropperLayout), null);

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }       

        public CustomImage customImage;
        public static InputImage inputImage = new InputImage();
        public PanContainer panContainer;
        public CropperLayout()
        {
            customImage = new CustomImage()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            panContainer = new PanContainer();           
            SetLayoutBounds(customImage, new Rectangle(0,0,1,1));
            SetLayoutFlags(customImage, AbsoluteLayoutFlags.All);            
            SetLayoutBounds(panContainer, new Rectangle(0, 0, 1, 1));
            SetLayoutFlags(panContainer, AbsoluteLayoutFlags.All);
            IsClippedToBounds = true;
            Children.Add(customImage);
            Children.Add(panContainer);

        }     
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width > height)
            {
                panContainer.CropperContainer.TranslationX = 0;
                panContainer.CropperContainer.TranslationY = 0;
                UpdateChildrenLayout();
            }
            inputImage.CropX = customImage.X;
            inputImage.CropY = customImage.Y;
            inputImage.ImageWidth = customImage.Width;
            inputImage.ImageHeight = customImage.Height;  
            
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (ImageSourceProperty.PropertyName.Equals(propertyName))
            {
                customImage.Source = this.ImageSource;
            }
        }
    }

    public class CropperView :Grid
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(string), typeof(CropperView), null);

        public static event EventHandler<string> OnImageCropped;
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        CropperLayout Cropperlayout = new CropperLayout();
        public CropperView()
        {
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            var cropButton = new Button
            {
                HeightRequest = 60,
               Text="Crop",
               TextColor = Color.White,
               BackgroundColor = Color.Black,
               BorderRadius = 0,
            };
            cropButton.Clicked += (sender, e) => { OnCropClicked(); };
            Children.Add(Cropperlayout, 0, 0);
            Children.Add(cropButton, 0, 1);
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (ImageSourceProperty.PropertyName.Equals(propertyName))
            {
                Cropperlayout.ImageSource = this.ImageSource;
            }
        }
        private async void OnCropClicked()
        {
           
            var x = Cropperlayout.panContainer.CropperContainer.Margin.Left + Cropperlayout.panContainer.CropperContainer.TranslationX +
                (Cropperlayout.panContainer.CropperView.X);

            var y = Cropperlayout.panContainer.CropperContainer.Margin.Top + Cropperlayout.panContainer.CropperContainer.TranslationY +
               (Cropperlayout.panContainer.CropperView.Y);

            var inputImage = new InputImage
            {
                ImageWidth = Cropperlayout.customImage.Width,
                ImageHeight = Cropperlayout.customImage.Height,
                ImageScale = Cropperlayout.customImage.Scale,
                ImageSource = Cropperlayout.ImageSource,
                CropX = x - Cropperlayout.customImage.X,
                CropY = y,
                CropWidth = Cropperlayout.panContainer.CropperView.Width,
                CropHeight = Cropperlayout.panContainer.CropperView.Height,
            };
            var result = await DependencyService.Get<IImageCropper>().CropImage(inputImage);
            OnImageCropped?.Invoke(this, result);
            
        }      
    }
}
