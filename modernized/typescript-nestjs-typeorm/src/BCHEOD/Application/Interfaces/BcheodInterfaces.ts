import Decimal from "decimal.js";
import { BcheodDomainModels, TransactionType, CustomerDetails } from "src/BCHEOD/Domain/BcheodDomainModels";

// Interface for BCHEOD application logic
export interface IBcheodApplication {
    /**
     * Main entry point for the BCHEOD program.
     */
    execute(): Promise<void>;
}

// Interface for file repository operations
export interface IBcheodFileRepository {
    /**
     * Reads the next transaction record from the input file.
     * @returns The next transaction record, or null if end of file is reached.
     */
    readNextTransaction(): Promise<BcheodDomainModels | null>;

    /**
     * Writes a transaction record to the output file.
     * @param record The transaction record to write.
     */
    writeTransaction(record: BcheodDomainModels): Promise<void>;

    /**
     * Writes an error record to the error file.
     * @param record The transaction record that caused the error.
     * @param errorMessage The error message describing the issue.
     */
    writeError(record: BcheodDomainModels, errorMessage: string): Promise<void>;
}

// Interface for CICS service operations
export interface IBcheodCicsService {
    /**
     * Retrieves customer details from the CICS system based on the account number.
     * @param accountNumber The account number to look up.
     * @returns The customer details associated with the account number.
     */
    getCustomerDetails(accountNumber: string): Promise<CustomerDetails>;

    /**
     * Posts a transaction to the CICS system.
     * @param record The transaction record to post.
     */
    postTransaction(record: BcheodDomainModels): Promise<void>;
}