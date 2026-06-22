import Decimal from "decimal.js";

export class KycsyncDomainModels {
    // Root-level domain models for KYCSYNC program

    // Example of a nested group structure
    export class CustomerRecord {
        customerId: number; // PIC 9(10)
        customerName: string; // PIC X(50)
        accountBalance: Decimal; // PIC S9(13)V99 COMP-3
        accountStatus: AccountStatus; // 88-level values

        constructor(
            customerId: number,
            customerName: string,
            accountBalance: Decimal,
            accountStatus: AccountStatus
        ) {
            this.customerId = customerId;
            this.customerName = customerName;
            this.accountBalance = accountBalance;
            this.accountStatus = accountStatus;
        }
    }

    // Example of an 88-level value mapped to an enum
    export enum AccountStatus {
        Active = "ACTIVE", // 88 ACTIVE VALUE "ACTIVE"
        Inactive = "INACTIVE", // 88 INACTIVE VALUE "INACTIVE"
        Suspended = "SUSPENDED", // 88 SUSPENDED VALUE "SUSPENDED"
    }

    // Example of an OCCURS clause
    export class TransactionHistory {
        transactions: Transaction[]; // OCCURS 10 TIMES

        constructor(transactions: Transaction[]) {
            if (transactions.length > 10) {
                throw new Error("Transaction history cannot exceed 10 entries.");
            }
            this.transactions = transactions;
        }
    }

    export class Transaction {
        transactionId: number; // PIC 9(10)
        transactionAmount: Decimal; // PIC S9(10)V99 COMP-3
        transactionDate: string; // PIC X(10)

        constructor(
            transactionId: number,
            transactionAmount: Decimal,
            transactionDate: string
        ) {
            this.transactionId = transactionId;
            this.transactionAmount = transactionAmount;
            this.transactionDate = transactionDate;
        }
    }

    // Example of a REDEFINES clause
    export type PaymentDetails =
        | { type: "CreditCard"; cardNumber: string; expiryDate: string } // PIC X(16), PIC X(5)
        | { type: "BankTransfer"; accountNumber: string; routingNumber: string }; // PIC X(20), PIC X(9)
}