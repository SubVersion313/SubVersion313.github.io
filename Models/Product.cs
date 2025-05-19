using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    [Required]
    public string Category { get; set; }
    public string Details { get; set; } // JSON
    public int? SellerId { get; set; }
    [ForeignKey("SellerId")]
    public User Seller { get; set; }
    public DateTime CreatedAt { get; set; }
}