import Decimal from 'decimal.js';
import { FrdchkDomainModels } from 'src/FRDCHK/Domain/FrdchkDomainModels';

export namespace FrdchkInterfaces {
    /**
     * Interface for the main FRDCHK application logic.
     */
    export interface FrdchkApplication {
        /**
         * Entry point for the FRDCHK program.
         */
        start(): void;

        /**
         * Computes the velocity metrics for transactions.
         * Updates working storage with velocity count and amount.
         */
        computeVelocity(): void;

        /**
         * Scores the risk of a transaction based on velocity and other factors.
         * Updates working storage with the calculated risk score.
         */
        scoreRisk(): void;

        /**
         * Freezes an account if the risk score exceeds a certain threshold.
         */
        freezeAccount(): void;
    }

    /**
     * Interface for repository operations related to transaction data.
     */
    export interface TransactionRepository {
        /**
         * Reads transaction data from the TXN_LEDGER table.
         * @param accountId The account ID for which to fetch transactions.
         * @returns A list of transaction records.
         */
        readTransactions(accountId: string): FrdchkDomainModels.TxnRecord[];

        /**
         * Updates the FRAUD_FLAGS table to mark an account as frozen.
         * @param accountId The account ID to freeze.
         */
        updateFraudFlags(accountId: string): void;
    }

    /**
     * Interface for CICS service operations.
     */
    export interface CicsService {
        /**
         * Returns control to the calling program or terminal.
         */
        returnControl(): void;

        /**
         * Links to another CICS program.
         * @param programName The name of the program to link to.
         * @param commArea The communication area to pass to the linked program.
         */
        linkToProgram(programName: string, commArea: Buffer): void;
    }
}