using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        public class FraudCheckRecord
        {
            [MaxLength(10)] // PIC X(10)
            public string RecordId { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string CustomerName { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15,2)")] // PIC S9(13)V99 COMP-3
            public decimal TransactionAmount { get; set; } = 0m;

            [MaxLength(3)] // PIC X(3)
            public string CurrencyCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string FraudIndicator { get; set; } = string.Empty;

            public FraudIndicatorEnum FraudIndicatorEnum
            {
                get => Enum.TryParse<FraudIndicatorEnum>(FraudIndicator, out var result) ? result : FraudIndicatorEnum.Unknown;
                set => FraudIndicator = value.ToString();
            }

            [MaxLength(8)] // PIC X(8)
            public string TransactionDate { get; set; } = string.Empty;

            [MaxLength(6)] // PIC X(6)
            public string TransactionTime { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string MerchantName { get; set; } = string.Empty;

            [MaxLength(30)] // PIC X(30)
            public string MerchantCategory { get; set; } = string.Empty;

            [Column(TypeName = "decimal(10,2)")] // PIC S9(8)V99 COMP-3
            public decimal MerchantRating { get; set; } = 0m;

            public List<FraudCheckDetail> FraudCheckDetails { get; set; } = new List<FraudCheckDetail>();
        }

        public class FraudCheckDetail
        {
            [MaxLength(10)] // PIC X(10)
            public string DetailId { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string Description { get; set; } = string.Empty;

            [Column(TypeName = "decimal(10,2)")] // PIC S9(8)V99 COMP-3
            public decimal Amount { get; set; } = 0m;

            [MaxLength(1)] // PIC X(1)
            public string Status { get; set; } = string.Empty;

            public StatusEnum StatusEnum
            {
                get => Enum.TryParse<StatusEnum>(Status, out var result) ? result : StatusEnum.Unknown;
                set => Status = value.ToString();
            }
        }

        public enum FraudIndicatorEnum
        {
            Unknown = 0,
            [Display(Name = "No Fraud")]
            N,
            [Display(Name = "Potential Fraud")]
            P,
            [Display(Name = "Confirmed Fraud")]
            C
        }

        public enum StatusEnum
        {
            Unknown = 0,
            [Display(Name = "Pending")]
            P,
            [Display(Name = "Approved")]
            A,
            [Display(Name = "Rejected")]
            R
        }
    }
}