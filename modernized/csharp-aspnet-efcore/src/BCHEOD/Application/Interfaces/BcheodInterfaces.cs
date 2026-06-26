using System.Collections.Generic;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodRepository
    {
        /// <summary>
        /// Reads the BCHEOD file and returns its contents as a BcheodFile object.
        /// </summary>
        /// <returns>BcheodFile object containing the file data.</returns>
        BcheodDomainModels.BcheodFile ReadBcheodFile();

        /// <summary>
        /// Writes the given BcheodFile object to the BCHEOD file.
        /// </summary>
        /// <param name="bcheodFile">The BcheodFile object to write.</param>
        void WriteBcheodFile(BcheodDomainModels.BcheodFile bcheodFile);

        /// <summary>
        /// Appends a detail record to the BCHEOD file.
        /// </summary>
        /// <param name="detail">The detail record to append.</param>
        void AppendDetail(BcheodDomainModels.Detail detail);

        /// <summary>
        /// Deletes a detail record from the BCHEOD file by its identifier.
        /// </summary>
        /// <param name="detailId">The identifier of the detail record to delete.</param>
        void DeleteDetail(int detailId);
    }

    public interface IBcheodService
    {
        /// <summary>
        /// Validates the header of the BCHEOD file.
        /// </summary>
        /// <param name="header">The header to validate.</param>
        /// <returns>True if the header is valid, otherwise false.</returns>
        bool ValidateHeader(BcheodDomainModels.Header header);

        /// <summary>
        /// Processes the details of the BCHEOD file.
        /// </summary>
        /// <param name="details">The list of details to process.</param>
        void ProcessDetails(List<BcheodDomainModels.Detail> details);

        /// <summary>
        /// Validates the footer of the BCHEOD file.
        /// </summary>
        /// <param name="footer">The footer to validate.</param>
        /// <returns>True if the footer is valid, otherwise false.</returns>
        bool ValidateFooter(BcheodDomainModels.Footer footer);

        /// <summary>
        /// Calculates the total amount from the list of details.
        /// </summary>
        /// <param name="details">The list of details to calculate the total amount from.</param>
        /// <returns>The calculated total amount.</returns>
        decimal CalculateTotalAmount(List<BcheodDomainModels.Detail> details);

        /// <summary>
        /// Generates a summary report for the BCHEOD file.
        /// </summary>
        /// <param name="bcheodFile">The BCHEOD file to generate the report for.</param>
        /// <returns>A string containing the summary report.</returns>
        string GenerateSummaryReport(BcheodDomainModels.BcheodFile bcheodFile);

        /// <summary>
        /// Retrieves a specific detail record by its identifier.
        /// </summary>
        /// <param name="detailId">The identifier of the detail record to retrieve.</param>
        /// <returns>The detail record with the specified identifier.</returns>
        BcheodDomainModels.Detail GetDetailById(int detailId);

        /// <summary>
        /// Updates a specific detail record in the BCHEOD file.
        /// </summary>
        /// <param name="detail">The detail record to update.</param>
        void UpdateDetail(BcheodDomainModels.Detail detail);
    }
}