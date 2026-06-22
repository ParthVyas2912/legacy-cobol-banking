import { IntcalcService } from 'src/INTCALC/Application/IntcalcService';

describe('IntcalcService Parity Tests', () => {
  let service: IntcalcService;

  beforeEach(() => {
    service = new IntcalcService();
  });

  it('should execute without errors', async () => {
    await expect(service.execute()).resolves.not.toThrow();
  });
});