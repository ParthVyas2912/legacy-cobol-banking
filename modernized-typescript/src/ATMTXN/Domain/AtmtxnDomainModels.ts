import { Decimal } from 'decimal.js';

export namespace AtmtxnDomainModels {
    /**
     * Represents the transaction record structure.
     */
    export class TransactionRecord {
        // PIC S9(11)V99 COMP-3
        txnAmount: Decimal; 

        // PIC S9(13)V99 COMP-3
        txnBalanceAfter: Decimal; 

        constructor(txnAmount: Decimal, txnBalanceAfter: Decimal) {
            this.txnAmount = txnAmount;
            this.txnBalanceAfter = txnBalanceAfter;
        }
    }
}