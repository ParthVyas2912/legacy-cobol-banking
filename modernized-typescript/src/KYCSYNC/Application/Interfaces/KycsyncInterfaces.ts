import { KycsyncDomainModels } from 'src/KYCSYNC/Domain/KycsyncDomainModels';

export interface KycsyncService {
    /**
     * Main entry point for the KYCSYNC program.
     * This method orchestrates the overall flow of the program.
     */
    main(): Promise<void>;

    /**
     * Handles the fetch loop logic.
     * This method iterates over the data to be processed.
     */
    fetchLoop(): Promise<void>;

    /**
     * Handles the upsert operation for KYC data.
     * This method updates or inserts KYC records into the database.
     * @param kycData - The KYC data to be upserted.
     */
    upsertKyc(kycData: KycsyncDomainModels): Promise<void>;
}