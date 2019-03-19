using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TimeLineControl
{
	public class MyObservableObject : INotifyPropertyChanged
	{

		public MyObservableObject()
		{

		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
