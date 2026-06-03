using System;

namespace BCHEOD.Application.Interfaces
{
    /// <summary>
    /// Interface for the BCHEOD application logic.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface IBcheodApplication
    {
        /// <summary>
        /// Corresponds to the 0000-DRIVE paragraph in the COBOL program.
        /// </summary>
        void Drive();

        /// <summary>
        /// Corresponds to the 1000-NEXT-STEP paragraph in the COBOL program.
        /// </summary>
        void NextStep();

        /// <summary>
        /// Corresponds to the 2000-INT-STEP paragraph in the COBOL program.
        /// </summary>
        void IntStep();

        /// <summary>
        /// Corresponds to the 3000-POST-STEP paragraph in the COBOL program.
        /// </summary>
        void PostStep();

        /// <summary>
        /// Corresponds to the 4000-DONE paragraph in the COBOL program.
        /// </summary>
        void Done();

        /// <summary>
        /// Corresponds to the 9000-MARK-START paragraph in the COBOL program.
        /// </summary>
        void MarkStart();

        /// <summary>
        /// Corresponds to the 9100-MARK-END paragraph in the COBOL program.
        /// </summary>
        void MarkEnd();
    }

    /// <summary>
    /// Interface for repository operations related to EOD_RUNS table.
    /// </summary>
    public interface IEodRunsRepository
    {
        /// <summary>
        /// Fetches an EOD run by its Run ID.
        /// </summary>
        /// <param name="runId">The Run ID to fetch.</param>
        /// <returns>An EodRun object representing the run details.</returns>
        BCHEOD.Domain.BcheodDomainModels.EodRun GetEodRunById(string runId);

        /// <summary>
        /// Updates an existing EOD run in the database.
        /// </summary>
        /// <param name="eodRun">The EodRun object containing updated details.</param>
        void UpdateEodRun(BCHEOD.Domain.BcheodDomainModels.EodRun eodRun);

        /// <summary>
        /// Inserts a new EOD run into the database.
        /// </summary>
        /// <param name="eodRun">The EodRun object to insert.</param>
        void InsertEodRun(BCHEOD.Domain.BcheodDomainModels.EodRun eodRun);
    }

    /// <summary>
    /// Interface for external service calls made by the BCHEOD application.
    /// </summary>
    public interface IBcheodExternalService
    {
        /// <summary>
        /// Calls the INTCALC service.
        /// </summary>
        /// <param name="parameters">The parameters required by the INTCALC service.</param>
        void CallIntCalcService(object parameters);

        /// <summary>
        /// Calls the TXNPOST service.
        /// </summary>
        /// <param name="parameters">The parameters required by the TXNPOST service.</param>
        void CallTxnPostService(object parameters);
    }
}