import { AcctmgrDomainModels } from 'src/ACCTMGR/Domain/AcctmgrDomainModels';
import { AcctmgrInterfaces } from 'src/ACCTMGR/Application/Interfaces/AcctmgrInterfaces';

export class AcctmgrService {
    private readonly accountRepository: AcctmgrInterfaces.IAccountRepository;
    private readonly cicsService: AcctmgrInterfaces.ICicsService;

    constructor(
        accountRepository: AcctmgrInterfaces.IAccountRepository,
        cicsService: AcctmgrInterfaces.ICicsService
    ) {
        this.accountRepository = accountRepository;
        this.cicsService = cicsService;
    }

    public async execute(): Promise<void> {
        await this.dispatch('__entry__');
    }

    private async dispatch(paragraph: string): Promise<void> {
        switch (paragraph) {
            case '__entry__':
                await this.dispatch('0000-START');
                break;
            case '0000-START':
                await this.dispatch('1000-CREATE-ACCOUNT');
                break;
            case '1000-CREATE-ACCOUNT':
                await this.createAccount();
                await this.dispatch('2000-READ-ACCOUNT');
                break;
            case '2000-READ-ACCOUNT':
                await this.readAccount();
                await this.dispatch('3000-UPDATE-ACCOUNT');
                break;
            case '3000-UPDATE-ACCOUNT':
                await this.updateAccount();
                await this.dispatch('3100-APPLY-UPDATES');
                break;
            case '3100-APPLY-UPDATES':
                await this.applyUpdates();
                await this.dispatch('4000-CLOSE-ACCOUNT');
                break;
            case '4000-CLOSE-ACCOUNT':
                await this.closeAccount();
                break;
            default:
                throw new Error(`Unknown paragraph: ${paragraph}`);
        }
    }

    private async createAccount(): Promise<void> {
        // Placeholder for 1000-CREATE-ACCOUNT logic
    }

    private async readAccount(): Promise<void> {
        // Placeholder for 2000-READ-ACCOUNT logic
    }

    private async updateAccount(): Promise<void> {
        // Placeholder for 3000-UPDATE-ACCOUNT logic
    }

    private async applyUpdates(): Promise<void> {
        // Placeholder for 3100-APPLY-UPDATES logic
    }

    private async closeAccount(): Promise<void> {
        // Placeholder for 4000-CLOSE-ACCOUNT logic
    }
}