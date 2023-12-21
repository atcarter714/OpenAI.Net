using OpenAI.Net.Services.Interfaces;
using System.Text.Json;

namespace OpenAI.Net.Services ;

public class BaseService: IBaseService
{
    readonly HttpClient _httpClient ;

    readonly JsonSerializerOptions _jsonSerializerOptions = new( )
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy   = JsonNamingPolicy.CamelCase
    } ;

    public HttpClient HttpClient => _httpClient ;

    public JsonSerializerOptions JsonSerializerOptions => _jsonSerializerOptions ;

    public BaseService( HttpClient client ) {
        _httpClient = client ;
    }
}