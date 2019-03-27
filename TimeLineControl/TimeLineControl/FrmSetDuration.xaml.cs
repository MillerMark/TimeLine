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
using System.Windows.Shapes;

namespace TimeLineControl
{
	/// <summary>
	/// Interaction logic for FrmSetDuration.xaml
	/// </summary>
	public partial class FrmSetDuration : Window
	{
		const string STR_Infinity = "∞";
		string lastSetDurationStr;
		string lastRealDurationStr;
		bool changingInternally;
		public double Duration
		{
			get
			{
				if (double.TryParse(tbxDuration.Text, out double duration))
					return duration;
				return double.PositiveInfinity;
			}
			set
			{
				string durationStr = value.ToString();
				if (double.IsInfinity(value))
				{
					lastRealDurationStr = tbxDuration.Text;
					durationStr = STR_Infinity;
				}
				else
					lastRealDurationStr = value.ToString();
				lastSetDurationStr = durationStr;
				tbxDuration.Text = durationStr;
			}
		}

		public FrmSetDuration()
		{
			InitializeComponent();
		}

		private void CkIsInfinity_Checked(object sender, RoutedEventArgs e)
		{
			if (changingInternally)
				return;
			//tbxDuration.Text = STR_Infinity;
			Duration = double.PositiveInfinity;
		}

		private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void CkIsInfinity_Unchecked(object sender, RoutedEventArgs e)
		{
			if (changingInternally)
				return;
			tbxDuration.Text = lastRealDurationStr;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			tbxDuration.Text = Duration.ToString();
		}

		private void TbxDuration_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (ckIsInfinity == null)
				return;
			changingInternally = true;
			try
			{
				ckIsInfinity.IsChecked = tbxDuration.Text == STR_Infinity;
			}
			finally
			{
				changingInternally = false;
			}
		}
	}
}
