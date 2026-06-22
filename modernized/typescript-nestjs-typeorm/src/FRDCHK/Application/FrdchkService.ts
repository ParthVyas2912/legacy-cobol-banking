import { FrdchkApplication } from 'src/FRDCHK/Application/Interfaces/FrdchkInterfaces';
import { FrdchkRepository } from 'src/FRDCHK/Application/Interfaces/FrdchkInterfaces';
import { FrdchkService as CicsService } from 'src/FRDCHK/Application/Interfaces/FrdchkInterfaces';
import { FrdchkParagraphs } from 'src/FRDCHK/Application/Interfaces/FrdchkInterfaces';

export class FrdchkService implements FrdchkApplication {
  private repository: FrdchkRepository;
  private cicsService: CicsService;
  private paragraphs: FrdchkParagraphs;

  constructor(
    repository: FrdchkRepository,
    cicsService: CicsService,
    paragraphs: FrdchkParagraphs
  ) {
    this.repository = repository;
    this.cicsService = cicsService;
    this.paragraphs = paragraphs;
  }

  async execute(): Promise<void> {
    // Step 1: Initialize the program
    await this.paragraphs.initialize();

    // Step 2: Validate the input data
    await this.paragraphs.validateInput();

    // Step 3: Process the main logic
    await this.paragraphs.processMainLogic();

    // Step 4: Finalize the program
    await this.paragraphs.finalize();
  }
}