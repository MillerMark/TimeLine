using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		TimeSpan mouseDownTime;
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

		Dictionary<Rectangle, EventTimeAdorner> adorners = new Dictionary<Rectangle, EventTimeAdorner>();
		private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			HandleMouseDown(sender, e, EventMoveType.Start);
		}

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
			
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(rectangle);
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
				TimeSpan minDuration = TimeSpan.FromSeconds(0.1);
				if (newDuration < minDuration)
					newDuration = minDuration;
				entry.Duration = newDuration;
				if (eventTimeAdorner != null)
					eventTimeAdorner.EventTime = entry.Duration;
			}

			e.Handled = true;
		}

		private void Rectangle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			entry = null;
			Rectangle rectangle = sender as Rectangle;
			if (rectangle == null)
				return;

			if (adorners.ContainsKey(rectangle))
			{
				AdornerLayer myAdornerLayer = AdornerLayer.GetAdornerLayer(rectangle);
				myAdornerLayer.Remove(adorners[rectangle]);
				adorners.Remove(rectangle);
			}
			
			rectangle.ReleaseMouseCapture();
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

		private void MenuItem_Click(object sender, RoutedEventArgs e)
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
					timeLineEntry.Duration = TimeSpan.FromSeconds(frmSetDuration.Duration);
			}
		}
	}
}
