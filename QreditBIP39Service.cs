using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using dotnetstandard_bip39;

namespace Qredit
{
    public static class QreditBIP39Service
    {
        [FunctionName("GenerateRandomMnemonic")]
        public static IActionResult GenerateRandomMnemonic([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            return new OkObjectResult(new BIP39().GenerateMnemonic(256, BIP39Wordlist.English).Trim().Replace("\r", ""));
        }

        [FunctionName("GetKeyFromMnemonic")]
        public static async Task<IActionResult> GetKeyFromMnemonic([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            return new OkObjectResult( new BIP39().MnemonicToSeedHex(requestBody, string.Empty));
        }
    }
}
