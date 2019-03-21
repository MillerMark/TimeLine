using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TimeLineControl
{
	public class TimeLineEntry : MyObservableObject
	{
		//private fields...
		TimeSpan duration;
		TimeSpan start;

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

		public int Index { get; set; }


		public TimeLineEntry()
		{

		}
	}
}
