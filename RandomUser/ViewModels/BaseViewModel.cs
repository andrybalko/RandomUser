using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using RandomUserApi.Models;

namespace RandomUser.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		private User selectedUser;

		public User SelectedUser { 
			get => selectedUser; 
			set { 
				selectedUser = value;
			}
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
