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
    public partial class AddMemorialDayPage : ContentPage
    {
        public AddMemorialDayPage()
        {
            InitializeComponent();
            DatePicker.Date = DateTime.Now;
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            TitleEntry.Text = "";
        }
    }
}