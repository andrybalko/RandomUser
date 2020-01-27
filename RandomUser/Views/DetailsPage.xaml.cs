using RandomUser.ViewModels;
using RandomUser.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace RandomUser
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailsPage : ContentPage
	{
		public DetailsPage()
		{
			InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
        }

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			Debug.WriteLine(BindingContext);
		}

		private void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			var p = new AddNotePage();
			p.BindingContext = new NotesViewModel() { SelectedUser = ((BaseViewModel)this.BindingContext).SelectedUser };
			Navigation.PushModalAsync(p, true);
		}
	}
}