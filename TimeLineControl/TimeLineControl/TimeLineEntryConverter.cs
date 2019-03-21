using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace TimeLineControl
{
	public class TimeLineEntryConverter : IMultiValueConverter
	{
		public const double HackPercentWidth = 0.9; // TODO: Find the actual width of the parenting Grid in the Visual Tree.
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			TimeSpan timelineTotalDuration = (TimeSpan)values[0];
			TimeSpan relativeTime = (TimeSpan)values[1];
			double containerWidth = (double)values[2] * HackPercentWidth;
			double factor;
			if (timelineTotalDuration.TotalSeconds == 0)
				factor = 0;
			else
				factor = relativeTime.TotalSeconds / timelineTotalDuration.TotalSeconds;
			double relativeDistance = factor * containerWidth;

			if (targetType == typeof(Thickness))
				return new Thickness(relativeDistance, 0, 0, 0);
			else
				return relativeDistance;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
