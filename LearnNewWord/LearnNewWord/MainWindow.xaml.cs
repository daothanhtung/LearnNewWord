using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LearnNewWord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
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
                var listTextFile =
                    folder.EnumerateFiles().Where(file => IsTextFile(file.Name) || IsExcelFile(file.Name));
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
                var listVocab = GetListVocab();
                if (listVocab.Count == 0)
                {
                    MessageBox.Show("Không có từ vựng", "");
                    return;
                }
                //timer
                int time2Next = 1000;
                int.TryParse(TbTimeToNext.Text, out time2Next);
                var notifier = new Notifier(listVocab,
                    time2Next,
                    CbWord.IsChecked != null && (bool)CbWord.IsChecked,
                    CbKana.IsChecked != null && (bool)CbKana.IsChecked,
                    CbMean.IsChecked != null && (bool)CbMean.IsChecked,
                    CbShuffle.IsChecked != null && (bool)CbShuffle.IsChecked
                    );
                notifier.Closed += ChildWindowOnClosed;
                Visibility = Visibility.Hidden;
                notifier.Topmost = true;

                notifier.Top = SystemParameters.WorkArea.Height - notifier.Height - 10;
                notifier.Left = SystemParameters.WorkArea.Width - notifier.Width - 20;
                notifier.Show();
            }
        }

        private void ChildWindowOnClosed(object sender, EventArgs eventArgs)
        {
            Visibility = Visibility.Visible;
            Activate();
        }

        private void ComboBoxFile_OnDropDownOpened(object sender, EventArgs e)
        {
            InitializeData();
        }

        private void ButtonCheckAll_OnClick(object sender, RoutedEventArgs e)
        {
            var listItem = ListBoxPart.Items.OfType<CheckBox>();
            foreach (var checkBox in listItem)
            {
                checkBox.IsChecked = true;
            }
        }

        private void ButtonUncheckAll_OnClick(object sender, RoutedEventArgs e)
        {
            var listItem = ListBoxPart.Items.OfType<CheckBox>();
            foreach (var checkBox in listItem)
            {
                checkBox.IsChecked = false;
            }
        }

        private void ComboBoxFile_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFile.SelectedItem != null)
            {
                var item = ComboBoxFile.SelectedItem as ComboBoxItem;
                string filePath = (item.Tag as FileInfo).FullName;
                AnalyseFile(filePath);
            }
        }

        private void AnalyseFile(string filePath)
        {
            ListBoxPart.Items.Clear();
            if (File.Exists(filePath))
            {
                var listPart = new List<string>();
                if (IsTextFile(filePath))
                {
                    listPart = TextHelper.LoadAllPart(filePath);
                }
                else if(IsExcelFile(filePath))
                {
                    listPart = ExcelHelper.LoadAllPart(filePath);
                }
                
                foreach (var value in listPart)
                {
                    var checkBox = new CheckBox();
                    checkBox.Content = value;
                    checkBox.IsChecked = true;
                    checkBox.IsThreeState = false;
                    ListBoxPart.Items.Add(checkBox);
                }
            }
        }

        private void ButtonView_OnClick(object sender, RoutedEventArgs e)
        {
            if (ComboBoxFile.SelectedItem != null)
            {
                var listVocab = GetListVocab();
                if (listVocab.Count == 0)
                {
                    MessageBox.Show("Không có từ vựng", "");
                    return;
                }

                Visibility = Visibility.Hidden;
                var overview = new Overview(listVocab);
                overview.Closed += ChildWindowOnClosed;
                overview.ShowDialog();
                

            }
        }

        private bool IsTextFile(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && path.EndsWith(".txt");
        }

        private bool IsExcelFile(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && path.EndsWith(".xlsx");
        }

        private List<Vocab> GetListVocab()
        {
            var item = ComboBoxFile.SelectedItem as ComboBoxItem;
            string filePath = (item.Tag as FileInfo).FullName;
            var selectedPart = (from checkBox in ListBoxPart.Items.OfType<CheckBox>()
                                select checkBox.IsChecked != null && (bool)checkBox.IsChecked).ToArray();
            List<Vocab> listVocab = new List<Vocab>();
            if (IsTextFile(filePath))
            {
                listVocab = TextHelper.LoadAllVocabs(filePath, selectedPart);
            }
            else if (IsExcelFile(filePath))
            {
                listVocab = ExcelHelper.LoadAllVocabs(filePath, selectedPart);
            }
            return listVocab;
        }
    }
}
