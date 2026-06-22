import { StmtrptService } from "../src/STMTRPT/Application/StmtrptService";

describe("StmtrptService Parity Tests", () => {
    let stmtrptService: StmtrptService;

    beforeEach(() => {
        stmtrptService = new StmtrptService();
    });

    it("should execute without errors", async () => {
        await expect(stmtrptService.execute()).resolves.not.toThrow();
    });
});