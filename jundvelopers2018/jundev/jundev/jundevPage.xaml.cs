using System;
using System.IO;
using System.Threading.Tasks;
using IBM.WatsonDeveloperCloud.VisualRecognition.v3;
using IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model;
using Plugin.Media;
using Xamarin.Forms;

namespace jundev
{
    public partial class jundevPage : ContentPage
    {

        string imagem = "https://pbs.twimg.com/media/C-ckO0PXgAU5ESK.jpg";

        public jundevPage()
        {
            InitializeComponent();


        }

        VisualRecognitionService visual = new VisualRecognitionService();
       
        public void GetImageDescription()
        {

            visual.SetCredential("sua-chave-ibm");

           
            SelectedImage.Source = txtimagem.Text;

            string[] classes = new string[]{"default", "food"};
            var result = visual.Classify(txtimagem.Text ,classes);
            TextInfo.Text = result.Images[0].Classifiers[0].Name;

            //var collect = visual.GetClassifiersBrief();

            foreach (var tag in result.Images[0].Classifiers[0].Classes)
            {
                InfoLabel.Text += "Acuracidade: " + tag.Score*100 + " para: " + tag._Class + ", ";
            }

        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            GetImageDescription();
        }
    }
}
