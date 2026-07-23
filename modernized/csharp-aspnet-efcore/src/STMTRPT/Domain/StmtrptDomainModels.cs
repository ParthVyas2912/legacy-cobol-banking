using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class AccountSummary
        {
            /// <summary>
            /// Account Number
            /// </summary>
            [MaxLength(10)]
            public string AccountNumber { get; set; }
        }
    }
}