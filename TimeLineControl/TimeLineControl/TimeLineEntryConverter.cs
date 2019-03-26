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
			double containerWidth = (double)values[2];

			double relativeDistance;
			if (relativeTime == System.Threading.Timeout.InfiniteTimeSpan)
			{
				TimeSpan start = (TimeSpan)values[3];
				double startPositionPixels = GetWidthPixels(start, containerWidth, timelineTotalDuration.TotalSeconds);
				relativeDistance = containerWidth - startPositionPixels;
			}
			else
			{
				double containerWidthPixels = GetWidthPixels(relativeTime, containerWidth, timelineTotalDuration.TotalSeconds);
				relativeDistance = containerWidthPixels * HackPercentWidth;
			}

			if (targetType == typeof(Thickness))
				return new Thickness(relativeDistance, 0, 0, 0);
			else
				return relativeDistance;
		}

		private static double GetWidthPixels(TimeSpan relativeTime, double containerWidth, double totalSeconds)
		{
			double factor;
			if (totalSeconds == 0)
				factor = 0;
			else
				factor = relativeTime.TotalSeconds / totalSeconds;
			return factor * containerWidth;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
