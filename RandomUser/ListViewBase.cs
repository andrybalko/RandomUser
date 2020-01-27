using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RandomUser
{
	public class ListViewBase : ListView
	{
		private const int ListRefreshingResetDelay = 10000;

		public const int PageSize = 10;

		private int _currentPage = 0;

		private int _visiblePosition = 0;

		public int TotalElements
		{
			get => (int)GetValue(TotalElementsProperty);
			set => SetValue(TotalElementsProperty, value);
		}

		public static BindableProperty TotalElementsProperty = BindableProperty.Create("TotalElements", typeof(int), typeof(ListViewBase), 0);

		public ICommand LoadPageCommand
		{
			get => (ICommand)GetValue(LoadPageCommandProperty);
			set => SetValue(LoadPageCommandProperty, value);
		}

		public static BindableProperty LoadPageCommandProperty = BindableProperty.Create("LoadPageCommand",
			typeof(ICommand),
			typeof(ListViewBase), null);

		public ListViewBase() : base(ListViewCachingStrategy.RecycleElement)
		{
			this.BackgroundColor = Color.Transparent;
			this.SeparatorVisibility = SeparatorVisibility.Default;
			this.HorizontalOptions = LayoutOptions.FillAndExpand;
			this.VerticalOptions = LayoutOptions.FillAndExpand;
			this.IsPullToRefreshEnabled = true;
			ResetLoadingState();


			this.ItemAppearing += (sender, args) =>
			{
				if (ItemsSource == null)
				{
					return;
				}

				ObservableCollection<RandomUserApi.Models.User> source = (ObservableCollection<RandomUserApi.Models.User>)ItemsSource;

				if (source != null)
				{

					_visiblePosition = source.IndexOf((RandomUserApi.Models.User)args.Item);

					//Debug.WriteLine("is refreshing: " + IsRefreshing);


					if (IsRefreshing || source.Count == 0)
						return;

					var i = source.Count;


					if (_visiblePosition == i - 1 && (/*TotalElements == 0 || */TotalElements >= i))
					{
						int pageindex = (int)i / PageSize + 1;

						LoadPageCommand?.Execute(pageindex);
						ResetLoadingState();

						Debug.WriteLine("last item + " + TotalElements);
						if (Device.RuntimePlatform == Device.iOS)
						{
							//this.ScrollTo(args.Item, ScrollToPosition.MakeVisible, false);
						}

					}
				}
			};

		}

		private void ResetLoadingState()
		{
			Debug.WriteLine("reset list IsRefreshing state");

			//disable loading after timeout
			var t = Task.Run(async () =>
			{
				await Task.Delay(ListRefreshingResetDelay);
				if (!this.IsRefreshing)
				{
					if (this != null)
					{
						//Debug.WriteLine(this.IsRefreshing);
						this.IsRefreshing = false;
					}
				}
			});
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (BindingContext != null)
			{
				this.SetBinding(ListView.IsRefreshingProperty, "IsListRefreshing");
			}
		}
	}
}