﻿namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Product entity in the business domain.
/// </summary>
public sealed class Product : DbEntity
{
    /// <summary>
    /// Represents the title of the product.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Represents the description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Represents the image URL (if any) of the product.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the Images (if any) of the product.
    /// </summary>
    public List<Image> Images { get; set; } = [];

    /// <summary>
    /// Represents the unique identifier of the category that the product is in.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Represents the status of whether the product is featured or not.
    /// </summary>
    public bool Featured { get; set; } = false;

    /// <summary>
    /// Represents the category that the product is in.
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// Represents a list of all of the variants of the product.
    /// </summary>
    public List<ProductVariant> Variants { get; set; } = [];

    /// <summary>
    /// Represents the status of whether the product should be visible for regular users or not.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Represents the status of whether or not the product is currently being edited. This
    /// property is not mapped/saved to the database, its only there for frontend purposes.
    /// </summary>
    [NotMapped]
    public bool Editing { get; set; } = false;

    /// <summary>
    /// Represents the status of whether this product is "being created" or not. This
    /// property is not mapped/saved to the database, its only there for frontend purposes.
    /// </summary>
    [NotMapped]
    public bool IsNew { get; set; } = false; 
}
