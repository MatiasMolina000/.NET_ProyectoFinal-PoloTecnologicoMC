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

namespace ProyectoFinal.Rules
{
    public class PublicacionRule
    {
        public Publicacion GetOnePostRandom() 
        {
            var connectionString = @"Server=DESKTOP-2SE23BA\SQLEXPRESS;DataBase=BlogDataBase;Trusted_Connection=True";

            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var post = connection.Query<Publicacion>("SELECT TOP 1 * FROM Publicacion");
                return post.First();
            }
        }

    }
}
