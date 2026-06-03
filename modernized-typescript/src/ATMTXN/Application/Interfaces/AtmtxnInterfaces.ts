import { AtmtxnDomainModels } from 'src/ATMTXN/Domain/AtmtxnDomainModels';

export namespace AtmtxnInterfaces {
    /**
     * Interface for the main application logic of the ATMTXN program.
     */
    export interface AtmtxnApplication {
        /**
         * Entry point for the program.
         */
        start(): Promise<void>;

        /**
         * Validates the transaction input.
         */
        validate(): Promise<void>;

        /**
         * Posts the transaction to the system.
         */
        postTransaction(): Promise<void>;

        /**
         * Sends the next screen or response.
         */
        sendNext(): Promise<void>;

        /**
         * Maps and handles errors.
         */
        mapError(): Promise<void>;

        /**
         * Handles fatal errors and terminates the program.
         */
        handleFatalError(): Promise<void>;
    }

    /**
     * Interface for CICS service operations used in the ATMTXN program.
     */
    export interface CicsService {
        /**
         * Handles CICS HANDLE command.
         */
        handle(): Promise<void>;

        /**
         * Handles CICS RECEIVE command.
         */
        receive(): Promise<void>;

        /**
         * Handles CICS RETURN command.
         */
        return(): Promise<void>;

        /**
         * Handles CICS LINK command.
         */
        link(): Promise<void>;

        /**
         * Handles CICS SEND command.
         */
        send(): Promise<void>;

        /**
         * Handles CICS ABEND command.
         */
        abend(): Promise<void>;
    }
}