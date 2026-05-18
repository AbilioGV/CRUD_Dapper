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

        [HttpGet]  
        public IActionResult Edit(int pessoaid)
        {
            IDbConnection con;

            try
            {
                string selecaoQuery = "SELECT * FROM pessoas WHERE PessoaId = @pessoaid";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                Pessoas pessoas = con.Query<Pessoas>(selecaoQuery, new { pessoaid = pessoaid }).FirstOrDefault();
                const.Close();
                return View(pessoas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Edit(int pessoaid, Pessoas pessoas)
        {
            if (pessoaid != pessoas.PessoaId)
                return NotFound();
            
            if (ModelState.IsValid)
            {
                IDbConnection con;

                try
                {
                    string atualizarQuery = "UPDATE pessoas SET nome = @nome, idade = @idade, peso = @peso WHERE PessoaId = @pessoaid";
                    con = new NpgsqlConnection(ConnectionString);
                    con.Open();
                    con.Execute(atualizarQuery, pessoas);
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
        [HttpPost]
        public IActionResult Delete(int pessoaid)
        {
            IDbConnection con;
            try
            {
                string excluirQuery = "DELETE FROM pessoas WHERE PessoaId = @pessoaid";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                con.Execute(excluirQuery, new { pessoaid = pessoaid });
                con.Close();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
