import { BcheodDomainModels } from 'src/BCHEOD/Domain/BcheodDomainModels';

export namespace BcheodInterfaces {
    /**
     * Interface for the main BCHEOD application logic.
     * Each method corresponds to a COBOL paragraph.
     */
    export interface IBcheodApplication {
        /**
         * Main driver paragraph.
         */
        drive(): Promise<void>;

        /**
         * Handles the next step in the process.
         */
        nextStep(): Promise<void>;

        /**
         * Handles intermediate step logic.
         */
        intStep(): Promise<void>;

        /**
         * Handles post-processing logic.
         */
        postStep(): Promise<void>;

        /**
         * Finalizes the process.
         */
        done(): Promise<void>;

        /**
         * Marks the start of the process.
         */
        markStart(): Promise<void>;

        /**
         * Marks the end of the process.
         */
        markEnd(): Promise<void>;
    }

    /**
     * Repository interface for interacting with the EOD_RUNS table.
     */
    export interface IEodRunsRepository {
        /**
         * Fetches all EOD_RUNS records.
         */
        fetchAllRuns(): Promise<BcheodDomainModels.EodRunBatch>;

        /**
         * Updates a specific EOD_RUN record.
         * @param run The EOD_RUN record to update.
         */
        updateRun(run: BcheodDomainModels.EodRun): Promise<void>;

        /**
         * Inserts a new EOD_RUN record.
         * @param run The EOD_RUN record to insert.
         */
        insertRun(run: BcheodDomainModels.EodRun): Promise<void>;
    }

    /**
     * Service interface for external calls to INTCALC.
     */
    export interface IIntCalcService {
        /**
         * Performs an interest calculation.
         * @param input The input parameters for the calculation.
         * @returns The result of the interest calculation.
         */
        calculateInterest(input: any): Promise<any>;
    }

    /**
     * Service interface for external calls to TXNPOST.
     */
    export interface ITxnPostService {
        /**
         * Posts a transaction.
         * @param input The input parameters for the transaction posting.
         * @returns The result of the transaction posting.
         */
        postTransaction(input: any): Promise<any>;
    }
}