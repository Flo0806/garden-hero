using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GardenHero.ViewModels
{
	public partial class CategoryViewModel : ViewModelBase
	{
		[ObservableProperty]
		private int _id;

        [ObservableProperty]
        private string _name;

        public CategoryViewModel(string name)
		{
			Name = name;
		}
	}
}