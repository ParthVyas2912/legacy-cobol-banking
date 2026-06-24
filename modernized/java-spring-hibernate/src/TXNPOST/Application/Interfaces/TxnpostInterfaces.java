package TXNPOST.Application.Interfaces;

import TXNPOST.Domain.TxnpostDomainModels.TransactionBatch;
import TXNPOST.Domain.TxnpostDomainModels.TransactionRecord;
import TXNPOST.Domain.TxnpostDomainModels.BatchHeader;
import TXNPOST.Domain.TxnpostDomainModels.BatchTrailer;

import java.util.List;

public interface TxnpostInterfaces {

    /**
     * Initialize the transaction posting process.
     */
    void initializeTransactionPosting();

    /**
     * Read the batch header record.
     * 
     * @return The batch header record.
     */
    BatchHeader readBatchHeader();

    /**
     * Read the next transaction record from the batch.
     * 
     * @return The next transaction record.
     */
    TransactionRecord readNextTransactionRecord();

    /**
     * Process a single transaction record.
     * 
     * @param transactionRecord The transaction record to process.
     */
    void processTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Write the processed transaction record to the output file.
     * 
     * @param transactionRecord The processed transaction record.
     */
    void writeTransactionRecord(TransactionRecord transactionRecord);

    /**
     * Read the batch trailer record.
     * 
     * @return The batch trailer record.
     */
    BatchTrailer readBatchTrailer();

    /**
     * Validate the batch totals.
     * 
     * @param batchHeader The batch header record.
     * @param batchTrailer The batch trailer record.
     * @param transactionRecords The list of transaction records.
     */
    void validateBatchTotals(BatchHeader batchHeader, BatchTrailer batchTrailer, List<TransactionRecord> transactionRecords);

    /**
     * Finalize the transaction posting process.
     */
    void finalizeTransactionPosting();

    /**
     * Handle errors encountered during the transaction posting process.
     * 
     * @param errorMessage The error message to handle.
     */
    void handleError(String errorMessage);

    /**
     * Commit the transaction batch to the database.
     * 
     * @param transactionBatch The transaction batch to commit.
     */
    void commitTransactionBatch(TransactionBatch transactionBatch);

    /**
     * Rollback the transaction batch in case of errors.
     */
    void rollbackTransactionBatch();
}