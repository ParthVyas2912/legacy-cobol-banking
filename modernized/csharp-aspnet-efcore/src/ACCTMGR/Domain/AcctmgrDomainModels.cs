using System;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AcctmgrDomainModels
    {
        public class Account
        {
            /// <summary>
            /// Account Number
            /// </summary>
            [MaxLength(10)] // PIC X(10)
            public string AccountNumber { get; set; }

            /// <summary>
            /// Account Balance
            /// </summary>
            [Range(-9999999999999.99, 9999999999999.99)] // PIC S9(13)V99 COMP-3
            public decimal AccountBalance { get; set; }

            /// <summary>
            /// Account Type
            /// </summary>
            public AccountTypeEnum AccountType { get; set; } // 88-level values for Account Type

            /// <summary>
            /// Account Holder Name
            /// </summary>
            [MaxLength(30)] // PIC X(30)
            public string AccountHolderName { get; set; }

            /// <summary>
            /// Account Status
            /// </summary>
            public AccountStatusEnum AccountStatus { get; set; } // 88-level values for Account Status
        }

        public enum AccountTypeEnum
        {
            Checking, // Value for Checking Account
            Savings,  // Value for Savings Account
            Loan      // Value for Loan Account
        }

        public enum AccountStatusEnum
        {
            Active,   // Value for Active Account
            Inactive, // Value for Inactive Account
            Closed    // Value for Closed Account
        }
    }
}