﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.Relational
{
    public class Feedback : BaseModel
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public Report Report { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime Creation { get; set; }
        public DateTime? LastSent { get; set; }
        public int TryCount { get; set; }
        public int? Rating { get; set; }
    }
}
