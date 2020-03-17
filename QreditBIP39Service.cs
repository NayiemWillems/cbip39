using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using dotnetstandard_bip39;

namespace Qredit
{
    public static class QreditBIP39Service
    {
        [FunctionName("GenerateRandomMnemonic")]
        public static IActionResult GenerateRandomMnemonic([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {

            BIP39 bip39 = new BIP39();

            var dataMen = bip39.GenerateMnemonic(256, BIP39Wordlist.English).Trim().Split("\r");

            return new JsonResult(dataMen);
        }

        [FunctionName("GetKeyFromMnemonic")]
        public static async Task<IActionResult> GetKeyFromMnemonic(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";


            BIP39 bip39 = new BIP39();

            var dataMen = bip39.GenerateMnemonic(256, BIP39Wordlist.English);

            var key = bip39.MnemonicToSeedHex(dataMen, string.Empty);

            return new OkObjectResult(key);
        }
    }
}
