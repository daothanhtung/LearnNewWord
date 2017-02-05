using System.Collections.Generic;
using System.Windows;

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
