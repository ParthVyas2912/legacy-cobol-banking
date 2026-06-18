package src.BCHEOD.Application.Interfaces;

import src.BCHEOD.Domain.BcheodDomainModels.MainRecord;

/**
 * Application interfaces for BCHEOD program.
 * Each COBOL paragraph is represented as a method.
 */
public interface BcheodInterfaces {

    /**
     * Initializes the program and sets up necessary resources.
     */
    void initializeProgram();

    /**
     * Reads the input file and processes each record.
     */
    void processInputFile();

    /**
     * Processes a single record from the input file.
     * 
     * @param record The record to process.
     */
    void processRecord(MainRecord record);

    /**
     * Writes the processed record to the output file.
     * 
     * @param record The record to write.
     */
    void writeOutputRecord(MainRecord record);

    /**
     * Finalizes the program, closes resources, and performs cleanup.
     */
    void finalizeProgram();
}