using System;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace crawler.UriOps;
public class UniformResourceIdentifier
{
    static HttpClient _httpClient = new HttpClient() {
        Timeout = TimeSpan.FromSeconds(10)
    };

    // Constructors
    public UniformResourceIdentifier() { }
    static string RemoveLastIndex(string _host)
    {
        int len = _host.Length;

        char[] chars = _host.ToCharArray();

        string modifieadChars = "";
        for (int i = 0; i < chars.Length - 1; i++)
        {
            modifieadChars += chars[i];
        }

        return modifieadChars.ToString();
    }

    static List<string> MakeAbsoluteUrls(Uri baseUrl, List<string> href)
    {
        for (int i = 0; i < href.Count; i++)
        {
            // Makes an absolute path out of relative paths
            if (href[i].StartsWith('/'))
            {
                // Removes trailing / and converts to absolute
                // path.
                if (baseUrl.ToString().EndsWith("/"))
                    href[i] = UniformResourceIdentifier.RemoveLastIndex(baseUrl.ToString()) + href[i];
                else
                    href[i] = baseUrl + href[i];
            }

            // Processes the paths that are neither relative
            // nor absolute.
            if (!href[i].StartsWith('/') && !href[i].StartsWith("http"))
            {
                if (baseUrl.ToString().EndsWith('/'))
                    href[i] = $"{RemoveLastIndex(baseUrl.ToString())}" + href[i];
                else
                    href[i] = $"{baseUrl}/" + href[i];
            }
        }

        return href;
    }

    public static async Task<string> GetHtmlBodyAsync(string url)
    {
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);

            return responseMessage.Content.ReadAsStringAsync().Result;
        }
        catch (Exception e)
        {
            ConsoleDataDisplay.PrintError($"{e.Message}");

            return e.Message;
        }
    }

    public static HashSet<string> GetLinks(Uri url ,string htmlContent)
    {
        if (htmlContent != null || htmlContent != "")
        {
            string anotherPattern3 = "\"(http.*?)\"|href=\"([^#http].*?)\"|src=\"([^http].*?)\"";

            MatchCollection linkCollection = Regex.Matches(htmlContent, anotherPattern3);

            var http = linkCollection.Where(match => match.Groups[1].Value != "")
                .Select(x => x.Groups[1].Value).ToList();

            var href = linkCollection.Where(match => match.Groups[2].Value != "")
                .Select(x => x.Groups[2].Value).ToList();

            var src = linkCollection.Where(match => match.Groups[3].Value != "")
                .Select(x => x.Groups[3].Value).ToList();

            // href and src lists contain relative paths
            href.AddRange(src);

            // Merge all the lists with http list
            // this list contains absolute urls.
            http.AddRange(MakeAbsoluteUrls(url, href));

            // Remove duplicates
            HashSet<string> uniqueUrls = new HashSet<string>(http);

            return uniqueUrls;
        }
        else
        {
            return new HashSet<string>();
        }
    }

    public static async Task<bool> ValidateAsync(Uri uri)
    {
        bool isInGoodFormat = Uri.IsWellFormedUriString(uri.ToString(), UriKind.Absolute);
        bool isAvailable = false;

        if (isInGoodFormat)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Head, uri);
            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequestMessage);

                if ((int)httpResponse.StatusCode >= 200 && (int)httpResponse.StatusCode <= 300)
                {
                    ConsoleDataDisplay.PrintSuccess($"{(int)httpResponse.StatusCode} {httpResponse.ReasonPhrase}");

                    try
                    {
                        MediaTypeHeaderValue? contentType = httpResponse.Content.Headers.ContentType;

                        if (contentType?.MediaType != null)
                        {
                            if (contentType.MediaType == "text/html")
                            {
                                isAvailable = true;
                            }
                            else
                            {
                                ConsoleDataDisplay.PrintWarning("Not Of text/html type !!");
                            }
                        }
                        else
                        {
                            ConsoleDataDisplay.PrintError("MediaType isnt specified");
                        }

                    }
                    catch (Exception)
                    {
                        ConsoleDataDisplay.PrintError("Content type may be null");
                    }
                }
                else
                {
                    ConsoleDataDisplay.PrintError($"{(int)httpResponse.StatusCode} {httpResponse.ReasonPhrase}");
                }
            }
            catch (Exception e)
            {
                ConsoleDataDisplay.PrintError($"{e.Message}");
            }

        }
        else
        {
            ConsoleDataDisplay.PrintError("Bad Url Format");

            return false;
        }

        return isAvailable;
    }

    public static bool TryParse(string uri)
    {
        // Uri class may fail to parse the content of seedUrl
        
        Uri url;

        try
        {
            url = new Uri(uri);

            return true;
        }
        catch (Exception e)
        {
            ConsoleDataDisplay.PrintError(e.Message);
        }

        return false;
    }

    public static bool IsOfTheSameHost(Uri host, string uri)
    {
        if (uri.Contains(host.Host))
        {
            return true;
        }
        else { return false; }
    }
}

