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

namespace LearnNewWord
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window
    {
        public Overview()
        {
            InitializeComponent();
        }

        public Overview(List<Vocab> listVocabs )
        {
            InitializeComponent();
            DataGridVocab.ItemsSource = listVocabs;
        }

        private void CbWord_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            ColumnWord.Visibility = Visibility.Visible;

        }

        private void CbKana_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            ColumnKana.Visibility = Visibility.Visible;
        }

        private void CbMean_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            ColumnMeaning.Visibility = Visibility.Visible;
        }

        private void CbWord_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            ColumnWord.Visibility = Visibility.Collapsed;
        }

        private void CbKana_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            ColumnKana.Visibility = Visibility.Collapsed;
        }

        private void CbMean_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            ColumnMeaning.Visibility = Visibility.Collapsed;
        }
    }
}
