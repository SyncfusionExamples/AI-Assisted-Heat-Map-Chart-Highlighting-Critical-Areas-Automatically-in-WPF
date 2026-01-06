using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;

namespace RabiesTracker
{
    public class RabiesTrackerService
    {
        #region Properties

        internal const string Endpoint = "https://mobilemaui.openai.azure.com/";
        internal const string DeploymentName = "gpt-4o";
        internal const string Key = "6673b6975f334c79bd0db8a1cd70aa49";

        internal IChatClient? Client { get; set; }
        internal bool IsValid { get; set; }

        #endregion

        #region Constructor

        public RabiesTrackerService()
        {
            _ = ValidateCredential();            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Predicts rabies data for a given country by dynamically querying an AI 
        /// for its major states and then generating the corresponding data.
        /// </summary>
        /// <param name="countryName">The country for which to generate data.</param>
        /// <returns>A list of RabisTrackerInfo objects for the last 8 years.</returns>
        internal async Task<List<RabisTrackerInfo>> PredictRabisData(string countryName)
        {
            try
            {
                // Dynamically get the 10 major states for the given country from the AI.
                List<string> states = await GetMajorStatesFromAI(countryName);

                if (states == null || states.Count == 0)
                {
                    MessageBox.Show($"Could not determine the major states for '{countryName}'.");
                    return new List<RabisTrackerInfo>();
                }


                // Step 2: Use the retrieved list of states to generate the final data in JSON format,
                // using the improved prompt structure.
                string response = await GetRabiesDataAsJsonFromAI(countryName, states);
                string extratedJsonData = JsonExtractor.ExtractJson(response);

                //// Step 3: Deserialize the JSON string directly into a list of objects.
                return !string.IsNullOrEmpty(extratedJsonData)
                ? JsonSerializer.Deserialize<List<RabisTrackerInfo>>(extratedJsonData) ?? new List<RabisTrackerInfo>()
                : new List<RabisTrackerInfo>();
            }
            catch (Exception)
            {
               MessageBox.Show("Invalid Credential, The data has been retrieved from the previously loaded JSON file.");
               return GetCurrentDataFromEmbeddedJson();
            }
        }

        private List<RabisTrackerInfo> GetCurrentDataFromEmbeddedJson()
        {
            var executingAssembly = typeof(App).GetTypeInfo().Assembly;

            using (var stream = executingAssembly.GetManifestResourceStream("RabiesTracker.Resources.RabiesData.json"))
            {
                if (stream == null)
                {
                    // Log or handle the missing resource scenario
                    return new List<RabisTrackerInfo>();
                }

                using (var textStream = new StreamReader(stream))
                {
                    string json = textStream.ReadToEnd();
                    return JsonSerializer.Deserialize<List<RabisTrackerInfo>>(json) ?? new List<RabisTrackerInfo>();
                }
            }
        }

        /// <summary>
        /// Prompts the AI to get a list of the 10 major states for a given country.
        /// </summary>
        private async Task<List<string>> GetMajorStatesFromAI(string country)
        {
            var systemMessage = "You are an AI assistant that provides World rabis case information. Respond only with the requested data and nothing else.";
            var userMessage = $"List the 10 most populous states/provinces of {country} as a single comma-separated string. Example: State1,State2,State3. Do not add any other text.";

            string response = await GetAnswerFromGPT(userMessage + " " + systemMessage);

            if (string.IsNullOrWhiteSpace(response)) return new List<string>();

            response = response.Trim().Replace("\"", "");
            return response.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
        }

        /// <summary>
        /// Prompts the AI to generate rabies case data using a structured, detailed prompt.
        /// </summary>
        private async Task<string> GetRabiesDataAsJsonFromAI(string country, List<string> states)
        {
            int currentYear = DateTime.UtcNow.Year - 1;
            int startYear = currentYear - 7;
            string statesString = string.Join(", ", states.Select(s => $"\"{s}\""));

            // Improved prompt structure based on your request
            var systemMessage = "You are an AI model specialized in Rabies case tracking. " +
                                $"Based on the request, generate a realistic but synthetic dataset for rabis cases for the 10 major states of the provided country over the past 8 years ({startYear} to {currentYear}).";

            var userMessage = $"Generate the rabies case data for the past 8 years for the states: {statesString}.\n\n" +
                              "Ensure the output follows this structured format:\n\n" +
                              "[\n" +
                              "  {\n" +
                              "    \"State\": \"State Name\",\n" +
                              "     \"Y{startYear}\": \"Rabis case count for Y{startYear} (any positive number)\",\n" +
                              "     \"Y{startYear + 1}\": \"Rabis case count for Y{startYear + 1} (any positive number)\",\n" +
                              "     \"Y{startYear + 2}\": \"Rabis case count for Y{startYear + 2} (any positive number)\",\n" +
                              "     \"Y{startYear + 3}\": \"Rabis case count for Y{startYear + 3} (any positive number)\",\n" +
                              "     \"Y{startYear + 4}\": \"Rabis case count for Y{startYear + 4} (any positive number)\",\n" +
                              "     \"Y{startYear + 5}\": \"Rabis case count for Y{startYear + 5} (any positive number)\",\n" +
                              "     \"Y{startYear + 6}\": \"Rabis case count for Y{startYear + 6} (any positive number)\",\n" +
                              "     \"Y{startYear + 7}\": \"Rabis case count for Y{startYear + 7} (any positive number)\",\n" +
                              "     \"Y{startYear + 8}\": \"Rabis case count for Y{startYear + 8} (any positive number)\",\n" +
                              "  }\n" +
                              "]\n\n" +
                              "Ensure that the generated data reflects plausible trends. The output MUST be only the JSON array.";

            string response = await GetAnswerFromGPT(userMessage + " " + systemMessage);            
            return response;
        }


        private async Task<string> GetAnswerFromGPT(string userPrompt)
        {
            try
            {
                if (Client != null)
                {
                    var response = await Client.CompleteAsync(userPrompt);
                    return response.ToString();
                }
            }
            catch
            {
                return "";
            }

            return "";
        }

        internal async Task ValidateCredential()
        {
            GetAzureOpenAIClient();

            try
            {
                if (Client != null)
                {
                    IsValid = true;
                    await Client!.CompleteAsync("Hello, AI Validation");
                }
                else
                {
                    IsValid = false;
                }
            }
            catch (Exception)
            {
                IsValid = false;
            }
        }

        private void GetAzureOpenAIClient()
        {
            try
            {
                var client = new AzureOpenAIClient(new Uri(Endpoint), new AzureKeyCredential(Key)).AsChatClient(modelId: DeploymentName);
                this.Client = client;
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }

    public class JsonExtractor
    {
        public static string ExtractJson(string response)
        {
            try
            {
                Match match = Regex.Match(response, @"\[.*?\]", RegexOptions.Singleline);

                if (match.Success && !string.IsNullOrWhiteSpace(match.Value))
                {
                    string json = match.Value.Trim();
                    return json;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error extracting JSON: {ex.Message}");
            }

            return "Invalid or No JSON Found";
        }
    }
}
