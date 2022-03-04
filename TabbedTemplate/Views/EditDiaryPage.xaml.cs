using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.XForms.RichTextEditor;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageSource = Syncfusion.XForms.RichTextEditor.ImageSource;
using CustomItem = Syncfusion.XForms.Buttons;

namespace TabbedTemplate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditDiaryPage : ContentPage
    {
        CustomItem.SfButton readOnlyButton;
        public EditDiaryPage()
        {
            InitializeComponent();
            AddCustomToolbarItems();
        }

        private void rte_ImageInserted(object sender, ImageInsertedEventArgs e)
        {
            Syncfusion.XForms.RichTextEditor.ImageSource imgSrc = new ImageSource();
            Assembly assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream image = assembly.GetManifestResourceStream("sig.png");
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "sig.png");
            //var source = (ImageSource) FileImageSource.FromFile(fileName);
            imgSrc.ImageStream = image;
            e.ImageSourceCollection.Add(imgSrc);
            RTE.InsertImage(imgSrc);
        }

        private void AddCustomToolbarItems()
        {
            ObservableCollection<object> collection = new ObservableCollection<object>();
            collection.Add(ToolbarOptions.Bold);
            collection.Add(ToolbarOptions.Italic);
            collection.Add(ToolbarOptions.Underline);
            collection.Add(ToolbarOptions.FontColor);
            collection.Add(ToolbarOptions.FontSize);
            collection.Add(ToolbarOptions.HighlightColor);
            collection.Add(ToolbarOptions.NumberList);
            collection.Add(ToolbarOptions.BulletList);

            if (Device.RuntimePlatform != Device.iOS)
            {
                readOnlyButton = new CustomItem.SfButton();
                readOnlyButton.BackgroundColor = Color.Transparent;
                readOnlyButton.TextColor = Color.FromHex("#DE333333");
                if (Device.RuntimePlatform == Device.Android)
                {
                    readOnlyButton.HeightRequest = 44;
                    //readOnlyButton.FontFamily = "Text edit 2.ttf#";
                    readOnlyButton.Text = "\ue702";
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    //readOnlyButton.FontFamily = "Text edit 2.ttf#Text edit 2";
                    readOnlyButton.Text = "\ue700";
                }
                readOnlyButton.FontSize = 18;
                readOnlyButton.Clicked += ReadOnlyButton_Clicked;
                collection.Add(readOnlyButton);
            }
            else
            {
                Button readonlyButtonIOS = new Button();
                readonlyButtonIOS.FontFamily = "Text edit 2";
                readonlyButtonIOS.Text = "\ue700";
                readonlyButtonIOS.TextColor = Color.FromHex("#DE333333");
                readonlyButtonIOS.Clicked += ReadonlyButtonIOS_Clicked; ;
                collection.Add(readonlyButtonIOS);
            }
            RTE.ToolbarItems = collection;
        }

        CustomItem.SfButton CreateButton(string text)
        {
            CustomItem.SfButton button = new CustomItem.SfButton();
            if (Device.RuntimePlatform == Device.Android)
            {
                button.HeightRequest = 44;
            }
            button.BackgroundColor = Color.Transparent;
            button.TextColor = Color.FromHex("#DE333333");
            if (Device.RuntimePlatform == Device.Android)
            {
                button.FontFamily = "android";
                button.FontSize = 18;
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                button.FontFamily = "V3 Xamarin iOS uwp new.ttf#V3 Xamarin iOS uwp new";
            }
            button.Text = text;
            return button;
        }

        private void ReadonlyButtonIOS_Clicked(object sender, EventArgs e)
        {
            RTE.ReadOnly = !RTE.ReadOnly;
            if (RTE.ReadOnly)
            {
                (sender as Button).Text = "\ue701";
            }
            else
            {
                (sender as Button).Text = "\ue700";
            }
        }

        private void ReadOnlyButton_Clicked(object sender, EventArgs e)
        {
            RTE.ReadOnly = !RTE.ReadOnly;
            if (RTE.ReadOnly)
            {
                if (Device.RuntimePlatform == Device.Android)
                {

                    readOnlyButton.Text = "\ue703";
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    readOnlyButton.Text = "\ue701";
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    readOnlyButton.Text = "\ue702";
                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    readOnlyButton.Text = "\ue700";
                }
            }
        }
    }
}