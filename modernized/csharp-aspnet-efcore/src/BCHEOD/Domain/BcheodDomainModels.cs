using System;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class Policy
        {
            // PIC X(10)
            [MaxLength(10)]
            public string PolicyNumber { get; set; }

            // PIC 9(8)
            public int PolicyDate { get; set; }

            // PIC S9(13)V99 COMP-3
            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal PolicyAmount { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string PolicyHolderName { get; set; }

            // PIC X(15)
            [MaxLength(15)]
            public string PolicyType { get; set; }

            // PIC 9(5)
            [Range(0, 99999)]
            public int PolicyTerm { get; set; }

            // PIC 9(3)
            [Range(0, 999)]
            public int PolicyStatus { get; set; }

            public PolicyDetails Details { get; set; }

            public Policy()
            {
                Details = new PolicyDetails();
            }
        }

        public class PolicyDetails
        {
            // PIC 9(10)
            [Range(0, 9999999999)]
            public long PolicyId { get; set; }

            // PIC X(20)
            [MaxLength(20)]
            public string BeneficiaryName { get; set; }

            // PIC 9(8)
            [Range(0, 99999999)]
            public int BeneficiaryDateOfBirth { get; set; }

            // PIC S9(13)V99 COMP-3
            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal BeneficiaryAmount { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string BeneficiaryAddress { get; set; }
        }

        public class Claim
        {
            // PIC 9(10)
            [Range(0, 9999999999)]
            public long ClaimId { get; set; }

            // PIC X(10)
            [MaxLength(10)]
            public string ClaimNumber { get; set; }

            // PIC 9(8)
            [Range(0, 99999999)]
            public int ClaimDate { get; set; }

            // PIC S9(13)V99 COMP-3
            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal ClaimAmount { get; set; }

            // PIC X(20)
            [MaxLength(20)]
            public string ClaimStatus { get; set; }

            public ClaimDetails Details { get; set; }

            public Claim()
            {
                Details = new ClaimDetails();
            }
        }

        public class ClaimDetails
        {
            // PIC 9(10)
            [Range(0, 9999999999)]
            public long ClaimantId { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string ClaimantName { get; set; }

            // PIC 9(8)
            [Range(0, 99999999)]
            public int ClaimantDateOfBirth { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string ClaimantAddress { get; set; }
        }
    }
}