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
		public EventTimeAdorner(UIElement adornedElement) : base(adornedElement)
		{
		}

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
				12,
				Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);

			double textWidth = formattedText.Width;
			double textHeight = formattedText.Height;
			const double horizontalMargin = 3;
			const double verticalMargin = 2;
			Point adornerTopLeft = adornedElementRect.BottomLeft;
			Point bottomRight = new Point(adornerTopLeft.X + textWidth + horizontalMargin * 2, adornerTopLeft.Y + textHeight + verticalMargin * 2);
			Rect rect = new Rect(adornerTopLeft, bottomRight);
			const double radius = 3;
			drawingContext.DrawRoundedRectangle(brush, pen, rect, radius, radius);

			Point textTopLeft = adornerTopLeft;
			textTopLeft.Offset(horizontalMargin, verticalMargin);
			drawingContext.DrawText(formattedText, textTopLeft);
		}

		public TimeSpan EventTime { get; set; }
	}
}
