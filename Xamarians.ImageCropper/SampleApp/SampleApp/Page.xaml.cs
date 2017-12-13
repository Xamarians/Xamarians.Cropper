using System;
using Xamarians.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page : ContentPage
	{
		public Page ()
		{
			InitializeComponent ();
		}
        private async void BtnClicked(object sender, EventArgs e)
        {           
            MediaResult result;
            var actions = new string[] { "Open Camera", "Open Gallery" };
            var action = await DisplayActionSheet("Change Picture", "Cancel", null, actions);
            if (actions[0].Equals(action))
            {

                var fileName = MediaService.Instance.GenerateUniqueFileName("jpg");
                var filePath = System.IO.Path.Combine(MediaService.Instance.GetPublicDirectoryPath(), fileName);
                result = await MediaService.Instance.TakePhotoAsync(new CameraOption() { FilePath = filePath });
                if (result.IsSuccess)
                 await App.Current.MainPage.Navigation.PushAsync(new MainPage(result.FilePath));
                else
                    await DisplayAlert("Error", result.Message, "OK");
            }
            else if (actions[1].Equals(action))
            {
                result = await MediaService.Instance.OpenMediaPickerAsync(MediaType.Image);
                if (result.IsSuccess)
                    await App.Current.MainPage.Navigation.PushAsync(new MainPage(result.FilePath));
                else
                    await DisplayAlert("Error", result.Message, "OK");
            }
        }   
    }
}