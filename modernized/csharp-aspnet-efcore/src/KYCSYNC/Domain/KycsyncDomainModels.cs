using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class Customer
        {
            [MaxLength(10)] // PIC X(10)
            public string CustomerId { get; set; } = string.Empty;

            [MaxLength(30)] // PIC X(30)
            public string FirstName { get; set; } = string.Empty;

            [MaxLength(30)] // PIC X(30)
            public string LastName { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string Gender { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string DateOfBirth { get; set; } = string.Empty;

            public Address CustomerAddress { get; set; } = new Address();

            [MaxLength(15)] // PIC X(15)
            public string PhoneNumber { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string Email { get; set; } = string.Empty;

            public KycStatusEnum KycStatus { get; set; } = KycStatusEnum.NotVerified;

            public List<Document> Documents { get; set; } = new List<Document>();
        }

        public class Address
        {
            [MaxLength(50)] // PIC X(50)
            public string Street { get; set; } = string.Empty;

            [MaxLength(30)] // PIC X(30)
            public string City { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string State { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string PostalCode { get; set; } = string.Empty;

            [MaxLength(30)] // PIC X(30)
            public string Country { get; set; } = string.Empty;
        }

        public class Document
        {
            [MaxLength(15)] // PIC X(15)
            public string DocumentId { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string DocumentType { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string DocumentName { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string IssueDate { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string ExpiryDate { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string VerificationStatus { get; set; } = string.Empty;
        }

        public enum KycStatusEnum
        {
            NotVerified = 0,
            Pending = 1,
            Verified = 2,
            Rejected = 3
        }
    }
}