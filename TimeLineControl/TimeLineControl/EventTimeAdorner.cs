using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace TimeLineControl
{
	public class EventTimeAdorner : Adorner
	{
		// Be sure to call the base class constructor.
		public EventTimeAdorner(UIElement adornedElement)
			: base(adornedElement)
		{
		}

		// A common way to implement an adorner's rendering behavior is to override the OnRender
		// method, which is called by the layout system as part of a rendering pass.
		protected override void OnRender(DrawingContext drawingContext)
		{
			Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

			SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(255, 247, 222));
			Pen pen = new Pen(new SolidColorBrush(Color.FromRgb(201, 184, 151)), 1);

			// adornedElementRect.TopLeft

			FormattedText formattedText = new FormattedText(
				string.Format("{0:0.##} sec", EventTime.TotalSeconds),
				CultureInfo.GetCultureInfo("en-us"),
				FlowDirection.LeftToRight,
				new Typeface("Verdana"),
				16,
				Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);

			double textWidth = formattedText.Width;
			double textHeight = formattedText.Height;
			Point textTopLeft = adornedElementRect.BottomLeft;
			Point bottomRight = new Point(textTopLeft.X + textWidth, textTopLeft.Y + textHeight);
			Rect rect = new Rect(textTopLeft, bottomRight);
			const double radius = 3;
			drawingContext.DrawRoundedRectangle(brush, pen, rect, radius, radius);

			drawingContext.DrawText(formattedText, textTopLeft);
		}

		public TimeSpan EventTime { get; set; }
	}
}
