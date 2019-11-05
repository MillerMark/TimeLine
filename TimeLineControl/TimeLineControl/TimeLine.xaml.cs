using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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

namespace TimeLineControl
{
	/// <summary>
	/// Interaction logic for TimeLine.xaml
	/// </summary>
	public partial class TimeLine : UserControl
	{
		public static readonly RoutedEvent TimeLineChangedEvent = EventManager.RegisterRoutedEvent("TimeLineChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TimeLine));

		public event RoutedEventHandler TimeLineChanged
		{
			add { AddHandler(TimeLineChangedEvent, value); }
			remove { RemoveHandler(TimeLineChangedEvent, value); }
		}

		protected virtual void OnTimeLineChanged()
		{
			RoutedEventArgs eventArgs = new RoutedEventArgs(TimeLineChangedEvent);
			RaiseEvent(eventArgs);
		}
		
		public static readonly DependencyProperty MinEntryDurationProperty = DependencyProperty.Register("MinEntryDuration", typeof(double), typeof(TimeLine), new FrameworkPropertyMetadata(0.1, new PropertyChangedCallback(OnMinEntryDurationChanged), new CoerceValueCallback(OnCoerceMinEntryDuration)));

		TimeSpan mouseDownTime;

		public event SelectionChangedEventHandler SelectionChanged;

		public TimeLine()
		{
			InitializeComponent();
			MinEntryDuration = 0.1;
			lbTimeEntries.SelectionChanged += LbTimeEntries_SelectionChanged;
		}

		private void LbTimeEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.RemovedItems?.Count > 0 && e.RemovedItems[0] is TimeLineEntry timeLineEntry && timeLineEntry.IsRenaming)
				timeLineEntry.IsRenaming = false;
			SelectionChanged?.Invoke(this, e);
		}

		public static readonly DependencyProperty TotalDurationProperty = DependencyProperty.Register("TotalDuration", typeof(TimeSpan), typeof(TimeLine), new FrameworkPropertyMetadata(TimeSpan.Zero, new PropertyChangedCallback(OnTotalDurationChanged), new CoerceValueCallback(OnCoerceTotalDuration)));
		double mouseDownX;
		TimeLineEntry entry;

		private static object OnCoerceMinEntryDuration(DependencyObject o, object value)
		{
			TimeLine timeLine = o as TimeLine;
			if (timeLine != null)
				return timeLine.OnCoerceMinEntryDuration((double)value);
			else
				return value;
		}

