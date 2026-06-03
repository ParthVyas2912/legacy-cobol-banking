import Decimal from 'decimal.js';

export namespace FrdchkDomainModels {
    /**
     * Represents the transaction record structure.
     */
    export class TxnRecord {
        // PIC S9(11)V99 COMP-3
        txnAmount: Decimal;

        // PIC S9(13)V99 COMP-3
        txnBalanceAfter: Decimal;

        constructor(txnAmount: Decimal, txnBalanceAfter: Decimal) {
            this.txnAmount = txnAmount;
            this.txnBalanceAfter = txnBalanceAfter;
        }
    }

    /**
     * Represents the working storage fields used in the FRDCHK program.
     */
    export class WorkingStorage {
        // PIC 9(05) COMP-3
        wsVelocityCount: Decimal;

        // PIC S9(11)V99 COMP-3
        wsVelocityAmt: Decimal;

        // PIC 9(03) COMP-3
        wsRiskScore: Decimal;

        constructor(wsVelocityCount: Decimal, wsVelocityAmt: Decimal, wsRiskScore: Decimal) {
            this.wsVelocityCount = wsVelocityCount;
            this.wsVelocityAmt = wsVelocityAmt;
            this.wsRiskScore = wsRiskScore;
        }
    }
}