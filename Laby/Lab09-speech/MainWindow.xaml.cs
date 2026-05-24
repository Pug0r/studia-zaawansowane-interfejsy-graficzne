using Microsoft.Win32;
using System;
using System.IO;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Input;

namespace Lab09_speech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechSynthesizer _synth = new();
        private bool _isPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
            _synth.SpeakCompleted += (_, _) => Dispatcher.Invoke(() =>
            {
                _isPlaying = false;
            });
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            TextContent.Text = File.ReadAllText(dialog.FileName);
            _synth.SpeakAsyncCancelAll();
            _isPlaying = false;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var text = TextContent.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            var startIndex = TextContent.SelectionStart;
            if (startIndex < 0 || startIndex >= text.Length)
            {
                startIndex = 0;
            }

            var fragment = text[startIndex..];
            if (string.IsNullOrWhiteSpace(fragment))
            {
                return;
            }

            _synth.SpeakAsyncCancelAll();
            _isPlaying = true;
            _synth.SpeakAsync(fragment);
            UpdatePauseButton();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                _synth.Pause();
                _isPlaying = false;
                PauseButton.Content = "Continue";
            }
            else
            {
                _synth.Resume();
                _isPlaying = true;
                PauseButton.Content = "Pause";
            }
        }

        private void UpdatePauseButton()
        {

            PauseButton.IsEnabled = _isPlaying;
        }

        protected override void OnClosed(EventArgs e)
        {
            _synth.Dispose();
            base.OnClosed(e);
        }
    }
}