namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Classes inheriting from this class will have some basic properties for database purposes.
/// This would be the base class for all entities in the system. The properties are
/// an unique identifier, a date created and a flag to indicate if the entity is soft deleted.
/// </summary>
public abstract class DbEntity
{
    private int id;

    /// <summary>
    /// Represents the unique identifier of a database entity.
    /// This id is an integer, and must be greater than 0.
    /// </summary>
    public virtual int Id 
    {
        get => id;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The Id must be greater than 0.");
            }

            id = value;
        }
    }

    /// <summary>
    /// Represents the date that the entity was created.
    /// </summary>
    public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Represents the status of whether the DB entity is "soft" deleted or not. On deletion the
    /// data of old entities is still saved for a period of time in the database. This is a common approach
    /// in companies for legal reasons. The data is then deleted after x amount of time.
    /// </summary>
    public virtual bool IsSoftDeleted { get; set; }
}
