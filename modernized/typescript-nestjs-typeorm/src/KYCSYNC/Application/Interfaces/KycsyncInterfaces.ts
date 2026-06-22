import Decimal from "decimal.js";
import { KycsyncDomainModels } from "src/KYCSYNC/Domain/KycsyncDomainModels";

export interface IKycsyncApplication {
    execute(): Promise<void>;
}

export interface IKycsyncRepository {
    readCustomerRecords(): Promise<KycsyncDomainModels.CustomerRecord[]>;
    writeCustomerRecords(records: KycsyncDomainModels.CustomerRecord[]): Promise<void>;
    readTransactionHistory(customerId: number): Promise<KycsyncDomainModels.TransactionHistory>;
    writeTransactionHistory(customerId: number, history: KycsyncDomainModels.TransactionHistory): Promise<void>;
}

export interface IKycsyncService {
    fetchCustomerData(customerId: number): Promise<KycsyncDomainModels.CustomerRecord>;
    updateCustomerStatus(customerId: number, status: KycsyncDomainModels.AccountStatus): Promise<void>;
    processPaymentDetails(details: KycsyncDomainModels.PaymentDetails): Promise<void>;
}