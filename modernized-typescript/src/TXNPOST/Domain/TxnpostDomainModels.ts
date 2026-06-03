import Decimal from 'decimal.js';

export class TxnpostDomainModels {
    // Represents the WS-RECORD-COUNT field
    // PIC 9(09) COMP-3
    wsRecordCount: Decimal;

    // Represents the WS-FAIL-COUNT field
    // PIC 9(09) COMP-3
    wsFailCount: Decimal;

    constructor(wsRecordCount: Decimal, wsFailCount: Decimal) {
        this.wsRecordCount = wsRecordCount;
        this.wsFailCount = wsFailCount;
    }
}