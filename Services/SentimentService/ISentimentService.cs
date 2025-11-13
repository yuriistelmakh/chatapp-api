using Azure.AI.TextAnalytics;

namespace ChatApp.Api.Services.SentimentService
{
    public interface ISentimentService
    {
        Task<TextSentiment> GetMessageSentiment(string message);
    }
}
