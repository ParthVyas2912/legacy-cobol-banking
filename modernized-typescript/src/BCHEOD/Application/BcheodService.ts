import { BcheodInterfaces } from 'src/BCHEOD/Application/Interfaces/BcheodInterfaces';

export class BcheodService implements BcheodInterfaces.IBcheodApplication {
    async execute(): Promise<void> {
        await this.drive();
    }

    async drive(): Promise<void> {
        await this.markStart();
        let currentStep: '1000-NEXT-STEP' | '2000-INT-STEP' = '1000-NEXT-STEP';

        while (true) {
            switch (currentStep) {
                case '1000-NEXT-STEP':
                    currentStep = await this.nextStep();
                    break;
                case '2000-INT-STEP':
                    currentStep = await this.intStep();
                    break;
                default:
                    return;
            }
        }
    }

    async nextStep(): Promise<'2000-INT-STEP'> {
        // Logic for 1000-NEXT-STEP
        return '2000-INT-STEP';
    }

    async intStep(): Promise<'1000-NEXT-STEP'> {
        // Logic for 2000-INT-STEP
        return '1000-NEXT-STEP';
    }

    async postStep(): Promise<void> {
        // Placeholder for the actual implementation of the post-processing logic
    }

    async done(): Promise<void> {
        await this.markEnd();
    }

    async markStart(): Promise<void> {
        // Placeholder for the actual implementation of marking the start
    }

    async markEnd(): Promise<void> {
        // Placeholder for the actual implementation of marking the end
    }
}