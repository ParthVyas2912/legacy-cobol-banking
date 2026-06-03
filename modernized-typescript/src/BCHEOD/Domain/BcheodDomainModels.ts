import Decimal from 'decimal.js';

export namespace BcheodDomainModels {
    // Enums for 88-level values
    export enum RunStatus {
        NotStarted = '0', // PIC X(1)
        InProgress = '1', // PIC X(1)
        Completed = '2',  // PIC X(1)
        Failed = '3'      // PIC X(1)
    }

    // Group structure for EOD_RUNS table
    export class EodRun {
        // PIC 9(8)
        public runDate: number;

        // PIC 9(4)
        public runTime: number;

        // PIC X(1)
        public status: RunStatus;

        // PIC S9(13)V99 COMP-3
        public totalAmount: Decimal;

        // PIC X(20)
        public description: string;

        constructor(
            runDate: number,
            runTime: number,
            status: RunStatus,
            totalAmount: Decimal,
            description: string
        ) {
            this.runDate = runDate;
            this.runTime = runTime;
            this.status = status;
            this.totalAmount = totalAmount;
            this.description = description;
        }
    }

    // Example of OCCURS clause (array of EOD_RUNS)
    export class EodRunBatch {
        // OCCURS 10 TIMES
        public runs: EodRun[];

        constructor(runs: EodRun[]) {
            if (runs.length > 10) {
                throw new Error('EodRunBatch can only contain up to 10 runs.');
            }
            this.runs = runs;
        }
    }
}