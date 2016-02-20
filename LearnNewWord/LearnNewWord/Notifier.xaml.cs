using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        private string[] _listString;
        private DispatcherTimer _timer;
        private int _currentN;
        public Notifier()
        {
            InitializeComponent();
        }

        public Notifier(string filePath, int timeToNext, bool isWord, bool isKana, bool isMean)
        {
            _isWord = isWord;
            _isKana = isKana;
            _isMean = isMean;
            InitializeComponent();
            InitializeData(filePath);
            StartTimer(timeToNext);
        }

        private void StartTimer(int timeToNext)
        {
            _timer = new DispatcherTimer();
            _timer.Tick += TimerOnTick;
            _timer.Interval = new TimeSpan(0,0,0,0,timeToNext);
            _timer.Start();
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            Word.Text = string.Empty;
            if (this.WindowState != WindowState.Normal)
            {
                Topmost = true;
                IntPtr handle = new WindowInteropHelper(this).Handle;
                ShowWindow(handle, 4);
            }
            //get next word
            Random random = new Random();
            if (_currentN == 0)
            {
                _currentN = _listString.Length;
            }
            int t = random.Next(_currentN);
            string s = _listString[t];
            _listString[t] = _listString[_currentN - 1];
            _listString[_currentN - 1] = s;
            _currentN--;
            //show next word
            if (string.IsNullOrWhiteSpace(s)) 
            {
                return;
            }
            var arr = s.Split('\t');
            var word = arr[0];
            var kana = arr[1];
            var mean = arr[2];
            
            if (_isWord)
            {
                Word.Text = word;
            }

            if (_isKana)
            {
                Word.Inlines.Add(new LineBreak());
                Word.Inlines.Add(new Run($"「{kana}」")
                {
                    FontSize = 18,
                    FontStyle = FontStyles.Italic
                });
            }

            if (_isMean)
            {
                Word.Inlines.Add(new LineBreak());
                Word.Inlines.Add(new Run(mean)
                {
                    FontSize = 20,
                    FontWeight = FontWeights.DemiBold
                });
            }
        }

        private void InitializeData(string filePath)
        {
            if (File.Exists(filePath))
            {
                _listString = System.IO.File.ReadAllLines(filePath).ToList().Where(s => !string.IsNullOrWhiteSpace(s) && Regex.IsMatch(s, @".*\t.*\t.*")).ToArray();
                _currentN = _listString.Length;
            }
            else
            {
                Close();
            }
        }


        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            this.Close();
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
