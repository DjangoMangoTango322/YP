using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace RestAPP.Services
{
    public class GigaChatService
    {

        private const string GIGACHAT_AUTH_KEY = "MDE5YjU1NWItZTFhMC03OTgzLTkwZGEtNjg0OWJiNzQzYmE4Ojc5ODk1ZDA4LWI4NzctNGM1Yy04NGZiLTQwZTc4NDhlZDY4NQ==";
        private const string AUTH_URL = "https://ngw.devices.sberbank.ru:9443/api/v2/oauth";
        private const string CHAT_URL = "https://gigachat.devices.sberbank.ru/api/v1/chat/completions";

        public async Task<string> GetDishDescriptionAsync(string dishName)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient(handler))
            {
                var requestId = Guid.NewGuid().ToString();
                client.DefaultRequestHeaders.Add("RqUID", requestId);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GIGACHAT_AUTH_KEY);

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("scope", "GIGACHAT_API_PERS")
                });

                var authResponse = await client.PostAsync(AUTH_URL, content);
                var authString = await authResponse.Content.ReadAsStringAsync();
                dynamic authData = JsonConvert.DeserializeObject(authString);
                string accessToken = authData.access_token;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var prompt = $"Расскажи кратко историю происхождения блюда '{dishName}' и перечисли его классический состав. Уложись в 3-4 предложения.";

                var chatBody = new
                {
                    model = "GigaChat",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.7
                };

                var jsonBody = JsonConvert.SerializeObject(chatBody);
                var chatContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var chatResponse = await client.PostAsync(CHAT_URL, chatContent);
                var chatString = await chatResponse.Content.ReadAsStringAsync();

                dynamic chatData = JsonConvert.DeserializeObject(chatString);
                return chatData.choices[0].message.content;
            }
        }
    }
}