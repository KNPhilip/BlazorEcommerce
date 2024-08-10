namespace Domain.Models;

public abstract class DbEntity
{
    private int id;

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

    public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public virtual bool IsSoftDeleted { get; set; }
}
