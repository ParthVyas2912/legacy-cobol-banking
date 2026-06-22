import Decimal from "decimal.js";

/**
 * Account Manager Domain Models
 * Generated from COBOL data structures
 */

export class AccountRecord {
    // PIC X(10)
    accountId: string; // MaxLength(10)

    // PIC X(30)
    accountName: string; // MaxLength(30)

    // PIC 9(13)V99 COMP-3
    accountBalance: Decimal; // Precision (15, 2)

    // PIC 9(8)
    lastTransactionDate: number; // YYYYMMDD format

    // PIC X(1)
    accountStatus: AccountStatus; // Enum for 88-level values

    constructor(
        accountId: string,
        accountName: string,
        accountBalance: Decimal,
        lastTransactionDate: number,
        accountStatus: AccountStatus
    ) {
        this.accountId = accountId;
        this.accountName = accountName;
        this.accountBalance = accountBalance;
        this.lastTransactionDate = lastTransactionDate;
        this.accountStatus = accountStatus;
    }
}

export enum AccountStatus {
    Active = "A", // 88-level value for active accounts
    Inactive = "I", // 88-level value for inactive accounts
    Closed = "C", // 88-level value for closed accounts
}

export class TransactionRecord {
    // PIC X(10)
    transactionId: string; // MaxLength(10)

    // PIC X(10)
    accountId: string; // MaxLength(10)

    // PIC 9(13)V99 COMP-3
    transactionAmount: Decimal; // Precision (15, 2)

    // PIC 9(8)
    transactionDate: number; // YYYYMMDD format

    // PIC X(1)
    transactionType: TransactionType; // Enum for 88-level values

    constructor(
        transactionId: string,
        accountId: string,
        transactionAmount: Decimal,
        transactionDate: number,
        transactionType: TransactionType
    ) {
        this.transactionId = transactionId;
        this.accountId = accountId;
        this.transactionAmount = transactionAmount;
        this.transactionDate = transactionDate;
        this.transactionType = transactionType;
    }
}

export enum TransactionType {
    Deposit = "D", // 88-level value for deposit transactions
    Withdrawal = "W", // 88-level value for withdrawal transactions
    Transfer = "T", // 88-level value for transfer transactions
}

export class AccountManager {
    // OCCURS 100 TIMES
    accounts: AccountRecord[]; // Array of size 100

    // OCCURS 1000 TIMES
    transactions: TransactionRecord[]; // Array of size 1000

    constructor(accounts: AccountRecord[], transactions: TransactionRecord[]) {
        this.accounts = accounts;
        this.transactions = transactions;
    }
}