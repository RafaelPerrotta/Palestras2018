using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

//implementação do CognitiveVision
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace meetopCompVision
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void SelectPicture(bool camera = false)
        {
            await CrossMedia.Current.Initialize();

            MediaFile image = null;
            if (camera == false)
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    image = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium
                    });
                }
            }
            else
            {
                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Download",
                        Name = "Test.jpg",
                        PhotoSize = PhotoSize.Medium
                    });
                }
                else
                {
                    await DisplayAlert("Error", "Camera is not awailable", "Ok");
                    return;
                }

            }
            if (image != null)
            {
                var stream = image.GetStream();
                SelectedImage.Source = ImageSource.FromStream(() =>
                {
                    return stream;
                });
                var result = await GetImageDescription(image.GetStream());
                image.Dispose();



                Description.Text = result.Description.Captions[0].Text;

                foreach (string tag in result.Description.Tags)
                {
                    InfoLabel.Text = InfoLabel.Text + ", " + tag;
                }
            }


        }

        async void Filter()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);    // Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            string[] fileEntries = Directory.GetFiles(directory);
            foreach (string fileName in fileEntries)
            {
                FileStream stream = File.Open(fileName, FileMode.Open);
                var result = await GetImageDescription(stream);
               
            }
            InfoLabel.Text = "Completed!";
        }
        public async Task<AnalysisResult> GetImageDescription(Stream imageStream)
        {
            VisionServiceClient visionClient = new VisionServiceClient("f3c195d841fc438d8ed00e64e4639b7d", "https://southcentralus.api.cognitive.microsoft.com/vision/v1.0");
            VisualFeature[] features = { VisualFeature.Tags, VisualFeature.Categories, VisualFeature.Description , VisualFeature.Faces };
            return await visionClient.AnalyzeImageAsync(imageStream, features.ToList(), null);
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            SelectedImage.Source = null;
            InfoLabel.Text = "";
            Description.Text = string.Empty;
            SelectPicture(false);
        }
        private void CamButton_Clicked(object sender, EventArgs e)
        {
            SelectedImage.Source = null;
            InfoLabel.Text = "";
            Description.Text = string.Empty;
            SelectPicture(true);
        }
       
    }
}
