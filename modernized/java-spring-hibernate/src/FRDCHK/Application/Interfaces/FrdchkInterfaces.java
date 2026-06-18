package src.FRDCHK.Application.Interfaces;

import src.FRDCHK.Domain.FrdchkDomainModels.FrdchkRecord;
import java.util.List;

/**
 * Application interfaces for the FRDCHK program.
 * Each COBOL paragraph is represented as a method.
 * File I/O operations are represented as repository interfaces.
 * CICS operations are represented as service interfaces.
 */
public interface FrdchkInterfaces {

    /**
     * Reads input records from the input file.
     * @return A list of FrdchkRecord objects representing the input records.
     */
    List<FrdchkRecord> readInputFile();

    /**
     * Writes output records to the output file.
     * @param records A list of FrdchkRecord objects to be written to the output file.
     */
    void writeOutputFile(List<FrdchkRecord> records);

    /**
     * Validates the transaction code in the record.
     * @param record The FrdchkRecord to validate.
     * @return True if the transaction code is valid, false otherwise.
     */
    boolean validateTransactionCode(FrdchkRecord record);

    /**
     * Validates the transaction amount in the record.
     * @param record The FrdchkRecord to validate.
     * @return True if the transaction amount is valid, false otherwise.
     */
    boolean validateTransactionAmount(FrdchkRecord record);

    /**
     * Updates the transaction status in the record based on validation results.
     * @param record The FrdchkRecord to update.
     */
    void updateTransactionStatus(FrdchkRecord record);

    /**
     * Processes a single record by performing validation and updating its status.
     * @param record The FrdchkRecord to process.
     */
    void processRecord(FrdchkRecord record);

    /**
     * Processes all records from the input file and writes the results to the output file.
     */
    void processAllRecords();
}