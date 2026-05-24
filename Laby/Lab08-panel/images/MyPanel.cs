using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Lab08_panel
{
    public class MyPanel : Panel
    {
        private Button openButton;
        private Grid imageOverlay;
        private Image displayImage;
        private Button closeButton;

        public MyPanel()
        {
            openButton = new Button { Content = "View Image", Width = 120, Height = 40 };
            openButton.Click += (s, e) => ToggleView(true);

            imageOverlay = new Grid { Visibility = Visibility.Collapsed };

            displayImage = new Image
            {
                Source = new BitmapImage(new Uri("images/img.jpg", UriKind.Relative)),
                Stretch = Stretch.Uniform
            };

            closeButton = new Button
            {
                Content = "X",
                Width = 25,
                Height = 25,
                Background = Brushes.Firebrick,
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            closeButton.Click += (s, e) => ToggleView(false);

            imageOverlay.Children.Add(displayImage);
            imageOverlay.Children.Add(closeButton);

            InternalChildren.Add(openButton);
            InternalChildren.Add(imageOverlay);
        }

        private void ToggleView(bool showImage)
        {
            if (showImage)
            {
                openButton.Visibility = Visibility.Collapsed;
                imageOverlay.Visibility = Visibility.Visible;
            }
            else
            {
                openButton.Visibility = Visibility.Visible;
                imageOverlay.Visibility = Visibility.Collapsed;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
            }
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect centerRect = new Rect(0, 0, finalSize.Width, finalSize.Height);

            foreach (UIElement child in InternalChildren)
            {
                if (child is Button btn && btn == openButton)
                {
                    double x = (finalSize.Width - btn.DesiredSize.Width) / 2;
                    double y = (finalSize.Height - btn.DesiredSize.Height) / 2;
                    btn.Arrange(new Rect(x, y, btn.DesiredSize.Width, btn.DesiredSize.Height));
                }
                else
                {
                    child.Arrange(centerRect);
                }
            }

            return finalSize;
        }
    }
}