import Decimal from 'decimal.js';

export namespace AuditsrvDomainModels {
    /**
     * Represents the structure of the AUDIT-FILE record in COBOL.
     */
    export class AuditFile {
        /**
         * Primary key for the audit record.
         * COBOL PIC clause: PIC X(20)
         */
        auditKey: string; // MaxLength(20)

        /**
         * Account number associated with the audit record.
         * COBOL PIC clause: PIC 9(18)
         */
        auditAcctNumber: bigint;

        /**
         * Action performed in the audit record.
         * COBOL PIC clause: PIC X(16)
         */
        auditAction: string; // MaxLength(16)

        /**
         * User who performed the action in the audit record.
         * COBOL PIC clause: PIC X(8)
         */
        auditUser: string; // MaxLength(8)

        /**
         * Timestamp of the audit record.
         * COBOL PIC clause: PIC X(26)
         */
        auditTs: string; // MaxLength(26)

        /**
         * Detailed description of the audit record.
         * COBOL PIC clause: PIC X(120)
         */
        auditDetail: string; // MaxLength(120)

        constructor(
            auditKey: string,
            auditAcctNumber: bigint,
            auditAction: string,
            auditUser: string,
            auditTs: string,
            auditDetail: string
        ) {
            this.auditKey = auditKey;
            this.auditAcctNumber = auditAcctNumber;
            this.auditAction = auditAction;
            this.auditUser = auditUser;
            this.auditTs = auditTs;
            this.auditDetail = auditDetail;
        }
    }
}