using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string openAiApiKey = "";

        Console.Write("Enter the text to translate: ");
        string userText = Console.ReadLine();

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiApiKey);

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "Translate the text to English"
                    },
                    new
                    {
                        role = "user",
                        content = userText
                    }
                },
                temperature = 1,
                max_tokens = 256,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = JObject.Parse(responseBody);
                var assistantMessage = jsonResponse["choices"][0]["message"]["content"].ToString();
                Console.WriteLine(new string('=', Console.WindowWidth));
                Console.WriteLine($"Assistant's Response: {assistantMessage}");
                Console.WriteLine(new string('=', Console.WindowWidth));
            }
            else
            {
                Console.WriteLine($"Request failed. Status code: {response.StatusCode}");
                Console.WriteLine($"Response content: {responseBody}");
            }
        }
    }
}