		private static void OnMinEntryDurationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TimeLine timeLine = o as TimeLine;
			if (timeLine != null)
				timeLine.OnMinEntryDurationChanged((double)e.OldValue, (double)e.NewValue);
		}

		protected virtual double OnCoerceMinEntryDuration(double value)
		{
			// TODO: Keep the proposed value within the desired range.
			return value;
		}

		protected virtual void OnMinEntryDurationChanged(double oldValue, double newValue)
		{
			// TODO: Add your property changed side-effects. Descendants can override as well.
		}
		private static object OnCoerceEnd(DependencyObject o, object value)
		{
			TimeLine timeLine = o as TimeLine;
			if (timeLine != null)
				return timeLine.OnCoerceEnd((TimeSpan)value);
			else
				return value;
		}

		private static void OnEndChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TimeLine timeLine = o as TimeLine;
			if (timeLine != null)
				timeLine.OnEndChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);
		}

		protected virtual TimeSpan OnCoerceEnd(TimeSpan value)
		{
			// TODO: Keep the proposed value within the desired range.
			return value;
		}

		protected virtual void OnEndChanged(TimeSpan oldValue, TimeSpan newValue)
		{
			// TODO: Add your property changed side-effects. Descendants can override as well.
		}
		private static object OnCoerceTotalDuration(DependencyObject o, object value)
		{
			TimeLine timeLine = o as TimeLine;
			if (timeLine != null)
				return timeLine.OnCoerceTotalDuration((TimeSpan)value);
			else
				return value;
		}

		private static void OnTotalDurationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TimeLine timeLine = o as TimeLine;
			if (timeLine != null)
				timeLine.OnTotalDurationChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);
		}

		protected virtual TimeSpan OnCoerceTotalDuration(TimeSpan value)
		{
			// TODO: Keep the proposed value within the desired range.
			return value;
		}

		protected virtual void OnTotalDurationChanged(TimeSpan oldValue, TimeSpan newValue)
		{
			// TODO: Add your property changed side-effects. Descendants can override as well.
			OnTimeLineChanged();
		}

		public double MinEntryDuration
		{
			// IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
			get
			{
				return (double)GetValue(MinEntryDurationProperty);
			}
			set
			{
				SetValue(MinEntryDurationProperty, value);
			}
		}
		public TimeSpan End
		{
			// IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
			get
			{
				return (TimeSpan)GetValue(EndProperty);
			}
			set
			{
				SetValue(EndProperty, value);
			}
		}
		public TimeSpan TotalDuration
		{
			// IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
			get
			{
				return (TimeSpan)GetValue(TotalDurationProperty);
			}
			set
			{
				SetValue(TotalDurationProperty, value);
			}
		}

		public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(TimeSpan), typeof(TimeLine), new FrameworkPropertyMetadata(TimeSpan.Zero, new PropertyChangedCallback(OnEndChanged), new CoerceValueCallback(OnCoerceEnd)));


		public IEnumerable ItemsSource
		{
			get => lbTimeEntries.ItemsSource;

			set
			{
				if (lbTimeEntries.ItemsSource is INotifyCollectionChanged iNotifyPropertyChanged)
				{
					iNotifyPropertyChanged.CollectionChanged -= ItemsSource_CollectionChanged;
					foreach (TimeLineEntry timeLineEntry in lbTimeEntries.ItemsSource)
					{
						timeLineEntry.PropertyChanged -= TimeLineEntry_PropertyChanged;
					}
				}

				lbTimeEntries.ItemsSource = value;

				if (lbTimeEntries.ItemsSource is INotifyCollectionChanged iNotifyPropertyChanged2)
				{
					iNotifyPropertyChanged2.CollectionChanged += ItemsSource_CollectionChanged;
					foreach (TimeLineEntry timeLineEntry in lbTimeEntries.ItemsSource)
					{
						timeLineEntry.PropertyChanged += TimeLineEntry_PropertyChanged;
					}
				}
			}
		}

		public object SelectedItem { get => lbTimeEntries.SelectedItem; set => lbTimeEntries.SelectedItem = value; }
		


		private void TimeLineEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			UpdateTotalDuration();
		}

		private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateTotalDuration();
		}

		Dictionary<Rectangle, EventTimeAdorner> adorners = new Dictionary<Rectangle, EventTimeAdorner>();
		private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			HandleMouseDown(sender, e, EventMoveType.Start);
		}

		public static T FindParentOfType<T>(DependencyObject child) where T : DependencyObject
		{
			if (child == null)
				return null;
			DependencyObject parentDepObj = child;
			do
			{
				parentDepObj = VisualTreeHelper.GetParent(parentDepObj);
				T parent = parentDepObj as T;
				if (parent != null) return parent;
			}
			while (parentDepObj != null);
			return null;
		}

		// <formula 3; \omega>il Rocks!

		private void HandleMouseDown(object sender, MouseButtonEventArgs e, EventMoveType eventMoveType)
		{
			e.Handled = false;

			Rectangle rectangle = sender as Rectangle;
			if (rectangle == null)
				return;
			int index = (int)rectangle.Tag;
			if (index != lbTimeEntries.SelectedIndex)
				lbTimeEntries.SelectedIndex = index;
			mouseDownX = e.GetPosition(lbTimeEntries).X;
			entry = lbTimeEntries.SelectedItem as TimeLineEntry;
			if (entry == null)
				return;

			AdornerLayer adornerLayer = GetAdornerLayer(rectangle);

			EventTimeAdorner adorner = new EventTimeAdorner(rectangle);

			RemoveRectangleAdorner(rectangle, adornerLayer);    // Solves issue that happens only when reaching breakpoint after mouse down.

			adorners.Add(rectangle, adorner);
			adornerLayer.Add(adorner);

			if (eventMoveType == EventMoveType.Start)
			{
				mouseDownTime = entry.Start;
				adorner.EventTime = entry.Start;
			}
			else
			{
				mouseDownTime = entry.End;
				adorner.EventTime = entry.Duration;
			}

			rectangle.CaptureMouse();

			e.Handled = true;
		}

		private static AdornerLayer GetAdornerLayer(FrameworkElement frameworkElement)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);
			AdornerLayer parentAdornerLayer = adornerLayer;
			AdornerDecorator lastParentAdornerDecorator = null;
			do
			{
				AdornerDecorator parentAdornerDecorator = FindParentOfType<AdornerDecorator>(parentAdornerLayer);
				if (lastParentAdornerDecorator == parentAdornerDecorator)
					break;
				lastParentAdornerDecorator = parentAdornerDecorator;
				if (parentAdornerDecorator == null)
					parentAdornerLayer = null;
				else
					parentAdornerLayer = parentAdornerDecorator.AdornerLayer;

				if (parentAdornerLayer != null)
					adornerLayer = parentAdornerLayer;
			} while (parentAdornerLayer != null);
			return adornerLayer;
		}

		private void RemoveRectangleAdorner(Rectangle rectangle, AdornerLayer adornerLayer)
		{
			if (!adorners.ContainsKey(rectangle))
				return;

			adornerLayer.Remove(adorners[rectangle]);
			adorners.Remove(rectangle);
		}

		public enum EventMoveType
		{
			Start,
			Duration
		}

		private void Rectangle_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			HandleMouseMove(sender, e, EventMoveType.Start);
		}

		private void HandleMouseMove(object sender, MouseEventArgs e, EventMoveType eventMoveType)
		{
			e.Handled = false;
			if (entry == null)
				return;

			Rectangle rectangle = sender as Rectangle;
			if (rectangle == null)
				return;

			double thisX = e.GetPosition(lbTimeEntries).X;
			double distanceMoved = thisX - mouseDownX;
			double pixelsPerSecond = ActualWidth * TimeLineEntryConverter.HackPercentWidth / TotalDuration.TotalSeconds;

			TimeSpan timeMoved = TimeSpan.FromSeconds(distanceMoved / pixelsPerSecond);

			EventTimeAdorner eventTimeAdorner = null;
			if (adorners.ContainsKey(rectangle))
				eventTimeAdorner = adorners[rectangle];

			TimeSpan newTime = mouseDownTime + timeMoved;
			if (eventMoveType == EventMoveType.Start)
			{
				if (newTime.TotalSeconds < 0)
					entry.Start = TimeSpan.FromSeconds(0);
				else
					entry.Start = newTime;
				if (eventTimeAdorner != null)
					eventTimeAdorner.EventTime = entry.Start;
			}
			else
			{
				TimeSpan newDuration = newTime - entry.Start;
				TimeSpan minDuration = TimeSpan.FromSeconds(MinEntryDuration);
				if (newDuration < minDuration)
					newDuration = minDuration;
				entry.Duration = newDuration;
				if (eventTimeAdorner != null)
					eventTimeAdorner.EventTime = entry.Duration;
			}

			e.Handled = true;
		}

		void UpdateTotalDuration()
		{
			double maxSeconds = 0;
			foreach (TimeLineEntry timeLineEntry in ItemsSource)
			{
				if (timeLineEntry.Duration == System.Threading.Timeout.InfiniteTimeSpan)
					continue;
				maxSeconds = Math.Max(maxSeconds, timeLineEntry.End.TotalSeconds);
			}
			/* 
			 var maxValue = (from timelineItem in ItemSource select timelineItem.duration).Max();
			wil_bennett: .. alternatively: maxValue = ((IEnumerable<TimelineEntry>)ItemSource).Max(t => t.duration)... [untested] */

			End = TimeSpan.FromSeconds(maxSeconds);
		}
		private void Rectangle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			entry = null;
			Rectangle rectangle = sender as Rectangle;
			if (rectangle == null)
				return;

			if (adorners.ContainsKey(rectangle))
			{
				AdornerLayer myAdornerLayer = GetAdornerLayer(rectangle);
				myAdornerLayer.Remove(adorners[rectangle]);
				adorners.Remove(rectangle);
			}

			rectangle.ReleaseMouseCapture();
			UpdateTotalDuration();
			OnTimeLineChanged();
		}

		private void LbTimeEntries_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			// TODO: Add Mouse Wheel support.
		}

		private void ResizeRectangle_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			HandleMouseMove(sender, e, EventMoveType.Duration);
		}

		private void ResizeRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			HandleMouseDown(sender, e, EventMoveType.Duration);
		}

		private void SetDuration_Click(object sender, RoutedEventArgs e)
		{
			if (!(sender is MenuItem menuItem && menuItem.DataContext is TimeLineEntry timeLineEntry))
				return;

			FrmSetDuration frmSetDuration = new FrmSetDuration();
			Point screenPos = menuItem.PointToScreen(new Point(0, 0));
			if (timeLineEntry.Duration == Timeout.InfiniteTimeSpan)
				frmSetDuration.Duration = double.PositiveInfinity;
			else
				frmSetDuration.Duration = timeLineEntry.Duration.TotalSeconds;
			frmSetDuration.Left = screenPos.X;
			frmSetDuration.Top = screenPos.Y;
			if (frmSetDuration.ShowDialog() == true)
			{
				if (double.IsInfinity(frmSetDuration.Duration))
					timeLineEntry.Duration = Timeout.InfiniteTimeSpan;
				else
					timeLineEntry.Duration = TimeSpan.FromSeconds(Math.Max(MinEntryDuration, frmSetDuration.Duration));
				OnTimeLineChanged();
			}
		}

		public static T GetChildOfType<T>(DependencyObject depObj, int startingIndex = 0) where T : DependencyObject
		{
			if (depObj == null)
				return null;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
			{
				var child = VisualTreeHelper.GetChild(depObj, i);

				var result = (child as T) ?? GetChildOfType<T>(child);
				if (result != null) return result;
			}
			return null;
		}

		private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(obj, i);
				if (child != null && child is T)
					return (T)child;
				else
				{
					T childOfChild = FindVisualChild<T>(child);
					if (childOfChild != null)
						return childOfChild;
				}
			}
			return null;
		}

		T GetDataTemplateControl<T>(TimeLineEntry timeLineEntry, string controlName) where T : DependencyObject
		{
			ListBoxItem listBoxItem = (ListBoxItem)(lbTimeEntries.ItemContainerGenerator.ContainerFromItem(timeLineEntry));

			ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);

			DataTemplate myDataTemplate = contentPresenter.ContentTemplate;
			T result = (T)myDataTemplate.FindName(controlName, contentPresenter);
			return result;
		}

		private void Rename_Click(object sender, RoutedEventArgs e)
		{
			// TODO: inline the rename so there's no modal popup UI.
			if (SelectedItem is TimeLineEntry timeLineEntry)
			{
				TextBox textBox = GetDataTemplateControl<TextBox>(timeLineEntry, "TextBox");
				if (textBox != null)
				{
					textBox.SelectAll();
					textBox.Focus();
				}

				timeLineEntry.IsRenaming = true;
			}
		}
		public void DeleteSelected()
		{
			if (ItemsSource == null)
				return;

			// Uses reflection to allow users of this timeline to set the ItemsSource to an ObservableCollection<*TimeLineEntryDescendant*>...

			MethodInfo method = ItemsSource.GetType().GetMethod("Remove", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			if (method == null)
				return;

			method.Invoke(ItemsSource, new object[] { SelectedItem });

			// We need to re-index the elements in the timeline...
			IEnumerator enumerator = ItemsSource.GetEnumerator();
			if (enumerator != null)
			{
				int i = 0;
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is TimeLineEntry timeLineEntry)
						timeLineEntry.Index = i;
					i++;
				}
			}
			OnTimeLineChanged();
			//if (ItemsSource is System.Collections.ObjectModel.ObservableCollection<TimeLineEffect> entries)
			//{
			//	entries.Remove(SelectedItem as TimeLineEntry);
			//	for (int i = 0; i < entries.Count; i++)
			//	{
			//		entries[i].Index = i;
			//	}
			//	
			//}
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			OnTimeLineChanged();
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (!(SelectedItem is TimeLineEntry timeLineEntry))
				return;
			if (e.Key == Key.Enter)
				timeLineEntry.IsRenaming = false;
			if (e.Key == Key.Escape)
			{
				timeLineEntry.CancelRename();
				OnTimeLineChanged();
			}
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (!(SelectedItem is TimeLineEntry timeLineEntry))
				return;
			timeLineEntry.IsRenaming = false;
		}
	}
}
