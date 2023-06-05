


using Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;


HttpClientHandler clientHandler = new()
{
    AutomaticDecompression = System.Net.DecompressionMethods.Brotli | System.Net.DecompressionMethods.GZip,

    //akceptujemy certyfikaty self-signed
    ClientCertificateOptions = ClientCertificateOption.Manual,
    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policy) => { return true; }
};

HttpClient httpClient = new(clientHandler)
{
    BaseAddress = new Uri("http://localhost:5209/api/")
};
httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("br"));

HttpResponseMessage response;

for (int i = 0; i < 10; i++)
{


    response = await httpClient.GetAsync("parents");

    if (i >= 9)
    {
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("NOT OK");
            continue;
        }
    }

    if (i >= 8)
    {
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("NOT 2xx Code");
            continue;
        }
    }

    try
    {
        _ = response.EnsureSuccessStatusCode();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        continue;
    }

    string content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(content);


    var parent = await response.Content.ReadFromJsonAsync<Parent>();

    parent = JsonConvert.DeserializeObject<Parent>(content);

}

response = await httpClient.GetAsync("users");

var users = await ReadAsJson<IEnumerable<User>>(response);

var user = users.First();

user.Username = "ala ma kota";

//response = await httpClient.PutAsJsonAsync($"users/{user.Id}", user);


using (var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"))
{
    await httpClient.PutAsync($"users/{user.Id}", content);
}



static async Task<T> ReadAsJson<T>(HttpResponseMessage response)
{
    string content = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(content);
}