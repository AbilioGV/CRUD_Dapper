using CRUD_Dapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace CRUD_Dapper.Controllers
{
    public class PessoasController : Controller
    {
        private readonly string ConnectionString =
            "User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=PessoasDB;";

        public IActionResult Index()
        {
            IDbConnection con;

            try
            {
                string selecaoQuery = "SELECT * FROM pessoas";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                IEnumerable<Pessoas> listaPessoas = con.Query<Pessoas>(selecaoQuery).ToList();
                return View(listaPessoas);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pessoas pessoas)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    string insercaoQuery =
                        "INSERT INTO pessoas (nome, idade, peso) VALUES (@nome, @idade, @peso)";

                    using IDbConnection con = new NpgsqlConnection(ConnectionString);

                    con.Open();
                    con.Execute(insercaoQuery, pessoas);
                    con.Close();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

                return View(pessoas);
           
        }
    }
}