using RandomUserApi;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RandomUser
{
	public partial class App : Application
	{
		public IRestClient Client { get; private set; }

		public App()
		{
			InitializeComponent();

			Client = new RestClient(new RestClientConfig()
			{
				UseSeed = true
			});

			MainPage = new NavigationPage(new MainPage());
        }

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
