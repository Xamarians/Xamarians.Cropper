using Xamarin.Forms;

namespace Xamarians.ImageCropper.Controls
{
    public class PanContainer : ContentView
    {
        double xOffset = 0;
        double yOffset = 0;
        InputImage Image;      

        public BoxView CropperView;
        public Grid CropperContainer;
        public PanContainer()
        {
            Image = CropperLayout.inputImage;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);
            CropperContainer = new Grid
            {
                Margin = -600,
                RowSpacing = 0,
                ColumnSpacing = 0,    
            };           
            Initialize();
            Content = CropperContainer;

        }

        public void Initialize()
        {
            CropperContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            CropperContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1,GridUnitType.Auto) });
            CropperContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            CropperContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            CropperContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1,GridUnitType.Auto) });
            CropperContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var boxView1 = new BoxView
            {
                BackgroundColor = Color.FromHex("#90000000"),
            };

            var boxView2 = new BoxView
            {
                BackgroundColor = Color.FromHex("#90000000"),
            };

            var boxView3 = new BoxView
            {
                BackgroundColor = Color.FromHex("#90000000"),
            };

            var boxView4 = new BoxView
            {
                BackgroundColor = Color.FromHex("#90000000"),
            };

            CropperView = new BoxView
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = 200,
                HeightRequest = 200,
            };
            CropperContainer.Children.Add(boxView1, 1, 0);
            CropperContainer.Children.Add(boxView2, 1, 2);
            CropperContainer.Children.Add(boxView3, 0, 0);
            Grid.SetRowSpan(boxView3, 3);
            CropperContainer.Children.Add(boxView4, 2, 0);
            Grid.SetRowSpan(boxView4, 3);
            CropperContainer.Children.Add(CropperView, 1, 1);
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            double imageX = Image.CropX;
            double imageY = Image.CropY;
            double imageWidth = Image.ImageWidth;
            double imageHeight = Image.ImageHeight;
            var x = CropperContainer.Margin.Left + xOffset + e.TotalX + CropperView.X;
            var y = CropperContainer. Margin.Top + yOffset + e.TotalY + CropperView.Y;

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    if (x > imageX && x < imageWidth - CropperView.Width)
                    {
                       Content.TranslationX = xOffset + e.TotalX ;
                    }
                   else
                    {
                       
                    }
                    if (y > imageY && y < imageHeight - CropperView.Height)
                    {
                        Content. TranslationY = yOffset + e.TotalY;
                    }                   
                    break;

                case GestureStatus.Completed:
                    xOffset = Content.TranslationX;
                    yOffset = Content.TranslationY;
                    break;
            }
        }
    }
}


