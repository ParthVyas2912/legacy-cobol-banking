import Decimal from "decimal.js";

/**
 * Represents the domain models for the STMTRPT program.
 * This file is generated based on the COBOL data structures and follows the specified mapping rules.
 */

// Example domain model classes based on the provided rules

export class AccountRecord {
    // PIC X(10)
    accountNumber: string; // MaxLength(10)

    // PIC X(30)
    accountName: string; // MaxLength(30)

    // PIC S9(13)V99 COMP-3
    accountBalance: Decimal; // Precision (15, 2)

    // PIC 9(4)
    branchCode: number; // Integer

    // PIC X(1)
    accountStatus: AccountStatus; // Enum for 88-level values

    constructor(
        accountNumber: string,
        accountName: string,
        accountBalance: Decimal,
        branchCode: number,
        accountStatus: AccountStatus
    ) {
        this.accountNumber = accountNumber;
        this.accountName = accountName;
        this.accountBalance = accountBalance;
        this.branchCode = branchCode;
        this.accountStatus = accountStatus;
    }
}

// Enum for 88-level values for AccountStatus
export enum AccountStatus {
    Active = "A", // 88 ACTIVE VALUE 'A'
    Inactive = "I", // 88 INACTIVE VALUE 'I'
    Closed = "C", // 88 CLOSED VALUE 'C'
}

// Example of a group item with OCCURS clause
export class TransactionRecord {
    // PIC X(10)
    transactionId: string; // MaxLength(10)

    // PIC S9(11)V99 COMP-3
    transactionAmount: Decimal; // Precision (13, 2)

    // PIC X(8)
    transactionDate: string; // MaxLength(8)

    constructor(
        transactionId: string,
        transactionAmount: Decimal,
        transactionDate: string
    ) {
        this.transactionId = transactionId;
        this.transactionAmount = transactionAmount;
        this.transactionDate = transactionDate;
    }
}

// Example of a group with OCCURS
export class AccountTransactions {
    // OCCURS 10 TIMES
    transactions: TransactionRecord[]; // Array of size 10

    constructor(transactions: TransactionRecord[]) {
        if (transactions.length !== 10) {
            throw new Error("AccountTransactions must contain exactly 10 transactions.");
        }
        this.transactions = transactions;
    }
}

// Example of a redefined structure
export class CustomerRecord {
    // PIC X(20)
    customerName: string; // MaxLength(20)

    // PIC X(10)
    customerId: string; // MaxLength(10)

    // REDEFINES customerId
    customerTaxId?: string; // MaxLength(10)

    constructor(customerName: string, customerId: string, customerTaxId?: string) {
        this.customerName = customerName;
        this.customerId = customerId;
        this.customerTaxId = customerTaxId;
    }
}