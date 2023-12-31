﻿
using Domain.Models.Base;

namespace Domain.Models
{
    public class Scheduling : BaseModel
    {
        public int SchedulingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public int ProcedureId { get; set; }
        public int SalonId { get; set; }
        public int PaymentId { get; set; }

        public virtual required Customer Customer { get; set; }
        public virtual required Employee Employee { get; set; }
        public virtual required Procedure Procedure { get; set; }
        public virtual required Salon Salon { get; set; }
        public virtual required Payment Payment { get; set; }
    }
}
