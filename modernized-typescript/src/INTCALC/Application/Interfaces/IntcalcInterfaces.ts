import { IntcalcDomainModels } from 'src/INTCALC/Domain/IntcalcDomainModels';

export namespace IntcalcInterfaces {
    /**
     * Interface for the main application logic of the INTCALC program.
     */
    export interface IntcalcApplication {
        /**
         * Entry point for the program.
         * Corresponds to the COBOL paragraph 0000-MAIN.
         */
        main(): Promise<void>;

        /**
         * Handles looping logic for processing accounts.
         * Corresponds to the COBOL paragraph 1000-LOOP.
         */
        loop(): Promise<void>;

        /**
         * Performs interest accrual calculations.
         * Corresponds to the COBOL paragraph 2000-ACCRUE.
         */
        accrue(): Promise<void>;
    }

    /**
     * Repository interface for file I/O operations related to the ACCT-FILE.
     */
    export interface AcctFileRepository {
        /**
         * Reads a record from the ACCT-FILE.
         * @param acctNumber The account number to read.
         * @returns The account record.
         */
        readAcctFile(acctNumber: number): Promise<IntcalcDomainModels.AcctFile | null>;

        /**
         * Writes a record to the ACCT-FILE.
         * @param acctFile The account record to write.
         */
        writeAcctFile(acctFile: IntcalcDomainModels.AcctFile): Promise<void>;
    }

    /**
     * Repository interface for file I/O operations related to the INT-JOURNAL.
     */
    export interface IntJournalRepository {
        /**
         * Writes a record to the INT-JOURNAL.
         * @param txnId The transaction ID.
         * @param dailyInterest The daily interest amount.
         * @param newBalance The new balance after interest accrual.
         */
        writeIntJournal(
            txnId: IntcalcDomainModels.WsNextTxnId,
            dailyInterest: IntcalcDomainModels.WsDailyInterest,
            newBalance: IntcalcDomainModels.WsNewBalance
        ): Promise<void>;
    }
}