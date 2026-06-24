package src.STMTRPT.Application.Interfaces;

import src.STMTRPT.Domain.StmtrptDomainModels.MainRecord;
import src.STMTRPT.Domain.StmtrptDomainModels.Transaction;
import java.util.List;

/**
 * Application interfaces for STMTRPT program.
 */
public interface StmtrptInterfaces {

    /**
     * Initializes the STMTRPT program.
     */
    void initialize();

    /**
     * Reads the main record from the input file.
     * 
     * @return The main record.
     */
    MainRecord readMainRecord();

    /**
     * Processes the main record.
     * 
     * @param mainRecord The main record to process.
     */
    void processMainRecord(MainRecord mainRecord);

    /**
     * Writes the processed main record to the output file.
     * 
     * @param mainRecord The main record to write.
     */
    void writeMainRecord(MainRecord mainRecord);

    /**
     * Reads a list of transactions associated with the main record.
     * 
     * @param accountNumber The account number to retrieve transactions for.
     * @return A list of transactions.
     */
    List<Transaction> readTransactions(String accountNumber);

    /**
     * Processes a single transaction.
     * 
     * @param transaction The transaction to process.
     */
    void processTransaction(Transaction transaction);

    /**
     * Writes a processed transaction to the output file.
     * 
     * @param transaction The transaction to write.
     */
    void writeTransaction(Transaction transaction);

    /**
     * Finalizes the STMTRPT program.
     */
    void finalizeReport();
}