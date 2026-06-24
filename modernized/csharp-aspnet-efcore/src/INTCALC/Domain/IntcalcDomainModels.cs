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
            [Column(TypeName = "decimal(15, 2)")]
            public decimal PrincipalAmount { get; set; } // PIC S9(13)V99 COMP-3

            [Column(TypeName = "decimal(5, 2)")]
            public decimal InterestRate { get; set; } // PIC S9(3)V99 COMP-3

            [Column(TypeName = "decimal(15, 2)")]
            public decimal InterestAmount { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(10)]
            public string AccountNumber { get; set; } = string.Empty; // PIC X(10)

            [MaxLength(8)]
            public string CalculationDate { get; set; } = string.Empty; // PIC X(8)

            [MaxLength(1)]
            public string CalculationType { get; set; } = string.Empty; // PIC X(1)

            public CalculationTypeEnum CalculationTypeEnum
            {
                get => Enum.Parse<CalculationTypeEnum>(CalculationType);
                set => CalculationType = value.ToString();
            }

            public List<InterestDetail> InterestDetails { get; set; } = new List<InterestDetail>();

            public class InterestDetail
            {
                [MaxLength(10)]
                public string TransactionId { get; set; } = string.Empty; // PIC X(10)

                [Column(TypeName = "decimal(15, 2)")]
                public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

                [MaxLength(8)]
                public string TransactionDate { get; set; } = string.Empty; // PIC X(8)
            }
        }

        public enum CalculationTypeEnum
        {
            [Display(Name = "Simple")]
            S, // Value 'S'

            [Display(Name = "Compound")]
            C // Value 'C'
        }
    }
}