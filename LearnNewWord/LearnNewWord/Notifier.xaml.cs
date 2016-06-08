using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Threading;

namespace LearnNewWord
{
    /// <summary>
    /// Interaction logic for Notifier.xaml
    /// </summary>
    public partial class Notifier : Window
    {
        private readonly bool _isWord;
        private readonly bool _isKana;
        private readonly bool _isMean;
        private readonly bool _isShuffle;
        //private string[] _listString;
        private List<Vocab> _listVocabs;
        private DispatcherTimer _timer;
        private int _currentN;
        public Notifier()
        {
            InitializeComponent();
        }

        //public Notifier(string filePath, int timeToNext, bool isWord, bool isKana, bool isMean, bool isShuffle)
        //{
        //    _isWord = isWord;
        //    _isKana = isKana;
        //    _isMean = isMean;
        //    _isShuffle = isShuffle;
        //    InitializeComponent();
        //    InitializeData(filePath);
        //    StartTimer(timeToNext);
        //}

        //public Notifier(string filePath, int timeToNext, bool isWord, bool isKana, bool isMean, bool isShuffle, bool[] selectedParts)
        //{
        //    _isWord = isWord;
        //    _isKana = isKana;
        //    _isMean = isMean;
        //    _isShuffle = isShuffle;
        //    InitializeComponent();
        //    InitializeData(filePath,selectedParts);
        //    StartTimer(timeToNext);
        //    Word.Inlines.Clear();
        //    Word.Inlines.Add(new Run($"Thời gian chuyển: {timeToNext}\r\nSố lượng từ: {_listString.Length}\r\nThời gian chạy: {(timeToNext * _listString.Length)/1000}s")
        //    {
        //        FontSize = 20,
        //        FontWeight = FontWeights.Bold
        //    });
            
        //}

        public Notifier(List<Vocab> listVocab, int timeToNext, bool isWord, bool isKana, bool isMean, bool isShuffle)
        {
            _isWord = isWord;
            _isKana = isKana;
            _isMean = isMean;
            _isShuffle = isShuffle;
            _listVocabs = listVocab;
            InitializeComponent();
            StartTimer(timeToNext);
            Word.Inlines.Clear();
            Word.Inlines.Add(new Run($"Thời gian chuyển: {timeToNext}\r\nSố lượng từ: {_listVocabs.Count}\r\nThời gian chạy: {(timeToNext * _listVocabs.Count) / 1000}s")
            {
                FontSize = 20,
                FontWeight = FontWeights.Bold
            });
        }

        //private void InitializeData(string filePath, bool[] selectedParts)
        //{
        //    if (File.Exists(filePath))
        //    {
        //        var arr = File.ReadAllLines(filePath);
        //        var patternPart = @"#.+#";
        //        var pattern = @".*\t.*\t.*";
        //        var list = new List<string>();
        //        int countPart = 0;
        //        bool flag = true;//mark when..
        //        foreach (var s in arr)
        //        {
        //            if (Regex.IsMatch(s, patternPart))
        //            {
        //                flag = selectedParts[countPart++];
        //            }
        //            else if (flag && !string.IsNullOrWhiteSpace(s) && !s.StartsWith("//") && Regex.IsMatch(s, pattern))
        //            {
        //                list.Add(s);
        //            }
        //        }
        //        _listString = list.ToArray();
        //    }
        //    else
        //    {
        //        Close();
        //    }
        //}

        private void StartTimer(int timeToNext)
        {
            _timer = new DispatcherTimer();
            if (_isShuffle)
            {
                _currentN = _listVocabs.Count;
                _timer.Tick += TimerOnTickShuffle;
            }
            else
            {
                _currentN = -1;
                _timer.Tick += TimerOnTickNormal;
            }
            
            _timer.Interval = new TimeSpan(0,0,0,0,timeToNext);
            _timer.Start();
        }

