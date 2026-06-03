import { TxnpostDomainModels } from 'src/TXNPOST/Domain/TxnpostDomainModels';

// Repository interface for file I/O operations
export interface ITxnpostRepository {
    /**
     * Opens the necessary files for processing.
     */
    openFiles(): Promise<void>;

    /**
     * Reads a transaction record from the input file.
     * @returns A promise that resolves to the transaction record, or null if end of file is reached.
     */
    readTransaction(): Promise<any | null>;

    /**
     * Writes an audit record to the output file.
     * @param auditRecord The audit record to write.
     */
    writeAuditRecord(auditRecord: any): Promise<void>;

    /**
     * Closes the opened files.
     */
    closeFiles(): Promise<void>;
}

// Service interface for database operations
export interface ITxnpostService {
    /**
     * Applies a transaction to the transaction ledger.
     * @param transaction The transaction to apply.
     * @returns A promise that resolves to a boolean indicating success or failure.
     */
    applyTransaction(transaction: any): Promise<boolean>;
}

// Main application interface
export interface ITxnpostApplication {
    /**
     * Main entry point for the TXNPOST program.
     */
    main(): Promise<void>;

    /**
     * Opens the necessary files for processing.
     */
    openFiles(): Promise<void>;

    /**
     * Processes all transactions until the end of the input file.
     */
    processTransactions(): Promise<void>;

    /**
     * Applies a single transaction to the ledger and writes an audit record.
     * @param transaction The transaction to apply.
     */
    applyTransaction(transaction: any): Promise<void>;

    /**
     * Closes the opened files after processing is complete.
     */
    closeFiles(): Promise<void>;
}