using System;
using System.Collections;
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

namespace TimeLineControl
{
	/// <summary>
	/// Interaction logic for TimeLine.xaml
	/// </summary>
	public partial class TimeLine : UserControl
	{
		TimeSpan mouseDownStartTime;
		public TimeLine()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty TotalDurationProperty = DependencyProperty.Register("TotalDuration", typeof(TimeSpan), typeof(TimeLine), new FrameworkPropertyMetadata(TimeSpan.Zero, new PropertyChangedCallback(OnTotalDurationChanged), new CoerceValueCallback(OnCoerceTotalDuration)));
		double mouseDownX;
		TimeLineEntry entry;


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
		public IEnumerable ItemsSource { get => lbTimeEntries.ItemsSource; set => lbTimeEntries.ItemsSource = value; }

		private T FindParent<T>(DependencyObject child) where T : DependencyObject
		{
			DependencyObject immediateParent = VisualTreeHelper.GetParent(child);
			T parent = immediateParent as T;
			if (parent != null)
				return parent;
			else
				return FindParent<T>(immediateParent);
		}

		private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

			//ListBoxItem parentListBoxItem = FindParent<ListBoxItem>(sender as DependencyObject);
			//if (parentListBoxItem != null)
			//{
			//	TimeLineEntry timeLineEntry = parentListBoxItem.DataContext as TimeLineEntry;
			//	if (timeLineEntry != null)
			//	{
			//		if (timeLineEntry != entry)
			//			return;
			//	}
			//}

			mouseDownStartTime = entry.Start;
			rectangle.CaptureMouse();

			e.Handled = true;
		}

		private void Rectangle_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			e.Handled = false;
			if (entry == null)
				return;

			double thisX = e.GetPosition(lbTimeEntries).X;
			double distanceMoved = thisX - mouseDownX;
			double pixelsPerSecond = ActualWidth * TimeLineEntryConverter.HackPercentWidth / TotalDuration.TotalSeconds;

			TimeSpan timeMoved = TimeSpan.FromSeconds(distanceMoved / pixelsPerSecond);

			TimeSpan newStart = mouseDownStartTime + timeMoved;
			if (newStart.TotalSeconds < 0)
				entry.Start = TimeSpan.FromSeconds(0);
			else
				entry.Start = newStart;
		}


		private void Rectangle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			entry = null;
			Rectangle rectangle = sender as Rectangle;
			if (rectangle == null)
				return;
			rectangle.ReleaseMouseCapture();
		}

		private void LbTimeEntries_MouseWheel(object sender, MouseWheelEventArgs e)
		{

		}
	}
}
