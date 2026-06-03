using System;
using System.Threading.Tasks;

namespace FRDCHK.Application.Interfaces
{
    /// <summary>
    /// Interface defining the operations for the FRDCHK application.
    /// </summary>
    public interface IFrdchkService
    {
        /// <summary>
        /// Entry point for the FRDCHK program.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Computes the velocity metrics for transactions.
        /// </summary>
        Task ComputeVelocityAsync();

        /// <summary>
        /// Scores the risk associated with transactions.
        /// </summary>
        Task ScoreRiskAsync();

        /// <summary>
        /// Freezes an account based on risk assessment.
        /// </summary>
        Task FreezeAccountAsync();
    }
}