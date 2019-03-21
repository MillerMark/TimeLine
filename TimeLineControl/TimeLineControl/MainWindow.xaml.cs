using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		TimeLineData timeLineData = new TimeLineData();
		public MainWindow()
		{
			InitializeComponent();
			timeLineData.AddEntry(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(3), "Test 1", null);
			timeLineData.AddEntry(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(2), "Test 2", null);
			timeLineData.AddEntry(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), "Test 3", null);
			timeLineData.AddEntry(TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(0.5), "Test 4", null);
			timeLine.ItemsSource = timeLineData.Entries;
			timeLine.TotalDuration = TimeSpan.FromSeconds(5);
		}

		//private ObservableCollection<TimeLineData> _timeLines = new ObservableCollection<TimeLineData>();
		//public ObservableCollection<TimeLineData> TimeLines
		//{
		//	get
		//	{
		//		return _timeLines;
		//	}
		//	set
		//	{
		//		Set(() => TimeLines, ref _timeLines, value);
		//	}
		//}
	}
}
