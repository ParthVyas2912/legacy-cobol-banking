import Decimal from 'decimal.js';

export namespace IntcalcDomainModels {
    // Represents the WS-DAILY-RATE field
    export class WsDailyRate {
        private value: Decimal;

        constructor(value: string | number | Decimal) {
            this.value = new Decimal(value);
        }

        getValue(): Decimal {
            return this.value;
        }

        setValue(value: string | number | Decimal): void {
            this.value = new Decimal(value);
        }
    }

    // Represents the WS-DAILY-INTEREST field
    export class WsDailyInterest {
        private value: Decimal;

        constructor(value: string | number | Decimal) {
            this.value = new Decimal(value);
        }

        getValue(): Decimal {
            return this.value;
        }

        setValue(value: string | number | Decimal): void {
            this.value = new Decimal(value);
        }
    }

    // Represents the WS-NEW-BALANCE field
    export class WsNewBalance {
        private value: Decimal;

        constructor(value: string | number | Decimal) {
            this.value = new Decimal(value);
        }

        getValue(): Decimal {
            return this.value;
        }

        setValue(value: string | number | Decimal): void {
            this.value = new Decimal(value);
        }
    }

    // Represents the WS-NEXT-TXN-ID field
    export class WsNextTxnId {
        private value: Decimal;

        constructor(value: string | number | Decimal) {
            this.value = new Decimal(value);
        }

        getValue(): Decimal {
            return this.value;
        }

        setValue(value: string | number | Decimal): void {
            this.value = new Decimal(value);
        }
    }

    // Represents the ACCT-FILE record structure
    export class AcctFile {
        acctNumber: number; // PIC 9(12)
        acctCustomerId: number; // PIC 9(12)
        acctOpenedDate: number; // PIC 9(8)
        acctType: string; // PIC X(2)
        acctStatus: string; // PIC X(1)
        acctBalance: Decimal; // PIC S9(13)V99 COMP-3
        acctAvailBal: Decimal; // PIC S9(13)V99 COMP-3
        acctIntRate: Decimal; // PIC 9(6)V9(4) COMP-3
        acctLastTxnDt: number; // PIC 9(8)
        acctCurrency: string; // PIC X(3)
        acctBranchCode: number; // PIC 9(5)
        acctFlags: string; // PIC X(4)
        acctFiller: string; // PIC X(50)

        constructor(
            acctNumber: number,
            acctCustomerId: number,
            acctOpenedDate: number,
            acctType: string,
            acctStatus: string,
            acctBalance: Decimal,
            acctAvailBal: Decimal,
            acctIntRate: Decimal,
            acctLastTxnDt: number,
            acctCurrency: string,
            acctBranchCode: number,
            acctFlags: string,
            acctFiller: string
        ) {
            this.acctNumber = acctNumber;
            this.acctCustomerId = acctCustomerId;
            this.acctOpenedDate = acctOpenedDate;
            this.acctType = acctType;
            this.acctStatus = acctStatus;
            this.acctBalance = acctBalance;
            this.acctAvailBal = acctAvailBal;
            this.acctIntRate = acctIntRate;
            this.acctLastTxnDt = acctLastTxnDt;
            this.acctCurrency = acctCurrency;
            this.acctBranchCode = acctBranchCode;
            this.acctFlags = acctFlags;
            this.acctFiller = acctFiller;
        }
    }
}