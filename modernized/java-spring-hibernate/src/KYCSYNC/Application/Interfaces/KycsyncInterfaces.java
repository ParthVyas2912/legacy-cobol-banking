package src.KYCSYNC.Application.Interfaces;

import src.KYCSYNC.Domain.KycsyncDomainModels.KYCRecord;
import java.util.List;

/**
 * Application interfaces for the KYCSYNC program.
 * Each method corresponds to a COBOL paragraph or operation.
 */
public interface KycsyncInterfaces {

    /**
     * Initializes the KYCSYNC program.
     */
    void initializeProgram();

    /**
     * Reads KYC records from the input source.
     * 
     * @return List of KYC records.
     */
    List<KYCRecord> readKYCRecords();

    /**
     * Processes a single KYC record.
     * 
     * @param record The KYC record to process.
     */
    void processKYCRecord(KYCRecord record);

    /**
     * Writes the processed KYC record to the output.
     * 
     * @param record The processed KYC record.
     */
    void writeKYCRecord(KYCRecord record);

    /**
     * Finalizes the KYCSYNC program.
     */
    void finalizeProgram();
}