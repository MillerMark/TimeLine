using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;

namespace TimeLineControl
{
	public class TimeLineEntry : MyObservableObject
	{
		//private fields...
		bool isDisplaying;
		Brush fill;
		Brush foreground;
		bool durationLocked;
		TimeSpan duration;
		TimeSpan start;


		public Brush Fill
		{
			get { return fill; }
			set
			{
				if (fill == value)
					return;
				fill = value;
				OnPropertyChanged();
			}
		}
		public Brush Foreground
		{
			get { return foreground; }
			set
			{
				if (foreground == value)
					return;
				foreground = value;
				OnPropertyChanged();
			}
		}

		public TimeSpan Start
		{
			get { return start; }
			set
			{
				if (start == value)
					return;
				start = value;
				OnPropertyChanged();
				OnPropertyChanged("End");
			}
		}

		public bool DurationLocked
		{
			get { return durationLocked; }
			set
			{
				if (durationLocked == value)
					return;
				durationLocked = value;
				OnPropertyChanged();
				OnPropertyChanged("DurationUnlocked");
				OnPropertyChanged("CanResize");
			}
		}



		public bool CanResize
		{
			get { return !DurationLocked && !IsInfinite; }
		}


		public bool IsInfinite
		{
			get
			{
				return Duration == Timeout.InfiniteTimeSpan;
			}
		}



		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool DurationUnlocked
		{
			get { return !DurationLocked; }
			set
			{
				DurationLocked = !value;
			}
		}

		public TimeSpan Duration
		{
			get { return duration; }
			set
			{
				if (duration == value)
					return;
				duration = value;
				OnPropertyChanged();
				OnPropertyChanged("End");
				OnPropertyChanged("CanResize");
				OnPropertyChanged("IsInfinite");
			}
		}

		public TimeSpan End
		{
			get
			{
				return start + duration;
			}
		}


		string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (name == value)
				{
					return;
				}

				name = value;
				OnPropertyChanged();
			}
		}
		object data;
		public object Data
		{
			get
			{
				return data;
			}
			set
			{
				if (data == value)
				{
					return;
				}

				data = value;
				OnPropertyChanged();
			}
		}

		int index;
		public int Index
		{
			get
			{
				return index;
			}
			set
			{
				if (index == value)
					return;

				index = value;
				OnPropertyChanged();
			}
		}

		bool isRenaming;
		string saveName;

		public bool IsRenaming
		{
			get
			{
				return isRenaming;
			}
			set
			{
				isRenaming = value;
				if (isRenaming)
					saveName = Name;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsDisplaying));
			}
		}

		public void CancelRename()
		{
			Name = saveName;
			OnPropertyChanged("Name");
			IsRenaming = false;
		}


		public bool IsDisplaying
		{
			get { return isDisplaying; }
			set
			{
				if (isDisplaying == value)
					return;
				isDisplaying = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsRenaming));
			}
		}
		


		public TimeLineEntry()
		{
			foreground = Brushes.White;
			fill = Brushes.DarkSlateBlue;
			IsDisplaying = true;
		}
	}
}
