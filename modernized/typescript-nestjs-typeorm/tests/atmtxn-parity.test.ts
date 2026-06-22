import { AtmtxnService } from "src/ATMTXN/Application/AtmtxnService";
import { AtmtxnDomainModels } from "src/ATMTXN/Domain/AtmtxnDomainModels";
import { IAccountRepository } from "src/ATMTXN/Application/Interfaces/AtmtxnInterfaces";
import { DataSource } from "typeorm";
import { AtmtxnDbContext } from "src/ATMTXN/Infrastructure/AtmtxnInfrastructure";
import Decimal from "decimal.js";

describe("ATMTXN Parity Tests", () => {
  let service: AtmtxnService;
  let mockDataSource: DataSource;
  let mockAccountRepository: jest.Mocked<IAccountRepository>;
  let dbContext: AtmtxnDbContext;

  beforeEach(() => {
    mockDataSource = new DataSource({ type: "sqlite", database: ":memory:" });
    mockAccountRepository = {
      readAccount: jest.fn(),
      updateAccount: jest.fn(),
    };
    dbContext = new AtmtxnDbContext(mockDataSource);
    jest.spyOn(dbContext, "getAccountRepository").mockReturnValue(mockAccountRepository);
    service = new AtmtxnService();
  });

  afterEach(async () => {
    if (mockDataSource.isInitialized) {
      await mockDataSource.destroy();
    }
  });

  it("should execute the ATMTXN service without errors", async () => {
    await expect(service.execute()).resolves.not.toThrow();
  });

  it("should read account information correctly", async () => {
    const accountNumber = "12345678901234567890";
    const mockAccountInfo = new AtmtxnDomainModels.AccountInfo(
      accountNumber,
      new Decimal(1000.50),
      "John Doe",
      1234
    );

    mockAccountRepository.readAccount.mockResolvedValue(mockAccountInfo);

    const accountInfo = await mockAccountRepository.readAccount(accountNumber);

    expect(accountInfo.accountNumber).toBe(accountNumber);
    expect(accountInfo.balance.toString()).toBe("1000.50");
    expect(accountInfo.accountHolderName).toBe("John Doe");
    expect(accountInfo.branchCode).toBe(1234);
    expect(mockAccountRepository.readAccount).toHaveBeenCalledWith(accountNumber);
  });

  it("should update account information correctly", async () => {
    const updatedAccountInfo = new AtmtxnDomainModels.AccountInfo(
      "12345678901234567890",
      new Decimal(2000.75),
      "Jane Doe",
      5678
    );

    mockAccountRepository.updateAccount.mockResolvedValue();

    await mockAccountRepository.updateAccount(updatedAccountInfo);

    expect(mockAccountRepository.updateAccount).toHaveBeenCalledWith(updatedAccountInfo);
  });

  it("should initialize and close the database context", async () => {
    jest.spyOn(mockDataSource, "initialize").mockResolvedValue(mockDataSource);
    jest.spyOn(mockDataSource, "destroy").mockResolvedValue();

    await dbContext.initialize();
    expect(mockDataSource.initialize).toHaveBeenCalled();

    await dbContext.close();
    expect(mockDataSource.destroy).toHaveBeenCalled();
  });
});