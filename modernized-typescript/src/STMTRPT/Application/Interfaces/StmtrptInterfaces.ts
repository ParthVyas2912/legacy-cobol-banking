import { StmtrptDomainModels } from 'src/STMTRPT/Domain/StmtrptDomainModels';

export interface IStmtrptApplication {
    /**
     * Main entry point of the program.
     */
    main(): Promise<void>;

    /**
     * Fetches customer records.
     */
    fetchCustomers(): Promise<void>;

    /**
     * Processes IMS GN loop for fetching customer data.
     */
    imsGnLoop(): Promise<void>;

    /**
     * Opens a database cursor for transaction processing.
     */
    openCursor(): Promise<void>;

    /**
     * Computes the range of transactions for processing.
     */
    computeRange(): Promise<void>;

    /**
     * Processes the fetch loop for retrieving transaction data.
     */
    fetchLoop(): Promise<void>;

    /**
     * Closes the database cursor after processing.
     */
    closeCursor(): Promise<void>;
}

export interface IStmtrptRepository {
    /**
     * Writes data to the statement file.
     * @param data The data to write to the statement file.
     */
    writeToStmtFile(data: StmtrptDomainModels.CustomerRecord): Promise<void>;

    /**
     * Reads data from the transaction ledger table.
     * @param query The query parameters for fetching transaction data.
     * @returns The transaction data fetched from the ledger.
     */
    readFromTxnLedger(query: any): Promise<any>;
}

export interface IStmtrptService {
    /**
     * Executes an IMS function call with the specified function code and parameters.
     * @param functionCode The IMS function code to execute.
     * @param parameters The parameters required for the IMS function call.
     * @returns The result of the IMS function call.
     */
    executeImsFunction(functionCode: string, parameters: any): Promise<any>;

    /**
     * Executes a SQL query for transaction processing.
     * @param query The SQL query to execute.
     * @returns The result of the SQL query execution.
     */
    executeSqlQuery(query: string): Promise<any>;
}