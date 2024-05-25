using System;
using DataAccess.Models;

public enum UpdateRequestStatus
{
    Pending,
    Approved,
    Rejected
}

public class UpdateRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FieldName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public UpdateRequestStatus Status { get; set; }
    public string AdminComment { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }

    public virtual User User { get; set; }
}