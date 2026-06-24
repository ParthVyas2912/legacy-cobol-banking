using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class InterestCalculation
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } = string.Empty; // PIC X(10)

            [Column(TypeName = "decimal(15,2)")]
            public decimal PrincipalAmount { get; set; } = 0m; // PIC 9(13)V99 COMP-3

            [Column(TypeName = "decimal(5,2)")]
            public decimal InterestRate { get; set; } = 0m; // PIC 9(3)V99 COMP-3

            [Column(TypeName = "decimal(15,2)")]
            public decimal InterestAmount { get; set; } = 0m; // PIC 9(13)V99 COMP-3

            [MaxLength(8)]
            public string CalculationDate { get; set; } = string.Empty; // PIC X(8)

            public CalculationStatus Status { get; set; } = CalculationStatus.Pending; // 88-level equivalent

            public List<InterestDetail> InterestDetails { get; set; } = new List<InterestDetail>(); // OCCURS clause

            public class InterestDetail
            {
                [MaxLength(10)]
                public string DetailId { get; set; } = string.Empty; // PIC X(10)

                [Column(TypeName = "decimal(15,2)")]
                public decimal DetailAmount { get; set; } = 0m; // PIC 9(13)V99 COMP-3
            }
        }

        public enum CalculationStatus
        {
            Pending = 0, // VALUE 'P'
            Completed = 1, // VALUE 'C'
            Failed = 2 // VALUE 'F'
        }
    }
}