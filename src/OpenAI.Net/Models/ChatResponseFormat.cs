namespace OpenAI.Net ;

public static class ChatResponseFormat
{
    public static readonly ChatResponseFormatType Text = new() { Type = "text" };
    public static readonly ChatResponseFormatType Json = new() { Type = "json_object" };
}