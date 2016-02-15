using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
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

namespace LearnNewWord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //InitializeData();
        }

        private void InitializeData()
        {
            ComboBoxFile.Items.Clear();
            var fileInfo = new FileInfo(Application.ResourceAssembly.Location);
            var folder = fileInfo.Directory;
            if (folder != null)
            {
                var listTextFile = folder.GetFiles("*.txt");
                foreach (var info in listTextFile)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = info.Name;
                    item.Tag = info;
                    ComboBoxFile.Items.Add(item);
                }
            }
        }

        private void ButtonStart_OnClick(object sender, RoutedEventArgs e)
        {
            if (ComboBoxFile.SelectedItem != null)
            {
                var item = ComboBoxFile.SelectedItem as ComboBoxItem;
                string filePath = (item.Tag as FileInfo).FullName;
                int time2Next = 1000;
                int.TryParse(TbTimeToNext.Text, out time2Next);
                var notifier = new Notifier(filePath,time2Next, CbWord.IsChecked != null && (bool) CbWord.IsChecked, CbKana.IsChecked != null && (bool)CbKana.IsChecked, CbMean.IsChecked != null && (bool)CbMean.IsChecked);
                notifier.Closed += NotifierOnClosed;
                this.Visibility = Visibility.Hidden;
                notifier.Topmost = true;

                notifier.Top = System.Windows.SystemParameters.WorkArea.Height - notifier.Height - 10;
                notifier.Left = System.Windows.SystemParameters.WorkArea.Width - notifier.Width - 20;
                notifier.Show();
            }
        }

        private void NotifierOnClosed(object sender, EventArgs eventArgs)
        {
            this.Visibility = Visibility.Visible;
        }

        private void ComboBoxFile_OnDropDownOpened(object sender, EventArgs e)
        {
            InitializeData();
        }
    }
}
