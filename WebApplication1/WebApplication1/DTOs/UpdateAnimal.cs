using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class UpdateAnimal
{
    [MaxLength(200)]
    public string name { get; set; }
    
    [MaxLength(200)]
    public string? description { get; set; }
    
    [MaxLength(200)]
    public string category { get; set; }
    
    [MaxLength(200)]
    public string area { get; set; }
}