import { AccountRecord, TransactionRecord } from 'src/ACCTMGR/Domain/AcctmgrDomainModels';
import Decimal from 'decimal.js';

/**
 * Interface for account repository operations.
 * Handles file I/O for account records.
 */
export interface IAccountRepository {
    /**
     * Reads all account records from the data source.
     * @returns A promise that resolves to an array of AccountRecord.
     */
    readAllAccounts(): Promise<AccountRecord[]>;

    /**
     * Writes a single account record to the data source.
     * @param account The account record to write.
     * @returns A promise that resolves when the operation is complete.
     */
    writeAccount(account: AccountRecord): Promise<void>;

    /**
     * Updates an existing account record in the data source.
     * @param account The account record to update.
     * @returns A promise that resolves when the operation is complete.
     */
    updateAccount(account: AccountRecord): Promise<void>;

    /**
     * Deletes an account record from the data source.
     * @param accountId The ID of the account to delete.
     * @returns A promise that resolves when the operation is complete.
     */
    deleteAccount(accountId: string): Promise<void>;
}

/**
 * Interface for transaction repository operations.
 * Handles file I/O for transaction records.
 */
export interface ITransactionRepository {
    /**
     * Reads all transaction records from the data source.
     * @returns A promise that resolves to an array of TransactionRecord.
     */
    readAllTransactions(): Promise<TransactionRecord[]>;

    /**
     * Writes a single transaction record to the data source.
     * @param transaction The transaction record to write.
     * @returns A promise that resolves when the operation is complete.
     */
    writeTransaction(transaction: TransactionRecord): Promise<void>;

    /**
     * Updates an existing transaction record in the data source.
     * @param transaction The transaction record to update.
     * @returns A promise that resolves when the operation is complete.
     */
    updateTransaction(transaction: TransactionRecord): Promise<void>;

    /**
     * Deletes a transaction record from the data source.
     * @param transactionId The ID of the transaction to delete.
     * @returns A promise that resolves when the operation is complete.
     */
    deleteTransaction(transactionId: string): Promise<void>;
}

/**
 * Interface for account management service operations.
 * Handles business logic related to account management.
 */
export interface IAccountManagementService {
    /**
     * Initializes the account management process.
     * @returns A promise that resolves when the initialization is complete.
     */
    initialize(): Promise<void>;

    /**
     * Processes a deposit transaction for a given account.
     * @param accountId The ID of the account.
     * @param amount The deposit amount.
     * @returns A promise that resolves when the operation is complete.
     */
    processDeposit(accountId: string, amount: Decimal): Promise<void>;

    /**
     * Processes a withdrawal transaction for a given account.
     * @param accountId The ID of the account.
     * @param amount The withdrawal amount.
     * @returns A promise that resolves when the operation is complete.
     */
    processWithdrawal(accountId: string, amount: Decimal): Promise<void>;

    /**
     * Processes a transfer transaction between two accounts.
     * @param fromAccountId The ID of the source account.
     * @param toAccountId The ID of the destination account.
     * @param amount The transfer amount.
     * @returns A promise that resolves when the operation is complete.
     */
    processTransfer(fromAccountId: string, toAccountId: string, amount: Decimal): Promise<void>;

    /**
     * Generates a report of all accounts and their balances.
     * @returns A promise that resolves to an array of AccountRecord.
     */
    generateAccountReport(): Promise<AccountRecord[]>;

    /**
     * Generates a report of all transactions.
     * @returns A promise that resolves to an array of TransactionRecord.
     */
    generateTransactionReport(): Promise<TransactionRecord[]>;
}