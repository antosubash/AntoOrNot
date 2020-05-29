﻿using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AntoOrNot.Interface;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;

namespace AntoOrNot.Droid
{
    [Activity(Label = "AntoOrNot", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            UserDialogs.Init(this);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            // Register any platform specific implementations
            container.Register<IImageClassifier, ImageClassifier>();
        }
    }
}

