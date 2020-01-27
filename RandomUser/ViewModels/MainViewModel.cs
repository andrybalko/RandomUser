using RandomUser.ViewModels;
using RandomUserApi;
using RandomUserApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RandomUser
{
	public class MainViewModel:BaseViewModel
	{
		private IRestClient client;

		public string Gender { get; set; }
		public string Nationality { get; set; }

		public bool IsListRefreshing { get; set; }

		public int TotalUsers => Users != null ? Users.Count : 0;
		public MainViewModel()
		{
			client = new RestClient(new RestClientConfig()
			{
				UseSeed = true
			});
			LoadUsersCommand.Execute(null);
		}

		private int GetPageNumber()
		{
			if (Users != null)
			{
				return Users.Count / 10 + 1;
			}
			return 1;
		}

		public ObservableCollection<User> Users { get; set; }

		public ICommand LoadUsersCommand => new Command(async () =>
		{
			var users = await client.MakeRequestAsync(FillRequest());

			var t = new ObservableCollection<User>(Users??new ObservableCollection<User>());
			foreach (var item in users)
			{
				t.Add(item);
			}
			Users = new ObservableCollection<User>(t);
			OnPropertyChanged(nameof(Users));
			OnPropertyChanged(nameof(TotalUsers));
			IsListRefreshing = false;
			OnPropertyChanged(nameof(IsListRefreshing));

		});

		private Request FillRequest()
		{
			var req = client.CreateRequest().Results().ForPage(GetPageNumber());

			if (Nationality?.Length > 0)
			{
				req.ForNationality(Nationality.Replace(" ", "")).ForSeed();

			}
			if (Gender?.Length > 0)
			{
				//seeds don't work with gender selection
				req.ForGender(Gender.Replace(" ", ""));
			}

			return req;
		}

		public ICommand RefreshUsersCommand => new Command(async () =>
		{
			Users = new ObservableCollection<User>();

			var users = await client.MakeRequestAsync(FillRequest());
			foreach (var item in users)
			{
				Users.Add(item);
			}
			OnPropertyChanged(nameof(Users));
			OnPropertyChanged(nameof(TotalUsers));
			IsListRefreshing = false;
			OnPropertyChanged(nameof(IsListRefreshing));

		});
	}
}
