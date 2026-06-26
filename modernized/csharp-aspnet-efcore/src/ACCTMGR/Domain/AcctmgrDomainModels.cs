using System;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AcctmgrDomainModels
    {
        public class Account
        {
            /// <summary>
            /// Account number
            /// </summary>
            [MaxLength(10)] // PIC X(10)
            public string AccountNumber { get; set; }

            /// <summary>
            /// Account balance
            /// </summary>
            [Range(-9999999999999.99, 9999999999999.99)] // PIC S9(13)V99 COMP-3
            public decimal Balance { get; set; }

            /// <summary>
            /// Account type
            /// </summary>
            public AccountType Type { get; set; } // 88-level equivalent

            /// <summary>
            /// Account status
            /// </summary>
            public AccountStatus Status { get; set; } // 88-level equivalent
        }

        public enum AccountType
        {
            Checking,
            Savings,
            Credit
        }

        public enum AccountStatus
        {
            Active,
            Inactive,
            Closed
        }
    }
}