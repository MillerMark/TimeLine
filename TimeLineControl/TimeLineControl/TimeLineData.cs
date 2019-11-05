using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TimeLineControl
{
public class TimeLineData : MyObservableObject
	{
		private ObservableCollection<TimeLineEntry> entries = new ObservableCollection<TimeLineEntry>();

		public TimeLineData()
		{
			entries.CollectionChanged += Entries_CollectionChanged;
		}



		private void Entries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged("Entries");
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
					return;

				if (entries != null)
					entries.CollectionChanged -= Entries_CollectionChanged;

				entries = value;
				OnPropertyChanged();
			}
		}

		public TimeLineEntry AddEntry(TimeSpan start, TimeSpan duration, string name, object data)
		{
			TimeLineEntry timeLineEntry = new TimeLineEntry() { Start = start, Duration = duration, Name = name, Data = data, Index = Entries.Count };
			Entries.Add(timeLineEntry);
			return timeLineEntry;
		}
	}
}
