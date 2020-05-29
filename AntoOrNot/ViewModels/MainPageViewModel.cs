using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using AntoOrNot.Interface;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace AntoOrNot.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, IImageClassifier imageClassifier) 
            : base (navigationService)
        {
            Title = "Main Page";
            TakeAPictureCommand = new DelegateCommand(TakeAPicture);
            ChooseAPictureCommand = new DelegateCommand(ChooseAPictureAsync);
            this.imageClassifier = imageClassifier;
        }

        private async void ChooseAPictureAsync()
        {
            await CheckPermission();
            await CrossMedia.Current.Initialize();
            var image = await CrossMedia.Current.PickPhotoAsync();
            this.ImageSource = image.Path;
            var bitmap = await BitmapFactory.DecodeStreamAsync(image.GetStreamWithImageRotatedForExternalStorage());
            var result = imageClassifier.RecognizeImage(bitmap);
            ResultText = result;
        }

        private async void TakeAPicture()
        {
            await CheckPermission();
            await CrossMedia.Current.Initialize();
            var image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { PhotoSize = PhotoSize.Medium });
            this.ImageSource = image.Path;
            var bitmap = await BitmapFactory.DecodeStreamAsync(image.GetStreamWithImageRotatedForExternalStorage());
            var result = imageClassifier.RecognizeImage(bitmap);
            ResultText = result;
        }

        private async Task CheckPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        //await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Camera))
                        status = results[Permission.Camera];
                }

                if (status == PermissionStatus.Granted)
                {
                    
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        public DelegateCommand TakeAPictureCommand { get; set; }
        public DelegateCommand ChooseAPictureCommand { get; set; }

        private string _resultText;

        public string ResultText
        {
            get { return _resultText; }
            set { SetProperty(ref _resultText, value); }
        }

        private ImageSource _imageSource;
        private readonly IImageClassifier imageClassifier;

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set => SetProperty(ref _imageSource, value);
        }

    }
}
