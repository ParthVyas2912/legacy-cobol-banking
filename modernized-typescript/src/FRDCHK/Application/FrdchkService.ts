import { FrdchkDomainModels } from 'src/FRDCHK/Domain/FrdchkDomainModels';
import { FrdchkInterfaces } from 'src/FRDCHK/Application/Interfaces/FrdchkInterfaces';

export class FrdchkService implements FrdchkInterfaces.FrdchkApplication {
    private workingStorage: FrdchkDomainModels.WorkingStorage;

    constructor(
        private readonly transactionRepository: FrdchkInterfaces.TransactionRepository,
        private readonly cicsService: FrdchkInterfaces.CicsService
    ) {
        this.workingStorage = new FrdchkDomainModels.WorkingStorage(
            new Decimal(0),
            new Decimal(0),
            new Decimal(0)
        );
    }

    public async execute(): Promise<void> {
        await this.start();
    }

    private async start(): Promise<void> {
        let currentStep: '1000-COMPUTE-VELOCITY' | '2000-SCORE-RISK' | '3000-FREEZE-ACCOUNT' | null = '1000-COMPUTE-VELOCITY';

        while (currentStep) {
            switch (currentStep) {
                case '1000-COMPUTE-VELOCITY':
                    await this.computeVelocity();
                    currentStep = '2000-SCORE-RISK';
                    break;
                case '2000-SCORE-RISK':
                    await this.scoreRisk();
                    currentStep = '3000-FREEZE-ACCOUNT';
                    break;
                case '3000-FREEZE-ACCOUNT':
                    await this.freezeAccount();
                    currentStep = null;
                    break;
            }
        }
    }

    private async computeVelocity(): Promise<void> {
        // Placeholder for 1000-COMPUTE-VELOCITY logic
    }

    private async scoreRisk(): Promise<void> {
        // Placeholder for 2000-SCORE-RISK logic
    }

    private async freezeAccount(): Promise<void> {
        // Placeholder for 3000-FREEZE-ACCOUNT logic
    }
}