import Decimal from "decimal.js";

export class BcheodDomainModels {
    // Root level domain model for BCHEOD program

    // Account Number
    accountNumber: string; // PIC X(10)

    // Transaction Amount
    transactionAmount: Decimal; // PIC S9(13)V99 COMP-3

    // Transaction Date
    transactionDate: string; // PIC X(8)

    // Transaction Type
    transactionType: TransactionType; // 88-level enum

    // Nested group example
    customerDetails: CustomerDetails;

    constructor(
        accountNumber: string,
        transactionAmount: Decimal,
        transactionDate: string,
        transactionType: TransactionType,
        customerDetails: CustomerDetails
    ) {
        this.accountNumber = accountNumber;
        this.transactionAmount = transactionAmount;
        this.transactionDate = transactionDate;
        this.transactionType = transactionType;
        this.customerDetails = customerDetails;
    }
}

// Enum for 88-level values
export enum TransactionType {
    DEPOSIT = "DEPOSIT", // 88 DEPOSIT VALUE "D"
    WITHDRAWAL = "WITHDRAWAL", // 88 WITHDRAWAL VALUE "W"
}

// Nested group example
export class CustomerDetails {
    // Customer Name
    customerName: string; // PIC X(30)

    // Customer Address
    customerAddress: string; // PIC X(50)

    // Customer Phone Number
    customerPhoneNumber: string; // PIC X(15)

    constructor(
        customerName: string,
        customerAddress: string,
        customerPhoneNumber: string
    ) {
        this.customerName = customerName;
        this.customerAddress = customerAddress;
        this.customerPhoneNumber = customerPhoneNumber;
    }
}