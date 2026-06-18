package src.TXNPOST.Application.Interfaces;

import src.TXNPOST.Domain.TxnpostDomainModels.TransactionRecord;

/**
 * Application interfaces for the TXNPOST program.
 * Each COBOL paragraph is represented as a method.
 * File I/O operations are represented as repository interfaces.
 * CICS operations are represented as service interfaces.
 */
public interface TxnpostInterfaces {

    /**
     * Initializes the transaction posting process.
     */
    void initializeTransactionPosting();

    /**
     * Reads the next transaction record from the input file.
     * @return The next transaction record.
     */
    TransactionRecord readTransactionRecord();

    /**
     * Validates the transaction record for correctness and compliance.
     * @param transactionRecord The transaction record to validate.
     * @return True if the record is valid, false otherwise.
     */
    boolean validateTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Posts the transaction to the appropriate account.
     * @param transactionRecord The transaction record to post.
     */
    void postTransaction(TransactionRecord transactionRecord);

    /**
     * Handles any errors encountered during transaction processing.
     * @param transactionRecord The transaction record that caused the error.
     */
    void handleTransactionError(TransactionRecord transactionRecord);

    /**
     * Writes the processed transaction record to the output file.
     * @param transactionRecord The transaction record to write.
     */
    void writeTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Finalizes the transaction posting process.
     */
    void finalizeTransactionPosting();

    /**
     * Archives the transaction record for long-term storage.
     * @param transactionRecord The transaction record to archive.
     */
    void archiveTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Sends notifications (email/SMS) based on the transaction record.
     * @param transactionRecord The transaction record for which notifications are sent.
     */
    void sendTransactionNotifications(TransactionRecord transactionRecord);

    /**
     * Performs reconciliation for the batch of transactions.
     */
    void performBatchReconciliation();

    /**
     * Generates reports based on the processed transactions.
     */
    void generateTransactionReports();

    /**
     * Performs compliance checks on the transaction record.
     * @param transactionRecord The transaction record to check.
     * @return True if the record passes compliance checks, false otherwise.
     */
    boolean performComplianceCheck(TransactionRecord transactionRecord);

    /**
     * Performs fraud detection checks on the transaction record.
     * @param transactionRecord The transaction record to check.
     * @return True if no fraud is detected, false otherwise.
     */
    boolean performFraudDetection(TransactionRecord transactionRecord);

    /**
     * Adjusts the transaction record if necessary.
     * @param transactionRecord The transaction record to adjust.
     */
    void adjustTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Overrides the transaction record based on specific conditions.
     * @param transactionRecord The transaction record to override.
     */
    void overrideTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Handles batch processing flags for the transaction record.
     * @param transactionRecord The transaction record to process.
     */
    void handleBatchProcessingFlags(TransactionRecord transactionRecord);

    /**
     * Prints the transaction record for audit purposes.
     * @param transactionRecord The transaction record to print.
     */
    void printTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Audits the transaction record for regulatory compliance.
     * @param transactionRecord The transaction record to audit.
     */
    void auditTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Handles regulatory reporting for the transaction record.
     * @param transactionRecord The transaction record to report.
     */
    void handleRegulatoryReporting(TransactionRecord transactionRecord);

    /**
     * Reserves the transaction record for special processing.
     * @param transactionRecord The transaction record to reserve.
     */
    void reserveTransactionRecord(TransactionRecord transactionRecord);
}