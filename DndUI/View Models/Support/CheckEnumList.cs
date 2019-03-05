﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndUI
{
	public class CheckEnumList : ViewModelBase
	{
		private void Initialize(Type enumType)
		{
			_enumType = enumType;

			Items = new ObservableCollection<CheckEnumViewModel>();
			Items.CollectionChanged += Items_CollectionChanged;
		}

		private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			
		}

		public CheckEnumList(Type enumType, object filterItems, EnumListOption enumListOption = EnumListOption.Add)
		{
			Initialize(enumType);

			int filterValue = Convert.ToInt32(filterItems);

			foreach (object enumValue in Enum.GetValues(_enumType))
			{
				int enumValueAsInt = Convert.ToInt32(enumValue);
				bool shouldAddItem = (filterValue & enumValueAsInt) != 0;
				if (enumListOption == EnumListOption.Exclude)
					if (filterValue == 0)
						shouldAddItem = enumValueAsInt != 0;
					else
						shouldAddItem = !shouldAddItem;

				if (shouldAddItem)
				{
					var item = new CheckEnumViewModel(enumValue);
					item.PropertyChanged += HandleItemPropertyChanged;
					Items.Add(item);
				}
			}
		}

		public CheckEnumList(Type enumType)
		{
			Initialize(enumType);
			foreach (object value in Enum.GetValues(_enumType))
			{
				var item = new CheckEnumViewModel(value);
				item.PropertyChanged += HandleItemPropertyChanged;
				Items.Add(item);
			}
		}


		private object _value;

		void SetCheckedFromValue(int value)
		{
			settingInternally = true;
			try
			{
				foreach (CheckEnumViewModel item in Items)
				{
					int itemValue = Convert.ToInt32(item.Value);
					item.IsChecked = (value & itemValue) == itemValue;
				}
			}
			finally
			{
				settingInternally = false;
			}
		}

		public object Value
		{
			get => _value;
			set
			{
				_value = value;
				SetCheckedFromValue((int)_value);
				OnPropertyChanged();
			}
		}

		public ObservableCollection<CheckEnumViewModel> Items { get; private set; }

		private Type _enumType;
		bool settingInternally;

		private object CalcValue()
		{
			// Assumes the enums are ints.  Can change if needed (can dynamically determine as well).
			int result = 0;

			foreach (CheckEnumViewModel item in Items)
			{
				if (item.IsChecked)
					result |= Convert.ToInt32(item.Value);
			}

			return Enum.ToObject(_enumType, result);
		}

		private void HandleItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (settingInternally)
				return;
			Value = CalcValue();
		}
	}
}