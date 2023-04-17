namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public string CallApi(string apiUrl)
    {
        var _httpClient = new HttpClient();
        var response = _httpClient.GetAsync(apiUrl).Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;

        if (response.IsSuccessStatusCode)
        {
            return responseBody;
        }

        throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
    }
}