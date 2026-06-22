import { AccountRecord, TransactionRecord } from "../../Domain/StmtrptDomainModels";

/**
 * Interface for the STMTRPT application.
 * Each COBOL paragraph is represented as a method.
 * File I/O operations are represented as repository interfaces.
 * CICS operations are represented as service interfaces.
 */

// Repository interface for file I/O operations
export interface IAccountRepository {
    /**
     * Reads an account record by account number.
     * @param accountNumber The account number to search for.
     * @returns The account record if found, otherwise null.
     */
    readAccount(accountNumber: string): Promise<AccountRecord | null>;

    /**
     * Writes an account record to the file.
     * @param accountRecord The account record to write.
     */
    writeAccount(accountRecord: AccountRecord): Promise<void>;

    /**
     * Reads all transaction records for a given account.
     * @param accountNumber The account number to retrieve transactions for.
     * @returns An array of transaction records.
     */
    readTransactions(accountNumber: string): Promise<TransactionRecord[]>;

    /**
     * Writes a transaction record to the file.
     * @param transactionRecord The transaction record to write.
     */
    writeTransaction(transactionRecord: TransactionRecord): Promise<void>;
}

// Service interface for CICS operations
export interface ICicsService {
    /**
     * Sends a CICS transaction to retrieve account details.
     * @param accountNumber The account number to retrieve details for.
     * @returns The account record if found, otherwise null.
     */
    getAccountDetails(accountNumber: string): Promise<AccountRecord | null>;

    /**
     * Sends a CICS transaction to update account details.
     * @param accountRecord The account record to update.
     */
    updateAccountDetails(accountRecord: AccountRecord): Promise<void>;

    /**
     * Sends a CICS transaction to retrieve transaction details.
     * @param transactionId The transaction ID to retrieve details for.
     * @returns The transaction record if found, otherwise null.
     */
    getTransactionDetails(transactionId: string): Promise<TransactionRecord | null>;
}

// Application interface for STMTRPT program
export interface IStmtrptApplication {
    /**
     * Main entry point for the STMTRPT program.
     */
    execute(): Promise<void>;
}