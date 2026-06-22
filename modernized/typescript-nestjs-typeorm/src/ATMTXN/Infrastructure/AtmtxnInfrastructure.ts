import { DataSource } from "typeorm";
import { Repository } from "typeorm";
import { AtmtxnDomainModels } from "src/ATMTXN/Domain/AtmtxnDomainModels";
import { IAccountRepository } from "src/ATMTXN/Application/Interfaces/AtmtxnInterfaces";
import Decimal from "decimal.js";

// Entity definition for the AccountInfo table
import { Entity, Column, PrimaryColumn } from "typeorm";

/**
 * Entity representing the AccountInfo table in the database.
 */
@Entity({ name: "account_info" })
export class AccountInfoEntity {
  @PrimaryColumn({ name: "account_number", type: "varchar", length: 20 })
  // COBOL: accountNumber PIC X(20)
  accountNumber!: string;

  @Column({ name: "balance", type: "decimal", precision: 15, scale: 2 })
  // COBOL: balance PIC S9(13)V99 COMP-3
  balance!: Decimal;

  @Column({ name: "account_holder_name", type: "varchar", length: 30 })
  // COBOL: accountHolderName PIC X(30)
  accountHolderName!: string;

  @Column({ name: "branch_code", type: "int", width: 4 })
  // COBOL: branchCode PIC 9(4)
  branchCode!: number;
}

/**
 * Repository implementation for account-related operations.
 */
export class AccountRepository implements IAccountRepository {
  private readonly repository: Repository<AccountInfoEntity>;

  constructor(dataSource: DataSource) {
    this.repository = dataSource.getRepository(AccountInfoEntity);
  }

  /**
   * Reads account information based on the account number.
   * @param accountNumber The account number to retrieve information for.
   * @returns A promise that resolves to the account information.
   */
  async readAccount(accountNumber: string): Promise<AtmtxnDomainModels.AccountInfo> {
    const entity = await this.repository.findOneBy({ accountNumber });
    if (!entity) {
      throw new Error(`Account with accountNumber ${accountNumber} not found.`);
    }
    return new AtmtxnDomainModels.AccountInfo(
      entity.accountNumber,
      entity.balance,
      entity.accountHolderName,
      entity.branchCode
    );
  }

  /**
   * Updates the account information in the repository.
   * @param accountInfo The account information to update.
   * @returns A promise that resolves when the update is complete.
   */
  async updateAccount(accountInfo: AtmtxnDomainModels.AccountInfo): Promise<void> {
    const entity = new AccountInfoEntity();
    entity.accountNumber = accountInfo.accountNumber;
    entity.balance = accountInfo.balance;
    entity.accountHolderName = accountInfo.accountHolderName;
    entity.branchCode = accountInfo.branchCode;

    await this.repository.save(entity);
  }
}

/**
 * Database context for the ATMTXN program.
 */
export class AtmtxnDbContext {
  private readonly dataSource: DataSource;

  constructor(dataSource: DataSource) {
    this.dataSource = dataSource;
  }

  /**
   * Initializes the database connection.
   */
  async initialize(): Promise<void> {
    if (!this.dataSource.isInitialized) {
      await this.dataSource.initialize();
    }
  }

  /**
   * Closes the database connection.
   */
  async close(): Promise<void> {
    if (this.dataSource.isInitialized) {
      await this.dataSource.destroy();
    }
  }

  /**
   * Provides the account repository.
   * @returns The account repository instance.
   */
  getAccountRepository(): IAccountRepository {
    return new AccountRepository(this.dataSource);
  }
}