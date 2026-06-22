import { AtmtxnDomainModels } from "src/ATMTXN/Domain/AtmtxnDomainModels";

/**
 * Interface for the ATMTXN application.
 */
export interface IAtmtxnApplication {
  /**
   * Main entry point for the ATMTXN program.
   */
  execute(): Promise<void>;
}

/**
 * Interface for repository operations related to account data.
 */
export interface IAccountRepository {
  /**
   * Reads account information based on the account number.
   * @param accountNumber The account number to retrieve information for.
   * @returns A promise that resolves to the account information.
   */
  readAccount(accountNumber: string): Promise<AtmtxnDomainModels.AccountInfo>;

  /**
   * Updates the account information in the repository.
   * @param accountInfo The account information to update.
   * @returns A promise that resolves when the update is complete.
   */
  updateAccount(accountInfo: AtmtxnDomainModels.AccountInfo): Promise<void>;
}

/**
 * Interface for CICS service operations.
 */
export interface ICicsService {
  /**
   * Sends a transaction request to the CICS system.
   * @param transactionData The transaction data to send.
   * @returns A promise that resolves when the transaction is processed.
   */
  sendTransaction(transactionData: any): Promise<void>;

  /**
   * Retrieves the status of a transaction from the CICS system.
   * @param transactionId The ID of the transaction to check.
   * @returns A promise that resolves to the transaction status.
   */
  getTransactionStatus(transactionId: string): Promise<string>;
}

/**
 * Interface for the ATMTXN program's paragraph methods.
 */
export interface IAtmtxnParagraphs {
  /**
   * Initializes the program and sets up the environment.
   */
  initialize(): Promise<void>;

  /**
   * Reads input data for processing.
   */
  readInput(): Promise<void>;

  /**
   * Processes the transaction.
   */
  processTransaction(): Promise<void>;

  /**
   * Writes output data after processing.
   */
  writeOutput(): Promise<void>;

  /**
   * Finalizes the program and performs cleanup operations.
   */
  finalize(): Promise<void>;
}