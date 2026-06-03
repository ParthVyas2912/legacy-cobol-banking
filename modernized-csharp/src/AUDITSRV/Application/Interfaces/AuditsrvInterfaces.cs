using System;
using System.Threading.Tasks;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    /// <summary>
    /// Interface for repository operations related to the AUDIT-FILE.
    /// </summary>
    public interface IAuditFileRepository
    {
        /// <summary>
        /// Reads an audit record from the AUDIT-FILE.
        /// </summary>
        /// <param name="auditKey">The primary key of the audit record to read.</param>
        /// <returns>The audit record if found, otherwise null.</returns>
        Task<AuditFile> ReadAuditRecordAsync(string auditKey);
    }

    /// <summary>
    /// Interface for CICS service operations used in the AUDITSRV program.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Writes a response back to the CICS terminal.
        /// </summary>
        /// <param name="response">The response message to write.</param>
        Task WriteResponseAsync(string response);

        /// <summary>
        /// Returns control to the CICS environment.
        /// </summary>
        Task ReturnControlAsync();
    }

    /// <summary>
    /// Interface representing the main entry point for the AUDITSRV program.
    /// </summary>
    public interface IAuditsrvService
    {
        /// <summary>
        /// Main entry point for the AUDITSRV program.
        /// </summary>
        Task StartAsync();
    }
}