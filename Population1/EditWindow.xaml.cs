/*Author: Group 7*/

using System.Windows;

namespace Population1
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        readonly VM vm;
        readonly City city = new City();

        public EditWindow(bool isEdit)
        {
            InitializeComponent();
            vm = VM.Instance;
            DataContext = city;

            if (isEdit)
            {
                city.Name = vm.SelectedCity.Name;
                city.Population = vm.SelectedCity.Population;
                city.IsNew = false;
                NameTextbox.IsEnabled = false;
            }
            else
            {
                Title = "Add";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //If Adding
            if (city.IsNew)
            {
                //Validate name not blank
                if (city.Name == null) MessageBox.Show("Name can not be blank");
                else
                {
                    //Check if city name already exists
                    bool keyExist = false;
                    for (int i = 0; i < vm.CityList.Count; i++)
                    {
                        if (city.Name.ToLower() == vm.CityList[i].Name.ToLower())
                        {
                            keyExist = true;
                            MessageBox.Show("City name already exists");
                            break;
                        }
                    }
                    //If city name not exists yet
                    if (!keyExist)
                    {
                        ValidateNumber();
                    }
                }
            }
            //If Editting
            else ValidateNumber(); ;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Validate number and call SaveData()
        private void ValidateNumber()
        {
            if (!double.TryParse(PopulationTxt.Text, out double population) || population < 0)
            {
                MessageBox.Show("Please input a positive number");
            }
            else SaveData();
        }

        //Call VM function to save data to DB then show message informing result
        private void SaveData()
        {
            vm.Save(city);
            MessageBox.Show(vm.saveSuccessfully ? "Saved!" : "Something went wrong!");
            Close();
        }
    }
}
