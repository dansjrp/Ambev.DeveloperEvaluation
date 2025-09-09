
using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Representa um produto disponível na plataforma.
/// </summary>
public class Product : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public Rating Rating { get; set; } = new Rating();
}

/// <summary>
/// Representa a avaliação de um produto.
/// </summary>
public class Rating
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
