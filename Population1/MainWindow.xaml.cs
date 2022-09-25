/*Author: Group 7*/

using System.Windows;

namespace Population1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VM vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = VM.Instance;
            DataContext = vm;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EditWindow ew = new EditWindow(false) { Owner = this };
            ew.ShowDialog();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedCity != null)
            {
                EditWindow ew = new EditWindow(true) { Owner = this };
                ew.ShowDialog();
            }
            else
                MessageBox.Show("Please select a city to edit");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            vm.Delete();
        }

        private void Keyword_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            vm.UpdateCityList();
        }
    }
}
