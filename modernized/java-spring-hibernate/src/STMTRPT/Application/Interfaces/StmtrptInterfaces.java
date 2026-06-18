package src.STMTRPT.Application.Interfaces;

import src.STMTRPT.Domain.StmtrptDomainModels.MainRecord;
import src.STMTRPT.Domain.StmtrptDomainModels.TransactionDetail;

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
     * Reads the input file and loads records into memory.
     *
     * @return List of MainRecord objects representing the input data.
     */
    List<MainRecord> readInputFile();

    /**
     * Processes each record and performs necessary calculations or transformations.
     *
     * @param record The MainRecord object to process.
     */
    void processRecord(MainRecord record);

    /**
     * Writes the processed records to the output file.
     *
     * @param records List of MainRecord objects to write to the output.
     */
    void writeOutputFile(List<MainRecord> records);

    /**
     * Generates a summary report based on the processed data.
     *
     * @param records List of MainRecord objects to summarize.
     */
    void generateSummaryReport(List<MainRecord> records);

    /**
     * Handles any cleanup operations required after processing.
     */
    void cleanup();
}