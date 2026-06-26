using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AcctmgrDomainModels
    {
        public class Account
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatusEnum AccountStatus { get; set; } // 88-level equivalent

            public CustomerDetails Customer { get; set; } // GROUP item

            public Transaction[] Transactions { get; set; } // OCCURS 10 TIMES

            public class CustomerDetails
            {
                [MaxLength(30)]
                public string Name { get; set; } // PIC X(30)

                [MaxLength(15)]
                public string PhoneNumber { get; set; } // PIC X(15)

                [MaxLength(50)]
                public string Address { get; set; } // PIC X(50)
            }

            public class Transaction
            {
                public DateTime TransactionDate { get; set; } // PIC 9(8)

                public decimal Amount { get; set; } // PIC S9(9)V99 COMP-3

                [MaxLength(20)]
                public string Description { get; set; } // PIC X(20)
            }

            public enum AccountStatusEnum
            {
                Active, // 88-level value
                Inactive, // 88-level value
                Closed // 88-level value
            }
        }
    }
}