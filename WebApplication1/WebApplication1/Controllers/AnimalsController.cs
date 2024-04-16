using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration iConfiguration)
    {
        _configuration = iConfiguration;
    }

    [HttpGet]
    public IActionResult GetAnimals()
    {
        SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * from Animal";

        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();
        int idAnimalOrdinal = reader.GetOrdinal("idAnimal");
        int nameOrdinal = reader.GetOrdinal("name");

        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                idAnimal = reader.GetInt32(idAnimalOrdinal),
                name = reader.GetString(nameOrdinal)
                
            });
        }
        
        return Ok(animals);
    }



}