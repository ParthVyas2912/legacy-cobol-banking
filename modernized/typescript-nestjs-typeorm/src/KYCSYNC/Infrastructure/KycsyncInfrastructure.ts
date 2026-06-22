import { Pool } from 'pg';
import Decimal from 'decimal.js';
import { KycsyncDomainModels } from 'src/KYCSYNC/Domain/KycsyncDomainModels';
import { IKycsyncRepository } from 'src/KYCSYNC/Application/Interfaces/KycsyncInterfaces';

export class KycsyncRepository implements IKycsyncRepository {
    private readonly db: Pool;

    constructor(db: Pool) {
        this.db = db;
    }

    async readCustomerRecords(): Promise<KycsyncDomainModels.CustomerRecord[]> {
        const query = `
            SELECT 
                cust_id, 
                cust_name, 
                cust_account_balance, 
                cust_account_status 
            FROM public.kyc_audit;
        `;
        const result = await this.db.query(query);

        return result.rows.map(row => new KycsyncDomainModels.CustomerRecord(
            parseInt(row.cust_id, 10),
            row.cust_name,
            new Decimal(row.cust_account_balance),
            row.cust_account_status as KycsyncDomainModels.AccountStatus
        ));
    }

    async writeCustomerRecords(records: KycsyncDomainModels.CustomerRecord[]): Promise<void> {
        const query = `
            INSERT INTO public.kyc_audit (cust_id, cust_name, cust_account_balance, cust_account_status)
            VALUES ($1, $2, $3, $4)
            ON CONFLICT (cust_id) DO UPDATE
            SET cust_name = EXCLUDED.cust_name,
                cust_account_balance = EXCLUDED.cust_account_balance,
                cust_account_status = EXCLUDED.cust_account_status;
        `;

        for (const record of records) {
            await this.db.query(query, [
                record.customerId,
                record.customerName,
                record.accountBalance.toString(),
                record.accountStatus
            ]);
        }
    }

    async readTransactionHistory(customerId: number): Promise<KycsyncDomainModels.TransactionHistory> {
        const query = `
            SELECT 
                transaction_id, 
                transaction_amount, 
                transaction_date 
            FROM public.transaction_history
            WHERE customer_id = $1;
        `;
        const result = await this.db.query(query, [customerId]);

        const transactions = result.rows.map(row => new KycsyncDomainModels.Transaction(
            parseInt(row.transaction_id, 10),
            new Decimal(row.transaction_amount),
            row.transaction_date
        ));

        return new KycsyncDomainModels.TransactionHistory(transactions);
    }

    async writeTransactionHistory(customerId: number, history: KycsyncDomainModels.TransactionHistory): Promise<void> {
        const query = `
            INSERT INTO public.transaction_history (customer_id, transaction_id, transaction_amount, transaction_date)
            VALUES ($1, $2, $3, $4)
            ON CONFLICT (transaction_id) DO UPDATE
            SET transaction_amount = EXCLUDED.transaction_amount,
                transaction_date = EXCLUDED.transaction_date;
        `;

        for (const transaction of history.transactions) {
            await this.db.query(query, [
                customerId,
                transaction.transactionId,
                transaction.transactionAmount.toString(),
                transaction.transactionDate
            ]);
        }
    }
}