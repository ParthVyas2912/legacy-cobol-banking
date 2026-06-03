import Decimal from 'decimal.js';

export class KycsyncDomainModels {
    // Represents the CUST-RISK-SCORE field
    // PIC 9(03) COMP-3
    custRiskScore: Decimal;

    // Represents the CUST-ACCT-COUNT field
    // PIC 9(03) COMP-3
    custAcctCount: Decimal;

    // Represents the WS-NEXT-COUNT field
    // PIC 9(09) COMP-3
    wsNextCount: Decimal;

    constructor(custRiskScore: string, custAcctCount: string, wsNextCount: string) {
        this.custRiskScore = new Decimal(custRiskScore);
        this.custAcctCount = new Decimal(custAcctCount);
        this.wsNextCount = new Decimal(wsNextCount);
    }
}