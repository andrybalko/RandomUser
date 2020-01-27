using Plugin.Settings;
using RandomUser.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace RandomUser.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddNotePage : ContentPage
	{
		public AddNotePage()
		{
			InitializeComponent();
            On<iOS>().SetUseSafeArea(true);

            Notes.Focus();
        }

		private void Button_Clicked(object sender, EventArgs e)
		{
			BaseViewModel vm = (BaseViewModel)this.BindingContext;
			if (vm != null)
			{
				CrossSettings.Current.AddOrUpdateValue(vm.SelectedUser.Login.Uuid.ToString(), Notes.Text);
			}
			Navigation.PopModalAsync(true);
		}
	}
}