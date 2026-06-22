import Decimal from "decimal.js";

/**
 * Represents the domain models for the INTCALC program.
 * Generated from COBOL data structures.
 */

export class IntcalcDomainModels {
  // Example of a decimal-safe value object or fixed-precision decimal library
  // Field: INTEREST_AMOUNT
  // PIC S9(13)V99 COMP-3
  interestAmount: Decimal;

  // Example of a string with MaxLength
  // Field: ACCOUNT_NUMBER
  // PIC X(10)
  accountNumber: string;

  // Example of an integer field
  // Field: TRANSACTION_COUNT
  // PIC 9(5)
  transactionCount: number;

  // Example of an enum for 88-level values
  // Field: TRANSACTION_TYPE
  // 88-level values: DEPOSIT, WITHDRAWAL
  transactionType: TransactionType;

  // Example of a nested class/record preserving the COBOL hierarchy
  // Group: CUSTOMER_DETAILS
  customerDetails: CustomerDetails;

  // Example of an array/list of size n
  // Field: TRANSACTION_HISTORY
  // OCCURS 10 TIMES
  transactionHistory: TransactionHistory[];

  constructor(
    interestAmount: Decimal,
    accountNumber: string,
    transactionCount: number,
    transactionType: TransactionType,
    customerDetails: CustomerDetails,
    transactionHistory: TransactionHistory[]
  ) {
    this.interestAmount = interestAmount;
    this.accountNumber = accountNumber;
    this.transactionCount = transactionCount;
    this.transactionType = transactionType;
    this.customerDetails = customerDetails;
    this.transactionHistory = transactionHistory;
  }
}

// Enum for 88-level values
export enum TransactionType {
  DEPOSIT = "DEPOSIT", // 88-level value
  WITHDRAWAL = "WITHDRAWAL", // 88-level value
}

// Nested class for CUSTOMER_DETAILS group
export class CustomerDetails {
  // Field: CUSTOMER_NAME
  // PIC X(30)
  customerName: string;

  // Field: CUSTOMER_ID
  // PIC 9(10)
  customerId: number;

  constructor(customerName: string, customerId: number) {
    this.customerName = customerName;
    this.customerId = customerId;
  }
}

// Nested class for TRANSACTION_HISTORY group
export class TransactionHistory {
  // Field: TRANSACTION_DATE
  // PIC 9(8)
  transactionDate: number;

  // Field: TRANSACTION_AMOUNT
  // PIC S9(11)V99 COMP-3
  transactionAmount: Decimal;

  constructor(transactionDate: number, transactionAmount: Decimal) {
    this.transactionDate = transactionDate;
    this.transactionAmount = transactionAmount;
  }
}