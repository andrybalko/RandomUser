using Plugin.Settings;
using RandomUser.ViewModels;
using System;
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
				//saving note here. It is not displayed, but can be
				CrossSettings.Current.AddOrUpdateValue(vm.SelectedUser.Login.Uuid.ToString(), Notes.Text);
			}
			Navigation.PopModalAsync(true);
		}
	}
}