package src.KYCSYNC.Application.Interfaces;

import src.KYCSYNC.Domain.KycsyncDomainModels.KycRecord;
import java.util.List;

/**
 * Application interfaces for KYCSYNC program.
 * Each COBOL paragraph is represented as a method.
 * File I/O operations are represented as repository interfaces.
 * CICS operations are represented as service interfaces.
 */
public interface KycsyncInterfaces {

    /**
     * Initializes the KYCSYNC process.
     */
    void initializeKycsync();

    /**
     * Reads KYC records from the input file.
     * 
     * @return List of KYC records.
     */
    List<KycRecord> readKycRecords();

    /**
     * Validates the KYC records.
     * 
     * @param records List of KYC records to validate.
     * @return List of valid KYC records.
     */
    List<KycRecord> validateKycRecords(List<KycRecord> records);

    /**
     * Processes the KYC records.
     * 
     * @param records List of KYC records to process.
     */
    void processKycRecords(List<KycRecord> records);

    /**
     * Writes the processed KYC records to the output file.
     * 
     * @param records List of processed KYC records.
     */
    void writeKycRecords(List<KycRecord> records);

    /**
     * Finalizes the KYCSYNC process.
     */
    void finalizeKycsync();
}