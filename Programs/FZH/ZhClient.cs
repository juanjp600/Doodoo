using System.Net.Http.Headers;
using DoodooCoreCsharp;

namespace FZH;

class ZhClient
{
    private readonly Configuration.ZhGraphQlKey zhGraphQlKey;    

    private Option<HttpClient> httpClient;

    public ZhClient(Configuration.ZhGraphQlKey zhGraphQlKey)
    {
        this.zhGraphQlKey = zhGraphQlKey;
    }

    public void Start()
    {
        var newHttpClient = new HttpClient();
        newHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: zhGraphQlKey.Value);
        httpClient = Option.Some(newHttpClient);
    }

    public enum RequestFailureReason
    {
        NotStarted,
        HttpRequestFailed
    }
    public async Task<Result<string, RequestFailureReason>> MakeRequest(string query)
    {
        if (!httpClient.TryUnwrap(out var currentHttpClient))
        {
            return Result.Failure(RequestFailureReason.NotStarted);
        }

        var msg = new HttpRequestMessage(requestUri: "https://api.zenhub.com/public/graphql", method: HttpMethod.Post);
        msg.Content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                { "query", query }
            });
        var response = await currentHttpClient.SendAsync(msg);
        if (!response.IsSuccessStatusCode)
        {
            return Result.Failure(RequestFailureReason.HttpRequestFailed);
        }
        return Result.Success(await response.Content.ReadAsStringAsync());
    }
}