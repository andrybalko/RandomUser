using RandomUser.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RandomUser
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void ListViewBase_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var p = new DetailsPage();
			p.BindingContext = new DetailsViewModel() { SelectedUser = ((MainViewModel)this.BindingContext).SelectedUser };
			Navigation.PushAsync(p, true);
		}
	}
}
