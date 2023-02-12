using System.Net.Http.Headers;

namespace FZH;

class ZhClient
{
    private readonly Configuration.ZhGraphQlKey zhGraphQlKey;    

    private HttpClient? httpClient;

    public ZhClient(Configuration.ZhGraphQlKey zhGraphQlKey)
    {
        this.zhGraphQlKey = zhGraphQlKey;
    }

    public void Start()
    {
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", zhGraphQlKey.Value);
    }

    public async Task<string> MakeRequest(string query)
    {
        if (httpClient is null) { return ""; }

        var msg = new HttpRequestMessage(requestUri: "https://api.zenhub.com/public/graphql", method: HttpMethod.Post);
        msg.Content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                { "query", query }
            });
        var response = await httpClient.SendAsync(msg);
        return await response.Content.ReadAsStringAsync();
    }
}