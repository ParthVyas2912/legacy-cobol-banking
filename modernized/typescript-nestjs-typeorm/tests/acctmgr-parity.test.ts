import { AcctmgrService } from 'src/ACCTMGR/Application/AcctmgrService';

describe('AcctmgrService Parity Tests', () => {
    let service: AcctmgrService;

    beforeEach(() => {
        service = new AcctmgrService();
    });

    it('should execute without errors', async () => {
        await expect(service.execute()).resolves.not.toThrow();
    });
});