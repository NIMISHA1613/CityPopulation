/*Author: Group 7*/

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Population1
{
    internal class VM : INotifyPropertyChanged
    {
        DB db = DB.Instance;
        List<City> cityList;

        private static VM vm;
        public static VM Instance { get { vm ??= new VM(); return vm; } }

        public BindingList<City> CityList { get; set; } = new BindingList<City>();

        private string selectedSort = "SortName";
        public string SelectedSort
        {
            get { return selectedSort; }
            set { selectedSort = value; propertyChanged(); UpdateCityList(); }
        }

        private string keyword;
        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; propertyChanged(); UpdateCityList(); }
        }

        private double? total;
        public double? Total
        {
            get { return total; }
            set { total = value; propertyChanged(); }
        }

        private double? highest;
        public double? Highest
        {
            get { return highest; }
            set { highest = value; propertyChanged(); }
        }

        private string highestCity;
        public string HighestCity
        {
            get { return highestCity; }
            set { highestCity = value; propertyChanged(); }
        }

        private City selectedCity;
        public City SelectedCity
        {
            get { return selectedCity; }
            set { selectedCity = value; propertyChanged(); }
        }

        private VM()
        {
            cityList = db.Get();
            UpdateCityList();
        }

        public bool saveSuccessfully = false;
        public bool Save(City city)
        {
            if (city.IsNew) // if adding
            {
                saveSuccessfully = db.Insert(city);
            }
            else // if editing
            {
                saveSuccessfully = db.Update(city);
                if (saveSuccessfully)
                {
                    cityList.Remove(SelectedCity);
                    SelectedCity = null;
                }
            }
            if (saveSuccessfully)
            {
                cityList.Add(city);
                UpdateCityList();
            }
            return saveSuccessfully;
        }

        public bool deleteSuccessfully = false;
        public bool Delete()
        {
            if (db.Delete(SelectedCity))
            {
                SelectedCity = null;
                cityList = db.Get();
                UpdateCityList();
                deleteSuccessfully = true;
            }
            return deleteSuccessfully;
        }

        public void UpdateCityList()
        {
            //Sort Listbox according to user's choice
            switch (SelectedSort)
            {
                case "SortName": cityList = cityList.OrderBy(city => city.Name).ToList(); break;
                case "SortNameDesc": cityList = cityList.OrderByDescending(city => city.Name).ToList(); break;
                case "SortPopulation": cityList = cityList.OrderBy(city => city.Population).ToList(); break;
                case "SortPopulationDesc": cityList = cityList.OrderByDescending(city => city.Population).ToList(); break;
            }
            //Fetch CityList to display on UI
            CityList.Clear();
            foreach (City city in cityList)
            {
                //If user specify search keyword
                if (keyword != null)
                {
                    if (city.Name.ToLower().Contains(keyword.ToLower()))
                        CityList.Add(city);
                }
                //If user doesn't specify search keyword
                else CityList.Add(city);
            }
            //Get total population
            Total = cityList.Sum(cityList => cityList.Population);
            //Get highest population
            Highest = cityList.Max(cityList => cityList.Population);
            for (int i = 0; i < cityList.Count; i++)
                if (cityList[i].Population == Highest) HighestCity = cityList[i].Name;
        }

        #region property changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void propertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
