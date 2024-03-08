namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Category entity in the business domain.
/// </summary>
public sealed class Category : DbEntity
{
    /// <summary>
    /// Represents the name of the category.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents the URL for the category.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Represents the status of whether the category
    /// should be visible for regular users or not.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Represents the status of whether or not the category is currently being edited. This
    /// property is not mapped/saved to the database, its only there for frontend purposes.
    /// </summary>
    [NotMapped]
    public bool Editing { get; set; } = false;

    /// <summary>
    /// Represents the status of whether this category is "being created" or not. This
    /// property is not mapped/saved to the database, its only there for frontend purposes.
    /// </summary>
    [NotMapped]
    public bool IsNew { get; set; } = false; 
}
