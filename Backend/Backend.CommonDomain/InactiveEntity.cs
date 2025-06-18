namespace Backend.CommonDomain
{
  public class InactiveEntity
  {
    public DateTime? Inactive { get; set; } = null;
    public DateTime? Deleted { get; set; } = null;

    public void setInactive()
    {
      this.Inactive = DateTime.UtcNow;
    }
    public void setDeleted()
    { 
      this.Deleted = DateTime.UtcNow;
    }
  }
}

