using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Potega();
            ZapiszWTablicy();
            PoleKola();
            SumaCyfr();
            ZamienElementy();
        }


        public void SetResult(TextBox targetTextBox, string message, bool isError)
        {
            targetTextBox.Text = message;
            targetTextBox.Foreground = isError ? Brushes.Red : Brushes.Green;
        }

        private void Potega()
        {
            try
            {
                int arg1 = int.Parse(PotegaInputArg1.Text);
                int arg2 = int.Parse(PotegaInputArg2.Text);
                int arg3 = int.Parse(PotegaInputArg3.Text);
                var result = Methods.potega(arg1, arg2, arg3);
                SetResult(PotegaResult, result.ToString(), isError: false);
            }
            catch (Exception ex)
            {
                SetResult(PotegaResult, ex.Message, isError: true);
            }
        }
        private void ZapiszWTablicy()
        {
            try
            {
                int[] arg1 = zapiszWTablicyArg1.Text.Split(',').Select(a => int.Parse(a.Trim())).ToArray();
                int arg2 = int.Parse(zapiszWTablicyArg2.Text);
                var result = Methods.zapiszWTablicy(arg1, arg2);
                string resultString = string.Join(", ", result);
                SetResult(zapiszWTablicyResult, resultString, isError: false);
            }
            catch (Exception ex)
            {
                SetResult(zapiszWTablicyResult, ex.Message, isError: true);
            }
        }

        private void PoleKola()
        {
            try
            {
                var arg1 = double.Parse(poleKolaArg1.Text);
                var result = Methods.poleKola(arg1);
                string resultString = result.ToString();
                SetResult(poleKolaResult, resultString, isError: false);
            }
            catch (Exception ex)
            {
                SetResult(poleKolaResult, ex.Message, isError: true);
            }
        }

        private void SumaCyfr()
        {
            try
            {
                var arg1 = double.Parse(sumaCyfrArg1.Text);
                var result = Methods.sumaCyfr(arg1);
                string resultString = result.ToString();
                SetResult(sumaCyfrResult, resultString, isError: false);
            }
            catch (Exception ex)
            {
                SetResult(sumaCyfrResult, ex.Message, isError: true);
            }
        }

        private void ZamienElementy()
        {
            try
            {
                var arg1 = zamienElementyArg1.Text.Split(',').ToArray();
                var arg2 = int.Parse(zamienElementyArg2.Text);
                var arg3 = int.Parse(zamienElementyArg3.Text);
                var arg4 = int.Parse(zamienElementyArg4.Text);

                var result = Methods.zamienElementy(arg1, arg2, arg3, arg4);
                string resultString = string.Join(", ", result);
                SetResult(zamienElementyResult, resultString, isError: false);
            }
            catch (Exception ex)
            {
                SetResult(zamienElementyResult, ex.Message, isError: true);
            }
        }
    }
}