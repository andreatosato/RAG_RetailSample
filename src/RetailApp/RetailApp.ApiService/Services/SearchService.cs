using Microsoft.KernelMemory;
using Microsoft.SemanticKernel.ChatCompletion;

namespace RetailApp.ApiService.Services;

public class SearchService
{
    private readonly IKernelMemory kernelMemory;
    private readonly IChatCompletionService chatCompletionService;

    public SearchService(IKernelMemory kernelMemory, IChatCompletionService chatCompletionService)
    {
        this.kernelMemory = kernelMemory;
        this.chatCompletionService = chatCompletionService;
    }

    public async Task<MemoryAnswer> Search(string question)
    {
        var chat = new ChatHistory();
        question = await CreateQuestionAsync(question, chat);
        var answer = await kernelMemory.AskAsync(question, minRelevance: 0.75);
        if (answer.NoResult == false)
        {
            chat.AddUserMessage(question);
            chat.AddAssistantMessage(answer.Result);
            foreach (Citation source in answer.RelevantSources)
            {
                try
                {
                    //var d = memoryDb.GetListAsync(source.Index);
                }
                catch (Exception)
                {
                }
                Console.WriteLine($"- {source.SourceUrl ?? source.SourceName}");
            }
            return answer;
        }
        return null;
    }

    async Task<string> CreateQuestionAsync(string question, ChatHistory chat)
    {
        var embeddingQuestion = $"""
        Reformulate the following question taking into account the context of the chat to perform embeddings search:
        ---
        {question}
        ---
        You must reformulate the question in the same language of the user's question.
        Never add "in this chat", "in the context of this chat", "in the context of our conversation", "search for" or something like that in your answer.
        """;
        var reformulatedQuestion = await chatCompletionService.GetChatMessageContentAsync(chat);        
        return reformulatedQuestion.Content;
    }
}
