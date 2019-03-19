using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TimeLineControl
{
	public class TimeLineData : MyObservableObject
	{
		TimeSpan totalDuration;
		private ObservableCollection<TimeLineEntry> entries = new ObservableCollection<TimeLineEntry>();

		public TimeLineData()
		{

		}

		public TimeSpan TotalDuration
		{
			get { return totalDuration; }
			set
			{
				if (totalDuration == value)
					return;
				totalDuration = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<TimeLineEntry> Entries
		{
			get
			{
				return entries;
			}
			set
			{
				if (entries == value)
				{
					return;
				}

				entries = value;
				OnPropertyChanged();
			}
		}
		public void AddEntry(TimeSpan start, TimeSpan duration, string name, object data)
		{
			TimeLineEntry timeLineEntry = new TimeLineEntry() { Start = start, Duration = duration, Name = name, Data = data };
			Entries.Add(timeLineEntry);
		}
	}
}
