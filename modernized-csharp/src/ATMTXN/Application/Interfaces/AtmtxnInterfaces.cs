using System;

namespace ATMTXN.Application.Interfaces
{
    /// <summary>
    /// Interface defining the application logic for the ATMTXN program.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface IAtmtxnApplicationService
    {
        /// <summary>
        /// Entry point for the ATMTXN program.
        /// Corresponds to the 0000-START paragraph.
        /// </summary>
        void Start();

        /// <summary>
        /// Validates the transaction details.
        /// Corresponds to the 1000-VALIDATE paragraph.
        /// </summary>
        void Validate();

        /// <summary>
        /// Posts the transaction to the system.
        /// Corresponds to the 2000-POST-TXN paragraph.
        /// </summary>
        void PostTransaction();

        /// <summary>
        /// Sends the next response or message.
        /// Corresponds to the 3000-SEND-NEXT paragraph.
        /// </summary>
        void SendNext();

        /// <summary>
        /// Maps and handles errors.
        /// Corresponds to the 9000-MAP-ERROR paragraph.
        /// </summary>
        void MapError();

        /// <summary>
        /// Handles fatal errors and terminates the program.
        /// Corresponds to the 9100-FATAL paragraph.
        /// </summary>
        void Fatal();
    }

    /// <summary>
    /// Interface defining CICS operations for the ATMTXN program.
    /// </summary>
    public interface IAtmtxnCicsService
    {
        /// <summary>
        /// Handles CICS HANDLE command.
        /// </summary>
        void Handle();

        /// <summary>
        /// Handles CICS RECEIVE command.
        /// </summary>
        void Receive();

        /// <summary>
        /// Handles CICS RETURN command.
        /// </summary>
        void Return();

        /// <summary>
        /// Handles CICS LINK command.
        /// </summary>
        void Link();

        /// <summary>
        /// Handles CICS SEND command.
        /// </summary>
        void Send();

        /// <summary>
        /// Handles CICS ABEND command.
        /// </summary>
        void Abend();
    }
}