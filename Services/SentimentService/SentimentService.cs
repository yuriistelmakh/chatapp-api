using Azure;
using Azure.AI.TextAnalytics;

namespace ChatApp.Api.Services.SentimentService
{
    public class SentimentService : ISentimentService
    {
        private readonly IConfiguration _configuration;

        public SentimentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TextSentiment> GetMessageSentiment(string message)
        {
            var client = new TextAnalyticsClient(
                new Uri(_configuration["AzureAI:Endpoint"] ?? throw new InvalidOperationException("Sentiment Analisys Endpoint was not found")),
                new AzureKeyCredential(_configuration["AzureAI:Key"] ?? throw new InvalidOperationException("Sentiment Analisys Key was not found")));

            var response = await client.AnalyzeSentimentAsync(message);
            return response.Value.Sentiment;
        }
    }
}
