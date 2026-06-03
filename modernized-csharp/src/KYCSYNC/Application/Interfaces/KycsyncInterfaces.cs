using System;

namespace KYCSYNC.Application.Interfaces
{
    /// <summary>
    /// Interface for the KYCSYNC application logic.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface IKycsyncApplication
    {
        /// <summary>
        /// Main entry point for the KYCSYNC program.
        /// Corresponds to the 0000-MAIN paragraph in COBOL.
        /// </summary>
        void Main();

        /// <summary>
        /// Fetch loop logic for processing customer records.
        /// Corresponds to the 1000-FETCH-LOOP paragraph in COBOL.
        /// </summary>
        void FetchLoop();

        /// <summary>
        /// Logic for upserting KYC data into the database.
        /// Corresponds to the 2000-UPSERT-KYC paragraph in COBOL.
        /// </summary>
        void UpsertKyc();
    }
}