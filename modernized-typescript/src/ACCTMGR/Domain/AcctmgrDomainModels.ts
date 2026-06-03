import Decimal from 'decimal.js';

export namespace AcctmgrDomainModels {
    // Enum for account status
    export enum AccountStatus {
        ACTIVE = 'A', // Active
        INACTIVE = 'I', // Inactive
        CLOSED = 'C' // Closed
    }

    // Enum for account type
    export enum AccountType {
        SAVINGS = 'S', // Savings Account
        CHECKING = 'C' // Checking Account
    }

    // Account Record
    export class AccountRecord {
        // PIC 9(18)
        acctNumber: number; // Account Number

        // PIC 9(18)
        acctCustomerId: number; // Customer ID

        // PIC 9(8)
        acctOpenedDate: number; // Account Opened Date (YYYYMMDD)

        // PIC X(2)
        acctType: AccountType; // Account Type

        // PIC X(1)
        acctStatus: AccountStatus; // Account Status

        // PIC S9(13)V99 COMP-3
        acctBalance: Decimal; // Account Balance

        // PIC S9(13)V99 COMP-3
        acctAvailBalance: Decimal; // Available Balance

        // PIC 9(2)V9(4) COMP-3
        acctIntRate: Decimal; // Interest Rate

        // PIC 9(8)
        acctLastTxnDate: number; // Last Transaction Date (YYYYMMDD)

        // PIC X(3)
        acctCurrency: string; // Currency Code

        // PIC 9(5)
        acctBranchCode: number; // Branch Code

        // PIC X(4)
        acctFlags: string; // Account Flags

        // PIC X(50)
        acctFiller: string; // Filler Field

        constructor(
            acctNumber: number,
            acctCustomerId: number,
            acctOpenedDate: number,
            acctType: AccountType,
            acctStatus: AccountStatus,
            acctBalance: Decimal,
            acctAvailBalance: Decimal,
            acctIntRate: Decimal,
            acctLastTxnDate: number,
            acctCurrency: string,
            acctBranchCode: number,
            acctFlags: string,
            acctFiller: string
        ) {
            this.acctNumber = acctNumber;
            this.acctCustomerId = acctCustomerId;
            this.acctOpenedDate = acctOpenedDate;
            this.acctType = acctType;
            this.acctStatus = acctStatus;
            this.acctBalance = acctBalance;
            this.acctAvailBalance = acctAvailBalance;
            this.acctIntRate = acctIntRate;
            this.acctLastTxnDate = acctLastTxnDate;
            this.acctCurrency = acctCurrency;
            this.acctBranchCode = acctBranchCode;
            this.acctFlags = acctFlags;
            this.acctFiller = acctFiller;
        }
    }

    // Customer Record
    export class CustomerRecord {
        // PIC 9(18)
        custId: number; // Customer ID

        // PIC X(50)
        custName: string; // Customer Name

        // PIC X(100)
        custAddress: string; // Customer Address

        // PIC X(15)
        custPhone: string; // Customer Phone Number

        // PIC X(50)
        custEmail: string; // Customer Email

        // PIC 9(03) COMP-3
        custRiskScore: Decimal; // Risk Score

        // PIC 9(03) COMP-3
        custAcctCount: Decimal; // Account Count

        constructor(
            custId: number,
            custName: string,
            custAddress: string,
            custPhone: string,
            custEmail: string,
            custRiskScore: Decimal,
            custAcctCount: Decimal
        ) {
            this.custId = custId;
            this.custName = custName;
            this.custAddress = custAddress;
            this.custPhone = custPhone;
            this.custEmail = custEmail;
            this.custRiskScore = custRiskScore;
            this.custAcctCount = custAcctCount;
        }
    }
}