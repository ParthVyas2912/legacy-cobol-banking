using System;
using System.Threading.Tasks;
using INTCALC.Application.Interfaces;

namespace INTCALC.Application
{
    public class IntcalcService : IIntcalcApplication
    {
        private readonly IAccountFileRepository _accountFileRepository;

        public IntcalcService(IAccountFileRepository accountFileRepository)
        {
            _accountFileRepository = accountFileRepository;
        }

        public async Task ExecuteMainAsync()
        {
            // Corresponds to 0000-MAIN paragraph
            await _accountFileRepository.OpenAccountFileAsync();
            await ProcessLoopAsync();
            await _accountFileRepository.CloseAccountFileAsync();
        }

        public async Task ProcessLoopAsync()
        {
            // Corresponds to 1000-LOOP paragraph
            while (await LoopConditionAsync())
            {
                await AccrueInterestAsync();
            }
        }

        public async Task AccrueInterestAsync()
        {
            // Corresponds to 2000-ACCRUE paragraph
            if (await AccrueConditionAsync())
            {
                // Perform accrual logic here
                await _accountFileRepository.WriteInterestJournalAsync();
            }
        }

        private async Task<bool> LoopConditionAsync()
        {
            // Corresponds to the loop condition in 1000-LOOP
            await _accountFileRepository.ReadAccountFileAsync();
            // Replace with actual condition logic
            return false; // Default to false for now
        }

        private async Task<bool> AccrueConditionAsync()
        {
            // Corresponds to the condition in 2000-ACCRUE
            // Replace with actual condition logic
            return true; // Default to true for now
        }
    }
}