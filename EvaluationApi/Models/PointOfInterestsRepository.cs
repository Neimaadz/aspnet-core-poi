using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EvaluationApi.Models
{
    public class PointOfInterestsRepository : IPointOfInterestsRepository
    {
        public string ConnectionString { get; set; }

        public PointOfInterestsRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("Default");
        }

        public List<PointOfInterestItem> GetAll()
        {
            //je me connecte à la bdd
            MySqlConnection cnn = new MySqlConnection(ConnectionString);

            cnn.Open();
            //Je crée une requête sql
            string sql = @"
                SELECT 
                    *
                FROM 
                    point_of_interest
                ";

            //Executer la requête sql, donc créer une commande
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            var reader = cmd.ExecuteReader();
            var maListe = new List<PointOfInterestItem>();

            //Récupérer le retour, et le transformer en objet
            while (reader.Read())
            {
                var pointOfInterestItem = new PointOfInterestItem()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    name = reader["name"].ToString(),
                    imagePath = reader["imagePath"].ToString(),
                    comment = reader["comment"].ToString(),
                    localization = reader["localization"].ToString()
                };

                maListe.Add(pointOfInterestItem);
            }

            cnn.Close();
            return maListe;
        }
    }
}
