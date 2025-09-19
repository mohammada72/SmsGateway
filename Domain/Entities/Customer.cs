using Domain.Common;

namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Account Account { get; set; } = new();
    public Email Email { get; set; } = string.Empty;
    public NationalId NationalId { get; set; } = string.Empty;

}
