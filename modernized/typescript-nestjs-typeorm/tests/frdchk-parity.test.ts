import { FrdchkService } from 'src/FRDCHK/Application/FrdchkService';
import { FrdchkRepository, FrdchkService as CicsService, FrdchkParagraphs } from 'src/FRDCHK/Application/Interfaces/FrdchkInterfaces';

describe('FrdchkService Parity Tests', () => {
  let repositoryMock: jest.Mocked<FrdchkRepository>;
  let cicsServiceMock: jest.Mocked<CicsService>;
  let paragraphsMock: jest.Mocked<FrdchkParagraphs>;
  let frdchkService: FrdchkService;

  beforeEach(() => {
    repositoryMock = {
      readInputFile: jest.fn(),
      writeOutputFile: jest.fn(),
    };

    cicsServiceMock = {
      performCicsOperation: jest.fn(),
    };

    paragraphsMock = {
      initialize: jest.fn(),
      validateInput: jest.fn(),
      processMainLogic: jest.fn(),
      finalize: jest.fn(),
    };

    frdchkService = new FrdchkService(repositoryMock, cicsServiceMock, paragraphsMock);
  });

  it('should execute the FRDCHK program in the correct sequence', async () => {
    await frdchkService.execute();

    expect(paragraphsMock.initialize).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.validateInput).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.processMainLogic).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.finalize).toHaveBeenCalledTimes(1);

    expect(paragraphsMock.initialize).toHaveBeenCalledBefore(paragraphsMock.validateInput);
    expect(paragraphsMock.validateInput).toHaveBeenCalledBefore(paragraphsMock.processMainLogic);
    expect(paragraphsMock.processMainLogic).toHaveBeenCalledBefore(paragraphsMock.finalize);
  });

  it('should handle errors during initialization gracefully', async () => {
    const error = new Error('Initialization failed');
    paragraphsMock.initialize.mockRejectedValueOnce(error);

    await expect(frdchkService.execute()).rejects.toThrow(error);

    expect(paragraphsMock.initialize).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.validateInput).not.toHaveBeenCalled();
    expect(paragraphsMock.processMainLogic).not.toHaveBeenCalled();
    expect(paragraphsMock.finalize).not.toHaveBeenCalled();
  });

  it('should handle errors during input validation gracefully', async () => {
    const error = new Error('Input validation failed');
    paragraphsMock.validateInput.mockRejectedValueOnce(error);

    await expect(frdchkService.execute()).rejects.toThrow(error);

    expect(paragraphsMock.initialize).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.validateInput).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.processMainLogic).not.toHaveBeenCalled();
    expect(paragraphsMock.finalize).not.toHaveBeenCalled();
  });

  it('should handle errors during main logic processing gracefully', async () => {
    const error = new Error('Main logic processing failed');
    paragraphsMock.processMainLogic.mockRejectedValueOnce(error);

    await expect(frdchkService.execute()).rejects.toThrow(error);

    expect(paragraphsMock.initialize).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.validateInput).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.processMainLogic).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.finalize).not.toHaveBeenCalled();
  });

  it('should handle errors during finalization gracefully', async () => {
    const error = new Error('Finalization failed');
    paragraphsMock.finalize.mockRejectedValueOnce(error);

    await expect(frdchkService.execute()).rejects.toThrow(error);

    expect(paragraphsMock.initialize).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.validateInput).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.processMainLogic).toHaveBeenCalledTimes(1);
    expect(paragraphsMock.finalize).toHaveBeenCalledTimes(1);
  });
});