using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUser.ViewModels
{
	public class DetailsViewModel : BaseViewModel
	{
		private int selectedTabIndex;

		public int SelectedTabIndex { 
			get => selectedTabIndex;
			set
			{
				selectedTabIndex = value;
				SetData();
			}
		}

		public string PropName { get; set; }
		public string PropValue { get; set; }


		private void SetData()
		{
			switch (selectedTabIndex)
			{
				case 0:
					PropName = "Hi, my name is";
					PropValue = SelectedUser.Name.First + " " + SelectedUser.Name.Last;
					break;
				case 1:
					PropName = "My email address is";
					PropValue = SelectedUser.Email;
					break;
				case 2:
					PropName = "My birthday is";
					PropValue = SelectedUser.Dob.Date.ToString();
					break;
				case 3:
					PropName = "My address is";
					PropValue = SelectedUser.Location.Street.Number + ", " + SelectedUser.Location.Street.Name;
					break;
				case 4:
					PropName = "My phone number is";
					PropValue = SelectedUser.Phone;
					break;
				case 5:
					PropName = "My password is";
					PropValue = SelectedUser.Login.Password;
					break;
				default:
					break;
			}
			OnPropertyChanged(nameof(PropName));
			OnPropertyChanged(nameof(PropValue));
		}
	}
}
