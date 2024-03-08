namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Category entity in the business domain.
/// </summary>
public sealed class Category
{
    /// <summary>
    /// Represents the unique identifier for the category.
    /// </summary>
    public int Id { get; set; }

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
    /// Represents the status of whether the category is deleted or not. On deletion the data of
    /// old categories is still saved for a period of time in the database (This is a common
    /// approach in most companies because of legal reasons) - The data is then deleted
    /// after x amount of years.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

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
