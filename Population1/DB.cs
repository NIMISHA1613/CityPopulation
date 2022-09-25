/*Author: Group 7*/

using System.Collections.Generic;
using System.Data.SqlClient;

namespace Population1
{
    internal class DB
    {
        const string CONNECTION_STRING = @"Server=.\SQLEXPRESS;Database=PopulationDB1;Trusted_Connection=True;";
        const string SELECT_COMMAND = "SELECT City, PopulationCount FROM PopulationTable";
        const string INSERT_COMMAND = "INSERT INTO PopulationTable (City, PopulationCount) VALUES (@Name, @Population)";
        const string UPDATE_COMMAND = "UPDATE PopulationTable SET PopulationCount = @Population WHERE City = @Name";
        const string DELETE_COMMAND = "DELETE FROM PopulationTable WHERE City = @Name";

        private SqlConnection conn;

        private static DB db;
        public static DB Instance { get { db ??= new DB(); return db; } }

        private DB()
        {
            conn = new SqlConnection(CONNECTION_STRING);
            conn.Open();
        }

        public bool Insert(City city)
        {
            using SqlCommand cmd = new SqlCommand(INSERT_COMMAND, conn);
            cmd.Parameters.AddWithValue("@Name", city.Name);
            cmd.Parameters.AddWithValue("@Population", city.Population);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(City city)
        {
            using SqlCommand cmd = new SqlCommand(UPDATE_COMMAND, conn);
            cmd.Parameters.AddWithValue("@Name", city.Name);
            cmd.Parameters.AddWithValue("@Population", city.Population);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(City city)
        {
            using SqlCommand cmd = new SqlCommand(DELETE_COMMAND, conn);
            cmd.Parameters.AddWithValue("@Name", city.Name);
            return cmd.ExecuteNonQuery() > 0;
        }

        public List<City> Get()
        {
            List<City> cityList = new List<City>();

            using SqlCommand cmd = new SqlCommand(SELECT_COMMAND, conn);
            using SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
                cityList.Add(new City
                {
                    Name = dr.GetString(dr.GetOrdinal("city")),
                    Population = dr.IsDBNull(dr.GetOrdinal("PopulationCount")) ? null : (double?)dr.GetDouble(dr.GetOrdinal("PopulationCount")),
                    IsNew = false
                });
            dr.Close();

            return cityList;
        }
    }
}
