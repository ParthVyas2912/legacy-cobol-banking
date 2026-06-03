import { IntcalcDomainModels } from 'src/INTCALC/Domain/IntcalcDomainModels';
import { IntcalcInterfaces } from 'src/INTCALC/Application/Interfaces/IntcalcInterfaces';
import Decimal from 'decimal.js';
import { Pool } from 'pg';

export class IntcalcInfrastructure {
    private dbPool: Pool;

    constructor(dbPool: Pool) {
        this.dbPool = dbPool;
    }

    public createAcctFileRepository(): IntcalcInterfaces.AcctFileRepository {
        return new AcctFileRepository(this.dbPool);
    }

    public createIntJournalRepository(): IntcalcInterfaces.IntJournalRepository {
        return new IntJournalRepository(this.dbPool);
    }
}

class AcctFileRepository implements IntcalcInterfaces.AcctFileRepository {
    private dbPool: Pool;

    constructor(dbPool: Pool) {
        this.dbPool = dbPool;
    }

    async readAcctFile(acctNumber: number): Promise<IntcalcDomainModels.AcctFile | null> {
        const query = `
            SELECT 
                acct_number, acct_customer_id, acct_opened_date, acct_type, acct_status, 
                acct_balance, acct_avail_bal, acct_int_rate, acct_last_txn_dt, acct_currency, 
                acct_branch_code, acct_flags, acct_filler
            FROM public.acctfile
            WHERE acct_number = $1
        `;
        const result = await this.dbPool.query(query, [acctNumber]);

        if (result.rows.length === 0) {
            return null;
        }

        const row = result.rows[0];
        return new IntcalcDomainModels.AcctFile(
            row.acct_number,
            row.acct_customer_id,
            row.acct_opened_date,
            row.acct_type,
            row.acct_status,
            new Decimal(row.acct_balance),
            new Decimal(row.acct_avail_bal),
            new Decimal(row.acct_int_rate),
            row.acct_last_txn_dt,
            row.acct_currency,
            row.acct_branch_code,
            row.acct_flags,
            row.acct_filler
        );
    }

    async writeAcctFile(acctFile: IntcalcDomainModels.AcctFile): Promise<void> {
        const query = `
            INSERT INTO public.acctfile (
                acct_number, acct_customer_id, acct_opened_date, acct_type, acct_status, 
                acct_balance, acct_avail_bal, acct_int_rate, acct_last_txn_dt, acct_currency, 
                acct_branch_code, acct_flags, acct_filler
            ) VALUES (
                $1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13
            )
            ON CONFLICT (acct_number) DO UPDATE SET
                acct_customer_id = EXCLUDED.acct_customer_id,
                acct_opened_date = EXCLUDED.acct_opened_date,
                acct_type = EXCLUDED.acct_type,
                acct_status = EXCLUDED.acct_status,
                acct_balance = EXCLUDED.acct_balance,
                acct_avail_bal = EXCLUDED.acct_avail_bal,
                acct_int_rate = EXCLUDED.acct_int_rate,
                acct_last_txn_dt = EXCLUDED.acct_last_txn_dt,
                acct_currency = EXCLUDED.acct_currency,
                acct_branch_code = EXCLUDED.acct_branch_code,
                acct_flags = EXCLUDED.acct_flags,
                acct_filler = EXCLUDED.acct_filler
        `;
        await this.dbPool.query(query, [
            acctFile.acctNumber,
            acctFile.acctCustomerId,
            acctFile.acctOpenedDate,
            acctFile.acctType,
            acctFile.acctStatus,
            acctFile.acctBalance.toString(),
            acctFile.acctAvailBal.toString(),
            acctFile.acctIntRate.toString(),
            acctFile.acctLastTxnDt,
            acctFile.acctCurrency,
            acctFile.acctBranchCode,
            acctFile.acctFlags,
            acctFile.acctFiller
        ]);
    }
}

class IntJournalRepository implements IntcalcInterfaces.IntJournalRepository {
    private dbPool: Pool;

    constructor(dbPool: Pool) {
        this.dbPool = dbPool;
    }

    async writeIntJournal(
        txnId: IntcalcDomainModels.WsNextTxnId,
        dailyInterest: IntcalcDomainModels.WsDailyInterest,
        newBalance: IntcalcDomainModels.WsNewBalance
    ): Promise<void> {
        const query = `
            INSERT INTO public.intjrnl (
                txn_id, txn_amount, txn_balance_after
            ) VALUES (
                $1, $2, $3
            )
        `;
        await this.dbPool.query(query, [
            txnId.getValue().toString(),
            dailyInterest.getValue().toString(),
            newBalance.getValue().toString()
        ]);
    }
}