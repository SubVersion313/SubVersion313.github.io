using System;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Method { get; set; }
    public string Status { get; set; }
    public DateTime PaidAt { get; set; }
}