import { Decimal } from "decimal.js";

// Domain model for AUDITSRV program
export namespace AuditsrvDomainModels {
    // Root group structure
    export class AuditRecord {
        // PIC X(10)
        public auditId: string; // MaxLength(10)

        // PIC X(20)
        public userId: string; // MaxLength(20)

        // PIC 9(8)
        public timestamp: number; // YYYYMMDD format

        // PIC 9(6)
        public time: number; // HHMMSS format

        // PIC X(50)
        public action: string; // MaxLength(50)

        // PIC 9(13)V99 COMP-3
        public amount: Decimal; // Precision (15, 2)

        // PIC X(100)
        public description: string; // MaxLength(100)

        // PIC 9(5)
        public status: number; // Integer field

        // 88-level for status
        public static StatusEnum = {
            SUCCESS: 0, // 88 SUCCESS VALUE 0
            FAILURE: 1, // 88 FAILURE VALUE 1
        };

        constructor(
            auditId: string,
            userId: string,
            timestamp: number,
            time: number,
            action: string,
            amount: Decimal,
            description: string,
            status: number
        ) {
            this.auditId = auditId;
            this.userId = userId;
            this.timestamp = timestamp;
            this.time = time;
            this.action = action;
            this.amount = amount;
            this.description = description;
            this.status = status;
        }
    }

    // Example of a nested group structure
    export class AuditGroup {
        // PIC X(10)
        public groupId: string; // MaxLength(10)

        // PIC X(30)
        public groupName: string; // MaxLength(30)

        // OCCURS 10 TIMES
        public auditRecords: AuditRecord[]; // Array of AuditRecord, size 10

        constructor(groupId: string, groupName: string, auditRecords: AuditRecord[]) {
            this.groupId = groupId;
            this.groupName = groupName;
            this.auditRecords = auditRecords;
        }
    }
}