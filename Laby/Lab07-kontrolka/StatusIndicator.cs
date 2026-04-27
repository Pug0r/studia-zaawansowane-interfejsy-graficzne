using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Lab07_kontrolka
{
    public class StatusIndicator : Control
    {
        static StatusIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusIndicator),
                new FrameworkPropertyMetadata(typeof(StatusIndicator)));
        }

        public static readonly DependencyProperty StatusColorProperty =
            DependencyProperty.Register("StatusColor", typeof(Brush), typeof(StatusIndicator),
                new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty IsActivatedProperty =
            DependencyProperty.Register("IsActivated", typeof(bool), typeof(StatusIndicator),
                new PropertyMetadata(true));
        public bool IsActivated
        {
            get => (bool)GetValue(IsActivatedProperty);
            set => SetValue(IsActivatedProperty, value);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            IsActivated = !IsActivated;
        }

        public Brush StatusColor
        {
            get => (Brush)GetValue(StatusColorProperty);
            set => SetValue(StatusColorProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(StatusIndicator),
                new PropertyMetadata("Offline"));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}