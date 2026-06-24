package src.ATMTXN.Application.Interfaces;

import src.ATMTXN.Domain.AtmtxnDomainModels.TransactionRecord;
import src.ATMTXN.Domain.AtmtxnDomainModels.TransactionDetail;
import src.ATMTXN.Domain.AtmtxnDomainModels.TransactionStatus;

import java.util.List;
import java.util.Optional;

public interface AtmtxnInterfaces {

    /**
     * Initialize the transaction processing environment.
     */
    void initializeTransactionEnvironment();

    /**
     * Read a transaction record by transaction ID.
     *
     * @param transactionId The ID of the transaction to retrieve.
     * @return An optional containing the transaction record if found, or empty if not found.
     */
    Optional<TransactionRecord> readTransactionRecord(String transactionId);

    /**
     * Write a new transaction record.
     *
     * @param transactionRecord The transaction record to write.
     */
    void writeTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Update an existing transaction record.
     *
     * @param transactionRecord The transaction record to update.
     */
    void updateTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Delete a transaction record by transaction ID.
     *
     * @param transactionId The ID of the transaction to delete.
     */
    void deleteTransactionRecord(String transactionId);

    /**
     * Retrieve all transaction records.
     *
     * @return A list of all transaction records.
     */
    List<TransactionRecord> getAllTransactionRecords();

    /**
     * Process a transaction.
     *
     * @param transactionRecord The transaction record to process.
     * @return The status of the transaction after processing.
     */
    TransactionStatus processTransaction(TransactionRecord transactionRecord);

    /**
     * Retrieve transaction details for a specific transaction.
     *
     * @param transactionId The ID of the transaction.
     * @return A list of transaction details associated with the transaction.
     */
    List<TransactionDetail> getTransactionDetails(String transactionId);

    /**
     * Validate a transaction record.
     *
     * @param transactionRecord The transaction record to validate.
     * @return True if the transaction is valid, false otherwise.
     */
    boolean validateTransaction(TransactionRecord transactionRecord);

    /**
     * Finalize the transaction processing environment.
     */
    void finalizeTransactionEnvironment();
}