        private void TimerOnTickNormal(object sender, EventArgs e)
        {
            //if (_listString.Length == 0)
            //{
            //    _timer.Stop();
            //    Close();
            //    return;
            //}

            Word.Text = string.Empty;
            if (WindowState != WindowState.Normal)
            {
                Topmost = true;
                IntPtr handle = new WindowInteropHelper(this).Handle;
                ShowWindow(handle, 4);
            }

            //get next word
            if (_currentN == _listVocabs.Count -1)
            {
                _currentN = -1;
                Word.Inlines.Add(new Run("Đã xem hết tất cả các từ.\r\nChuẩn bị xem lại.")
                {
                    FontSize = 20,
                    FontWeight = FontWeights.Bold
                });
                return;
            }
            //string s = _listString[++_currentN];
            //show next word
            ShowVocab(_listVocabs[++_currentN]);
        }

        private void TimerOnTickShuffle(object sender, EventArgs eventArgs)
        {
            //if (_listString.Length == 0)
            //{
            //    _timer.Stop();
            //    Close();
            //    return;
            //}
            Word.Text = string.Empty;
            if (WindowState != WindowState.Normal)
            {
                Topmost = true;
                IntPtr handle = new WindowInteropHelper(this).Handle;
                ShowWindow(handle, 4);
            }
            //get next word
            if (_currentN == 0)
            {
                _currentN = _listVocabs.Count;
                Word.Inlines.Add(new Run("Đã xem hết tất cả các từ.\r\nChuẩn bị xem lại.")
                {
                    FontSize = 20,
                    FontWeight = FontWeights.Bold
                });
                return;
            }

            Random random = new Random();
            int t = random.Next(_currentN);
            var vocab = _listVocabs[t];
            _listVocabs[t] = _listVocabs[_currentN - 1];
            _listVocabs[_currentN - 1] = vocab;
            _currentN--;
            //show next word
           ShowVocab(vocab);
        }

        //private void ShowWord(string s)
        //{
        //    if (string.IsNullOrWhiteSpace(s))
        //    {
        //        return;
        //    }
        //    var arr = s.Split('\t');
        //    var word = arr[0];
        //    var kana = arr[1];
        //    var mean = arr[2];

        //    if (_isWord)
        //    {
        //        Word.Inlines.Add(new Run($"{word}")
        //        {
        //            FontSize = 25,
        //            FontWeight = FontWeights.Bold
        //        });
        //    }

        //    if (_isKana)
        //    {
        //        Word.Inlines.Add(new LineBreak());
        //        Word.Inlines.Add(new Run($"「{kana}」")
        //        {
        //            FontSize = 18,
        //            FontStyle = FontStyles.Italic,
        //            FontWeight = FontWeights.DemiBold
        //        });
        //    }

        //    if (_isMean)
        //    {
        //        Word.Inlines.Add(new LineBreak());
        //        Word.Inlines.Add(new Run(mean)
        //        {
        //            FontSize = 20,
        //            FontWeight = FontWeights.DemiBold
        //        });
        //    }

        //    Topmost = true;
        //}

        private void ShowVocab(Vocab v)
        {
            if (v == null)
            {
                return;
            }
            

            if (_isWord)
            {
                Word.Inlines.Add(new Run($"{v.Word}")
                {
                    FontSize = 25,
                    FontWeight = FontWeights.Bold
                });
            }

            if (_isKana)
            {
                Word.Inlines.Add(new LineBreak());
                Word.Inlines.Add(new Run($"「{v.Kana}」")
                {
                    FontSize = 18,
                    FontStyle = FontStyles.Italic,
                    FontWeight = FontWeights.DemiBold
                });
            }

            if (_isMean)
            {
                Word.Inlines.Add(new LineBreak());
                Word.Inlines.Add(new Run(v.Meaning)
                {
                    FontSize = 20,
                    FontWeight = FontWeights.DemiBold
                });
            }

            Topmost = true;
        }

        //private void InitializeData(string filePath)
        //{
        //    if (File.Exists(filePath))
        //    {
        //        _listString = File.ReadAllLines(filePath).ToList().Where(s => !string.IsNullOrWhiteSpace(s) && Regex.IsMatch(s, @".*\t.*\t.*") && !s.StartsWith("//")).ToArray();
        //    }
        //    else
        //    {
        //        Close();
        //    }
        //}


        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            Close();
        }

        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        private void Notifier_OnDeactivated(object sender, EventArgs e)
        {
            Topmost = true;
            IntPtr handle = new WindowInteropHelper(this).Handle;
            ShowWindow(handle, 4);
        }

       
    }
}
