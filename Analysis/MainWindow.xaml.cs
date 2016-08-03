using Analysis.Controllers;
using Analysis.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Analysis
{
    public partial class MainWindow : Window
    {
        List<string> securities = new List<string>()
        {
            "SBER",
            "GAZP",
            "LKOH",
            "ROSN",
            "VTBR",
            "MGNT",
            "GMKN",
            "URKA"
        };

        MainController controller;
        ObservableCollection<Security> trackingSecurities;

        public MainWindow()
        {
            InitializeComponent();

            controller = new MainController();
            securityPicker.ItemsSource = securities;
            InitTrackingSecurities();
        }

        void InitTrackingSecurities()
        {
            trackingSecurities = controller.GetTrackingSecurities();
            trackingSecuritiesList.ItemsSource = trackingSecurities;
        }

        private void addSecurityButton_Click(object sender, RoutedEventArgs e)
        {
            var secName = (string)securityPicker.SelectedValue;
            if (secName == null)
                return;

            var sec = new Security();
            sec.SecurityName = secName;
            sec.DateFrom = DateTime.Now;
            sec.DateTo = DateTime.Now;
            sec.AutoUpdate = false;
            trackingSecurities.Add(sec);

        }

        private void TextBlock_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            var tb = (TextBlock)e.TargetObject;
            double num = double.Parse(tb.Text);
            string sym = "";
            if (Math.Abs(num) >= 1000)
            {
                num /= 1000;
                sym = "K";
            }
            if (Math.Abs(num) >= 1000)
            {
                num /= 1000;
                sym = "M";
            }
            if (Math.Abs(num) >= 1000)
            {
                num /= 1000;
                sym = "B";
            }

            if (num < 10)
                num = Math.Round(num, 2);
            else if (num < 100)
                num = Math.Round(num, 1);
            else
                num = Math.Round(num, 0);

            tb.Text = string.Format("{0:##0.##}{1}", num, sym);

            if (num > 0)
                tb.Foreground = new SolidColorBrush(Colors.Green);
            else if (num == 0)
                tb.Foreground = new SolidColorBrush(Colors.Black);
            else
                tb.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void trackingSecuritiesList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    int index = trackingSecuritiesList.SelectedIndex;
                    while (index != -1)
                    {
                        controller.RemoveTrackingSecurity(trackingSecurities[index].SecurityName);
                        trackingSecurities.RemoveAt(index);
                        index = trackingSecuritiesList.SelectedIndex;
                    }
                    break;
                case Key.Escape:
                    trackingSecuritiesList.UnselectAll();
                    break;
            }
        }

        private void DateFromPicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dateFromPicker.SelectedDate == null) return;
            var selectedItems = trackingSecuritiesList.SelectedItems;
            foreach (var item in selectedItems)
                ((Security)item).DateFrom = (DateTime)dateFromPicker.SelectedDate;
        }

        private void timeFromPicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (timeFromPicker.SelectedTime == null) return;
            var selectedItems = trackingSecuritiesList.SelectedItems;
            foreach (var item in selectedItems)
                ((Security)item).TimeFrom = (DateTime)timeFromPicker.SelectedTime;
        }

        private void DateToPicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dateToPicker.SelectedDate == null) return;
            var selectedItems = trackingSecuritiesList.SelectedItems;
            foreach (var item in selectedItems)
                ((Security)item).DateTo = (DateTime)dateToPicker.SelectedDate;
        }

        private void timeToPicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (timeToPicker.SelectedTime == null) return;
            var selectedItems = trackingSecuritiesList.SelectedItems;
            foreach (var item in selectedItems)
                ((Security)item).TimeTo = (DateTime)timeToPicker.SelectedTime;
        }
    }
}
