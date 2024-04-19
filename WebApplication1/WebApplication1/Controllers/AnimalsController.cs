using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication1.DTOs;
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
    public IActionResult GetAnimals(String orderBy = "Name")
    {
        string[] allowedColumns = { "idAnimal", "name", "description", "area" };

        if (!allowedColumns.Contains(orderBy.ToLower()))
        {
            orderBy = "name";
        }


        SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * from Animal" +
                              " ORDER BY " + orderBy;

        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("idAnimal");
        int nameOrdinal = reader.GetOrdinal("name");
        int descOrdinal = reader.GetOrdinal("decription");
        int categoryOrdinal = reader.GetOrdinal("category");
        int areaOrdinal = reader.GetOrdinal("area");

        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                idAnimal = reader.GetInt32(idAnimalOrdinal),
                name = reader.GetString(nameOrdinal),
                description = reader.GetString(descOrdinal),
                category = reader.GetString(categoryOrdinal),
                area = reader.GetString(areaOrdinal)
            });
        }

        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal addAnimal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using //definicja commanda
            SqlCommand command = new SqlCommand();
        command.Connection = connection;

        command.CommandText = "INSERT INTO Animal VALUES (@animalName,@animalDesc,@animalCategory,@animalArea)";

        //ustawianie parametrow
        command.Parameters.AddWithValue("@animalName", addAnimal.name);
        command.Parameters.AddWithValue("@animalDesc", addAnimal.description ?? null);
        command.Parameters.AddWithValue("@animalCategory", addAnimal.category ?? "");
        command.Parameters.AddWithValue("@animalArea", addAnimal.area ?? "");


        command.ExecuteNonQuery();

        return Created("", null);
    }



}