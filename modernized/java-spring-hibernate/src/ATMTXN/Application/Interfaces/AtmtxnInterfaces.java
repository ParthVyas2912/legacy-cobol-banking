package src.ATMTXN.Application.Interfaces;

import src.ATMTXN.Domain.AtmtxnDomainModels.AtmTransaction;
import java.util.List;

/**
 * Application interfaces for the ATMTXN program.
 */
public interface AtmtxnInterfaces {

    /**
     * Reads ATM transaction records from the input file.
     * 
     * @return List of ATM transactions.
     */
    List<AtmTransaction> readAtmTransactionRecords();

    /**
     * Processes a single ATM transaction record.
     * 
     * @param transaction The ATM transaction to process.
     * @return Processed ATM transaction.
     */
    AtmTransaction processAtmTransaction(AtmTransaction transaction);

    /**
     * Writes ATM transaction records to the output file.
     * 
     * @param transactions List of ATM transactions to write.
     */
    void writeAtmTransactionRecords(List<AtmTransaction> transactions);

    /**
     * Logs the details of a processed ATM transaction.
     * 
     * @param transaction The ATM transaction to log.
     */
    void logTransactionDetails(AtmTransaction transaction);

    /**
     * Updates the status of an ATM transaction.
     * 
     * @param transactionId The ID of the transaction to update.
     * @param status The new status of the transaction.
     */
    void updateTransactionStatus(String transactionId, String status);

    /**
     * Retrieves an ATM transaction by its ID.
     * 
     * @param transactionId The ID of the transaction to retrieve.
     * @return The ATM transaction with the specified ID.
     */
    AtmTransaction getTransactionById(String transactionId);

    /**
     * Validates the details of an ATM transaction.
     * 
     * @param transaction The ATM transaction to validate.
     * @return True if the transaction is valid, false otherwise.
     */
    boolean validateTransaction(AtmTransaction transaction);

    /**
     * Calculates the total transaction amount for a list of transactions.
     * 
     * @param transactions List of ATM transactions.
     * @return The total transaction amount.
     */
    double calculateTotalTransactionAmount(List<AtmTransaction> transactions);

    /**
     * Generates a summary report for a list of ATM transactions.
     * 
     * @param transactions List of ATM transactions.
     * @return Summary report as a string.
     */
    String generateTransactionSummaryReport(List<AtmTransaction> transactions);

    /**
     * Archives processed ATM transactions.
     * 
     * @param transactions List of ATM transactions to archive.
     */
    void archiveProcessedTransactions(List<AtmTransaction> transactions);
}