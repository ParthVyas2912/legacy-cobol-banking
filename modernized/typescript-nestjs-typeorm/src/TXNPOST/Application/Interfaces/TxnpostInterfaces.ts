/**
 * Application interfaces for TXNPOST program.
 * Generated from COBOL paragraphs, file I/O operations, and CICS operations.
 */

export interface ITxnpostApplication {
  /**
   * Entry point for the TXNPOST program.
   * Executes the main processing logic.
   */
  execute(): Promise<void>;
}

export interface ITxnpostParagraphs {
  /**
   * Initializes the program and sets up necessary configurations.
   */
  initialize(): Promise<void>;

  /**
   * Reads input transactions from the input file.
   */
  readInputTransactions(): Promise<void>;

  /**
   * Validates the input transactions for correctness.
   */
  validateTransactions(): Promise<void>;

  /**
   * Processes each transaction and applies business logic.
   */
  processTransactions(): Promise<void>;

  /**
   * Writes the processed transactions to the output file.
   */
  writeOutputTransactions(): Promise<void>;

  /**
   * Finalizes the program, performs cleanup, and generates reports.
   */
  finalize(): Promise<void>;
}

export interface ITxnpostFileRepository {
  /**
   * Reads a record from the input file.
   * @returns The next transaction record from the input file.
   */
  readInputFile(): Promise<string | null>;

  /**
   * Writes a record to the output file.
   * @param record The transaction record to write.
   */
  writeOutputFile(record: string): Promise<void>;
}

export interface ITxnpostCicsService {
  /**
   * Performs a CICS operation to retrieve account details.
   * @param accountId The ID of the account to retrieve.
   * @returns The account details.
   */
  getAccountDetails(accountId: string): Promise<any>;

  /**
   * Performs a CICS operation to update account balances.
   * @param accountId The ID of the account to update.
   * @param amount The amount to update the balance by.
   */
  updateAccountBalance(accountId: string, amount: Decimal): Promise<void>;
}