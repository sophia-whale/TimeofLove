using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TabbedTemplate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MemorialDetailPage : ContentPage
    {
        public MemorialDetailPage()
        {
            InitializeComponent();
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            TitleEntry.Text = "";
        }
    }
}