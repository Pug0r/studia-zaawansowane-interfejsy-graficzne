using System;
using System.IO;
using System.Speech.Synthesis;
using System.Windows;
using Microsoft.Win32;

namespace Lab09_speech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechSynthesizer _synth = new();
        private bool _isPlaying;

        public MainWindow()
        {
            InitializeComponent();
            _synth.SpeakCompleted += (_, _) => Dispatcher.Invoke(() =>
            {
                _isPlaying = false;
                UpdatePlayStop();
            });
            UpdatePlayStop();
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
            UpdatePlayStop();
        }

        private void PlayStop_Click(object sender, RoutedEventArgs e)
        {
            var text = TextContent.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if (_isPlaying)
            {
                _synth.SpeakAsyncCancelAll();
                _isPlaying = false;
            }
            else
            {
                _synth.SpeakAsync(text);
                _isPlaying = true;
            }

            UpdatePlayStop();
        }

        protected override void OnClosed(EventArgs e)
        {
            _synth.Dispose();
            base.OnClosed(e);
        }

        private void UpdatePlayStop()
        {
            PlayStopButton.IsEnabled = !string.IsNullOrWhiteSpace(TextContent.Text);
            PlayStopButton.Content = _isPlaying ? "Stop" : "Play";
        }
    }
}