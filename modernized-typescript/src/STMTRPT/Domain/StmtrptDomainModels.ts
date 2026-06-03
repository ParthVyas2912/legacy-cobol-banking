import Decimal from 'decimal.js';

export class StmtrptDomainModels {
    // Customer Record
    static CustomerRecord = class {
        // PIC X(10)
        public CUST_ID: string; // Customer ID
        // PIC X(30)
        public CUST_NAME: string; // Customer Name
        // PIC 9(03) COMP-3
        public CUST_RISK_SCORE: Decimal; // Customer Risk Score
        // PIC 9(03) COMP-3
        public CUST_ACCT_COUNT: Decimal; // Customer Account Count

        constructor(
            CUST_ID: string,
            CUST_NAME: string,
            CUST_RISK_SCORE: Decimal,
            CUST_ACCT_COUNT: Decimal
        ) {
            this.CUST_ID = CUST_ID;
            this.CUST_NAME = CUST_NAME;
            this.CUST_RISK_SCORE = CUST_RISK_SCORE;
            this.CUST_ACCT_COUNT = CUST_ACCT_COUNT;
        }
    };

    // IMS Segment: CUST-STATUS
    static CustStatus = class {
        // PIC X(20)
        public STATUS_DESC: string; // Status Description

        constructor(STATUS_DESC: string) {
            this.STATUS_DESC = STATUS_DESC;
        }
    };

    // IMS Segment: CUST-ID
    static CustId = class {
        // PIC X(10)
        public ID: string; // Customer ID

        constructor(ID: string) {
            this.ID = ID;
        }
    };

    // IMS Segment: CUST-SSA-VAL
    static CustSsaVal = class {
        // PIC X(50)
        public SSA_VALUE: string; // SSA Value

        constructor(SSA_VALUE: string) {
            this.SSA_VALUE = SSA_VALUE;
        }
    };

    // IMS Segment: 2000-OPEN-CURSOR
    static OpenCursor = class {
        // PIC X(30)
        public CURSOR_NAME: string; // Cursor Name

        constructor(CURSOR_NAME: string) {
            this.CURSOR_NAME = CURSOR_NAME;
        }
    };

    // IMS Segment: 3000-FETCH-LOOP
    static FetchLoop = class {
        // PIC X(50)
        public FETCH_DATA: string; // Fetch Data

        constructor(FETCH_DATA: string) {
            this.FETCH_DATA = FETCH_DATA;
        }
    };

    // IMS Segment: SQLCODE
    static SqlCode = class {
        // PIC 9(05)
        public CODE: number; // SQL Code

        constructor(CODE: number) {
            this.CODE = CODE;
        }
    };
}