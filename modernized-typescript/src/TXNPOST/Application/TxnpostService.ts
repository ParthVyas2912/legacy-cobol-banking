import { ITxnpostRepository, ITxnpostService } from 'src/TXNPOST/Application/Interfaces/TxnpostInterfaces';

export class TxnpostService {
    private repository: ITxnpostRepository;
    private service: ITxnpostService;

    constructor(repository: ITxnpostRepository, service: ITxnpostService) {
        this.repository = repository;
        this.service = service;
    }

    async execute(): Promise<void> {
        await this.main();
    }

    private async main(): Promise<void> {
        await this.openFiles();

        let endOfFile = false;
        while (!endOfFile) {
            endOfFile = await this.processTransactions();
        }

        await this.closeFiles();
    }

    private async openFiles(): Promise<void> {
        await this.repository.openFiles();
    }

    private async processTransactions(): Promise<boolean> {
        const transaction = await this.repository.readTransaction();
        if (transaction === null) {
            return true; // End of file reached
        }

        await this.applyTransaction(transaction);
        return false; // Continue processing
    }

    private async applyTransaction(transaction: any): Promise<void> {
        const success = await this.service.applyTransaction(transaction);

        const auditRecord = {
            transaction,
            success,
        };

        await this.repository.writeAuditRecord(auditRecord);
    }

    private async closeFiles(): Promise<void> {
        await this.repository.closeFiles();
    }
}