using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenAI.Net;

namespace WebApplication.Pages ;

public class IndexModel : PageModel
{
    readonly ILogger<IndexModel> _logger;
    readonly IOpenAIService      _openAIService;

    public List<string> Results { get; set; } = new();
    public string ErrorMessage { get; set; } = "";
    public IndexModel(ILogger<IndexModel> logger, IOpenAIService openAIService)
    {
        _logger        = logger;
        _openAIService = openAIService;
    }

    public void OnGet()
    {

    }

    [BindProperty]
    public string SearchText { get; set; } = "What does USA stand for?";
    [BindProperty]
    public int MaxResults { get; set; } = 1;
    [BindProperty]
    public int MaxTokens { get; set; } = 500;
    public async Task OnPost()
    {
        var response = await _openAIService.TextCompletion.Get(SearchText, o => {
                                                                               o.N         = MaxResults;
                                                                               o.MaxTokens = MaxTokens;
                                                                           });
        if(response.IsSuccess)
        {
            Results = response.Result!.Choices.Select(i=> i.Text).ToList();
        }
        else
        {
            ErrorMessage = response.ErrorMessage;
        }
    }
}