import Decimal from "decimal.js";

/**
 * Represents the domain models for the ATMTXN program.
 */
export class AtmtxnDomainModels {
  /**
   * Account information structure.
   */
  export class AccountInfo {
    // PIC X(20)
    accountNumber: string; // MaxLength(20)

    // PIC S9(13)V99 COMP-3
    balance: Decimal; // Precision (15, 2)

    // PIC X(30)
    accountHolderName: string; // MaxLength(30)

    // PIC 9(4)
    branchCode: number; // MaxLength(4)

    constructor(
      accountNumber: string,
      balance: Decimal,
      accountHolderName: string,
      branchCode: number
    ) {
      this.accountNumber = accountNumber;
      this.balance = balance;
      this.accountHolderName = accountHolderName;
      this.branchCode = branchCode;
    }
  }
}