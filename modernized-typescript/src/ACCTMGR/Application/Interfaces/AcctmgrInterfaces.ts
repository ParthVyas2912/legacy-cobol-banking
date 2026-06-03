import { AcctmgrDomainModels } from 'src/ACCTMGR/Domain/AcctmgrDomainModels';

export namespace AcctmgrInterfaces {
    // Repository interface for file I/O operations
    export interface IAccountRepository {
        readAccount(acctNumber: number): AcctmgrDomainModels.AccountRecord | null;
        writeAccount(account: AcctmgrDomainModels.AccountRecord): void;
    }

    // Service interface for CICS operations
    export interface ICicsService {
        returnControl(): void;
        writeAuditRecord(record: any): void;
        readCustomer(custId: number): AcctmgrDomainModels.CustomerRecord | null;
        rewriteAccount(account: AcctmgrDomainModels.AccountRecord): void;
    }

    // Main application interface
    export interface IAcctmgrApplication {
        start(): void;
        createAccount(account: AcctmgrDomainModels.AccountRecord): void;
        readAccount(acctNumber: number): AcctmgrDomainModels.AccountRecord | null;
        updateAccount(account: AcctmgrDomainModels.AccountRecord): void;
        applyUpdates(account: AcctmgrDomainModels.AccountRecord): void;
        closeAccount(acctNumber: number): void;
    }
}