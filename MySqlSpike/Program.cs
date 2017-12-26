using System;
using System.IO;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MySqlSpike
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            var host = configuration["mysql:host"];
            var user = configuration["mysql:username"];
            var password = configuration["mysql:password"];
            var database = configuration["mysql:database"];


            var connectionString = $"host={host};port=3306;user id={user};password={password};database={database};";
            using (var connection = new MySqlConnection(connectionString))
            {
                var customers = connection.Query<MySqlCustomer>("SELECT user_name, facebook_id FROM ordrcoffee.customers;");
                foreach (var mySqlCustomer in customers)
                {
                    Console.WriteLine($"customer: {mySqlCustomer.user_name}, {mySqlCustomer.facebook_id}");
                }
            }

            Console.WriteLine("Hello World!");
        }
    }

    internal class MySqlCustomer
    {
        public string user_name { get; set; }
        public string facebook_id { get; set; }
    }

    public class AppDb : IDisposable
    {
        public readonly MySqlConnection Connection;

        public AppDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}