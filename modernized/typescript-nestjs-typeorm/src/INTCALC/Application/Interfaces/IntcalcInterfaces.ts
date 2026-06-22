import { IntcalcDomainModels, TransactionType, CustomerDetails, TransactionHistory } from 'src/INTCALC/Domain/IntcalcDomainModels';
import Decimal from 'decimal.js';

/**
 * Interface for the INTCALC program's main application logic.
 * The application interface exposes only the public entry point.
 */
export interface IIntcalcApplication {
  /**
   * Main entry point for the INTCALC program.
   */
  execute(): Promise<void>;
}

/**
 * Interface for the repository handling file I/O operations.
 */
export interface IIntcalcRepository {
  /**
   * Reads input data from a file.
   * @returns The domain model populated with input data.
   */
  readInputFile(): Promise<IntcalcDomainModels>;

  /**
   * Writes output data to a file.
   * @param data The domain model containing the calculated interest data.
   */
  writeOutputFile(data: IntcalcDomainModels): Promise<void>;
}

/**
 * Interface for CICS service operations used by the INTCALC program.
 */
export interface IIntcalcCicsService {
  /**
   * Retrieves customer details from a CICS service.
   * @param accountNumber The account number for which to retrieve customer details.
   * @returns The customer details associated with the account number.
   */
  getCustomerDetails(accountNumber: string): Promise<CustomerDetails>;

  /**
   * Logs a transaction to a CICS service.
   * @param accountNumber The account number for which the transaction is being logged.
   * @param transactionType The type of transaction (e.g., DEPOSIT, WITHDRAWAL).
   * @param amount The amount of the transaction.
   */
  logTransaction(accountNumber: string, transactionType: TransactionType, amount: Decimal): Promise<void>;
}