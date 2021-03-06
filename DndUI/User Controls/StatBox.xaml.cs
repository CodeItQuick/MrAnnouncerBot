﻿using System;
using DndCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DndUI
{
	/// <summary>
	/// Interaction logic for StatBox.xaml
	/// </summary>
	public partial class StatBox : UserControl
	{
		public static readonly RoutedEvent StatChangedEvent = EventManager.RegisterRoutedEvent("StatChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(StatBox));
		public static readonly RoutedEvent PreviewStatChangedEvent = EventManager.RegisterRoutedEvent("PreviewStatChanged", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(StatBox));

		public event RoutedEventHandler StatChanged
		{
			add { AddHandler(StatChangedEvent, value); }
			remove { RemoveHandler(StatChangedEvent, value); }
		}
		public event RoutedEventHandler PreviewStatChanged
		{
			add { AddHandler(PreviewStatChangedEvent, value); }
			remove { RemoveHandler(PreviewStatChangedEvent, value); }
		}
		protected virtual void OnStatChanged()
		{
			RoutedEventArgs previewEventArgs = new RoutedEventArgs(PreviewStatChangedEvent);
			RaiseEvent(previewEventArgs);
			if (previewEventArgs.Handled)
				return;
			RoutedEventArgs eventArgs = new RoutedEventArgs(StatChangedEvent);
			RaiseEvent(eventArgs);
		}
		
		public static readonly RoutedEvent ActivatedEvent = EventManager.RegisterRoutedEvent("Activated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(StatBox));
		public static readonly RoutedEvent PreviewActivatedEvent = EventManager.RegisterRoutedEvent("PreviewActivated", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(StatBox));

		public event RoutedEventHandler Activated
		{
			add { AddHandler(ActivatedEvent, value); }
			remove { RemoveHandler(ActivatedEvent, value); }
		}

		public event RoutedEventHandler PreviewActivated
		{
			add { AddHandler(PreviewActivatedEvent, value); }
			remove { RemoveHandler(PreviewActivatedEvent, value); }
		}

		protected virtual void OnActivated()
		{
			RoutedEventArgs previewEventArgs = new RoutedEventArgs(PreviewActivatedEvent);
			RaiseEvent(previewEventArgs);
			if (previewEventArgs.Handled)
				return;
			RoutedEventArgs eventArgs = new RoutedEventArgs(ActivatedEvent);
			RaiseEvent(eventArgs);
		}
		

		public static readonly DependencyProperty FocusItemProperty = DependencyProperty.Register("FocusItem", typeof(string), typeof(StatBox), new FrameworkPropertyMetadata(string.Empty));
		

		//TextBox txtEdit;
		public static readonly DependencyProperty StatBoxStateProperty = DependencyProperty.Register(
			nameof(StatBoxState), typeof(StatBoxState), typeof(StatBox),
			new FrameworkPropertyMetadata(StatBoxState.DisplayOnly, new PropertyChangedCallback(OnStatBoxStateChanged)));

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			nameof(Text), typeof(string), typeof(StatBox),
			new FrameworkPropertyMetadata("stat", new PropertyChangedCallback(OnStatBoxTextChanged)));

		public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
			nameof(TextAlignment), typeof(TextAlignment), typeof(StatBox),
			new FrameworkPropertyMetadata(TextAlignment.Center, new PropertyChangedCallback(OnStatBoxTextAlignmentChanged)));
		string saveText;


		static StatBox()
		{
			// Change defaults for inherited dependency properties...
			HorizontalAlignmentProperty.OverrideMetadata(typeof(StatBox), new FrameworkPropertyMetadata(HorizontalAlignment.Left));
			VerticalAlignmentProperty.OverrideMetadata(typeof(StatBox), new FrameworkPropertyMetadata(VerticalAlignment.Top));
		}

		//TextBox txtEdit;
		public StatBox()
		{
			InitializeComponent();
			txtDisplay.TextAlignment = TextAlignment.Center;
			txtEdit.TextAlignment = TextAlignment.Center;
			TextAlignment = TextAlignment.Center;
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;
			StatBoxState = StatBoxState.DisplayOnly;
		}

		public string FocusItem
		{
			// IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
			get
			{
				return (string)GetValue(FocusItemProperty);
			}
			set
			{
				SetValue(FocusItemProperty, value);
			}
		}
		public StatBoxState StatBoxState
		{
			get => (StatBoxState)GetValue(StatBoxStateProperty);
			set => SetValue(StatBoxStateProperty, value);
		}

		public TextAlignment TextAlignment
		{
			get => (TextAlignment)GetValue(TextAlignmentProperty);
			set => SetValue(TextAlignmentProperty, value);
		}

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		void OnTextChanged(string oldValue, string newValue)
		{
			txtDisplay.Text = newValue;
			txtEdit.Text = newValue;
		}

		private static void OnStatBoxTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (o is StatBox statBox)
				statBox.OnTextChanged((string)e.OldValue, (string)e.NewValue);
		}

		void OnTextAlignmentChanged(TextAlignment oldValue, TextAlignment newValue)
		{
			txtDisplay.TextAlignment = newValue;
			txtEdit.TextAlignment = newValue;
		}

		private static void OnStatBoxTextAlignmentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (o is StatBox statBox)
				statBox.OnTextAlignmentChanged((TextAlignment)e.OldValue, (TextAlignment)e.NewValue);
		}

		private static void OnStatBoxStateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (o is StatBox statBox)
				statBox.OnStatBoxStateChanged((StatBoxState)e.OldValue, (StatBoxState)e.NewValue);
		}

		protected virtual void OnStatBoxStateChanged(StatBoxState oldValue, StatBoxState newValue)
		{
			switch (StatBoxState)
			{
				case StatBoxState.DisplayOnly:
					FocusHelper.Remove(this);
					txtDisplay.Background = Brushes.Transparent;
					txtDisplay.Visibility = Visibility.Visible;
					txtEdit.Visibility = Visibility.Hidden;
					break;
				case StatBoxState.Focused:
					if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
						FocusHelper.ClearActiveStatBoxes();
					FocusHelper.Add(this);
					txtDisplay.Background = Brushes.LightGoldenrodYellow;
					txtDisplay.Visibility = Visibility.Visible;
					txtEdit.Visibility = Visibility.Hidden;
					OnActivated();
					// TODO: Update the overlay via SignalR.
					break;
				case StatBoxState.Editing:
					saveText = txtEdit.Text;
					txtDisplay.Visibility = Visibility.Hidden;
					txtEdit.Visibility = Visibility.Visible;
					if (oldValue == StatBoxState.Focused)
						txtEdit.Focus();
					OnActivated();
					break;
			}
		}
		private void TxtDisplay_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (StatBoxState == StatBoxState.DisplayOnly)
				StatBoxState = StatBoxState.Focused;
			else if (StatBoxState == StatBoxState.Focused)
				StatBoxState = StatBoxState.Editing;
			
		}

		private void TxtEdit_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Text = saveText;
				StatBoxState = StatBoxState.DisplayOnly;
			}
			else if (e.Key == Key.Enter)
				StatBoxState = StatBoxState.DisplayOnly;
			//StatBoxState = StatBoxState.Focused;
		}

		private void TxtDisplay_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				StatBoxState = StatBoxState.Editing;
		}

		private void TxtEdit_TextChanged(object sender, TextChangedEventArgs e)
		{
			Text = txtEdit.Text;
			OnStatChanged();
		}
		public int ToInt()
		{
			return Text.ToInt();
		}

		public double ToDouble()
		{
			return Text.ToDouble();
		}
		public decimal ToDecimal()
		{
			return Text.ToDecimal();
		}
	}
}
