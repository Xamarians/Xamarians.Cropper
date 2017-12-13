using System;
using Xamarin.Forms;

namespace SampleApp
{
    public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public MainPage(string imageSource):this()
        {
            Cropperview.ImageSource = imageSource;
        }

        private void Cropperview_OnImageCropped(object sender, string e)
        {
            var page = new ContentPage
            {
                Content = new Image
                {
                    Source = e,
                }
            };
             App.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
