using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using ProyectoFinal.Models;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Threading.Channels;
using System;
using System.Data.SqlClient;
using Dapper;
using System.Runtime.CompilerServices;

namespace ProyectoFinal.Rules
{
    public class PublicacionRule
    {
        private readonly IConfiguration _configuration;
        public PublicacionRule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Publicacion GetOnePostRandom() 
        {
            //var connectionString = @"Server=DESKTOP-2SE23BA\SQLEXPRESS;DataBase=BlogDataBase;Trusted_Connection=True";
            var connectionString = _configuration.GetConnectionString("BlogDataBase");

            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var post = connection.Query<Publicacion>("SELECT TOP 1 * FROM Publicacion ORDER BY NEWID()");
                return post.First();
            }
        }

        public List<Publicacion> GetPostsHome()
        {
            //var connectionString = @"Server=DESKTOP-2SE23BA\SQLEXPRESS;DataBase=BlogDataBase;Trusted_Connection=True";
            var connectionString = _configuration.GetConnectionString("BlogDataBase");

            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var post = connection.Query<Publicacion>("SELECT TOP 4 * FROM Publicacion ORDER BY Creacion DESC");
                return post.ToList();
            }
        }

        public Publicacion GetPostById(int id) {
            var connectionString = _configuration.GetConnectionString("BlogDataBase");

            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var query = "SELECT * FROM Publicacion WHERE Id = @id";
                var post = connection.QueryFirstOrDefault<Publicacion>(query, new { id });
                return post;
            }
        }

    }
}
