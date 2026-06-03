using System;
using System.Threading.Tasks;
using BCHEOD.Application.Interfaces;

namespace BCHEOD.Application
{
    public class BcheodService : IBcheodApplication
    {
        private readonly IBcheodExternalService _externalService;

        public BcheodService(IBcheodExternalService externalService)
        {
            _externalService = externalService ?? throw new ArgumentNullException(nameof(externalService));
        }

        public async Task ExecuteAsync()
        {
            await DriveAsync();
        }

        public async Task DriveAsync()
        {
            await MarkStartAsync();
            await NextStepAsync();
        }

        public async Task NextStepAsync()
        {
            while (true)
            {
                await IntStepAsync();
            }
        }

        public async Task IntStepAsync()
        {
            _externalService.CallIntCalcService(new { });
        }

        public async Task PostStepAsync()
        {
            _externalService.CallTxnPostService(new { });
        }

        public async Task DoneAsync()
        {
            await MarkEndAsync();
        }

        public async Task MarkStartAsync()
        {
            // Placeholder for the actual implementation of the exec-block in 9000-MARK-START
        }

        public async Task MarkEndAsync()
        {
            // Placeholder for the actual implementation of the exec-block in 9100-MARK-END
        }

        // Explicit interface implementations for IBcheodApplication
        void IBcheodApplication.Drive() => ExecuteAsync().GetAwaiter().GetResult();
        void IBcheodApplication.NextStep() => NextStepAsync().GetAwaiter().GetResult();
        void IBcheodApplication.IntStep() => IntStepAsync().GetAwaiter().GetResult();
        void IBcheodApplication.PostStep() => PostStepAsync().GetAwaiter().GetResult();
        void IBcheodApplication.Done() => DoneAsync().GetAwaiter().GetResult();
        void IBcheodApplication.MarkStart() => MarkStartAsync().GetAwaiter().GetResult();
        void IBcheodApplication.MarkEnd() => MarkEndAsync().GetAwaiter().GetResult();
    }
}