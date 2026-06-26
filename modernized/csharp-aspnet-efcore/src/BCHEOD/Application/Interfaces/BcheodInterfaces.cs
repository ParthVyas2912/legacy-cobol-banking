using System;
using System.Collections.Generic;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodFileRepository
    {
        /// <summary>
        /// Reads the BCHEOD file and returns the parsed domain model.
        /// </summary>
        /// <returns>A BcheodFile object representing the file contents.</returns>
        BcheodDomainModels.BcheodFile ReadFile();

        /// <summary>
        /// Writes the BCHEOD file using the provided domain model.
        /// </summary>
        /// <param name="file">A BcheodFile object representing the file contents to be written.</param>
        void WriteFile(BcheodDomainModels.BcheodFile file);
    }

    public interface IBcheodProcessingService
    {
        /// <summary>
        /// Validates the BCHEOD file structure and content.
        /// </summary>
        /// <param name="file">A BcheodFile object to validate.</param>
        /// <returns>A boolean indicating whether the file is valid.</returns>
        bool ValidateFile(BcheodDomainModels.BcheodFile file);

        /// <summary>
        /// Processes the BCHEOD file and performs necessary business logic.
        /// </summary>
        /// <param name="file">A BcheodFile object to process.</param>
        void ProcessFile(BcheodDomainModels.BcheodFile file);

        /// <summary>
        /// Generates a summary report based on the BCHEOD file.
        /// </summary>
        /// <param name="file">A BcheodFile object to generate the report from.</param>
        /// <returns>A string containing the summary report.</returns>
        string GenerateReport(BcheodDomainModels.BcheodFile file);
    }
}