using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Invoice
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public DateTimeOffset? PaidAt { get; set; }
        public int CourseBySemesterEnrollId { get; set; }

        public virtual CourseBySemesterEnroll CourseBySemesterEnroll { get; set; }
    }
}
