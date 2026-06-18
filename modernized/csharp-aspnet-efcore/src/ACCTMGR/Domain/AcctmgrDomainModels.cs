using System;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AccountManager
    {
        public CustomerRecord Customer { get; set; } = new CustomerRecord();

        public class CustomerRecord
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } = string.Empty; // PIC X(10)

            [MaxLength(30)]
            public string CustomerName { get; set; } = string.Empty; // PIC X(30)

            public AddressRecord Address { get; set; } = new AddressRecord();

            public class AddressRecord
            {
                [MaxLength(50)]
                public string Street { get; set; } = string.Empty; // PIC X(50)

                [MaxLength(20)]
                public string City { get; set; } = string.Empty; // PIC X(20)

                [MaxLength(2)]
                public string State { get; set; } = string.Empty; // PIC X(2)

                [MaxLength(10)]
                public string ZipCode { get; set; } = string.Empty; // PIC X(10)
            }

            public AccountRecord[] Accounts { get; set; } = new AccountRecord[5]; // OCCURS 5 TIMES

            public class AccountRecord
            {
                [MaxLength(12)]
                public string AccountNumber { get; set; } = string.Empty; // PIC X(12)

                [Precision(15, 2)]
                public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

                public AccountTypeEnum AccountType { get; set; } // 88-level equivalent

                public enum AccountTypeEnum
                {
                    Checking, // VALUE "C"
                    Savings,  // VALUE "S"
                    Loan      // VALUE "L"
                }
            }
        }
    }
}