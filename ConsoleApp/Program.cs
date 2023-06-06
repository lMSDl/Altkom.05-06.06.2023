


using Microsoft.AspNetCore.SignalR.Client;
using Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;


var httpClient = new HttpClient();
var openApiClient = new WebAPI.WebApiService("http://localhost:5209", httpClient);

var parent = await openApiClient.ParentsAsync();

var signalR = new HubConnectionBuilder()
    .WithUrl("http://localhost:5209/SignalR/Demo")
        .WithAutomaticReconnect()
    .Build();

signalR.Reconnecting += SignalR_Reconnecting;
signalR.Reconnected += SignalR_Reconnected;

Task SignalR_Reconnected(string? arg)
{
    Console.WriteLine("Connected");
    return Task.CompletedTask;
}

Task SignalR_Reconnecting(Exception? arg)
{
    if (arg != null)
        Console.WriteLine(arg.Message);
    Console.WriteLine("Reconnecting...");
    return Task.CompletedTask;
}


signalR.On<string>(nameof(Welcome), Welcome);
signalR.On<string>("TextMessage", x => Console.WriteLine(x));


async void Welcome(string message)
{
    Console.WriteLine(message);
    await signalR.SendAsync("SayHelloToOthers", $"Hello my name is {signalR.ConnectionId}");
}

await signalR.StartAsync();

await signalR.SendAsync("JoinToGroup", (DateTime.Now.Second % 3).ToString());

Console.ReadLine();

/*HttpClientHandler clientHandler = new()
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
}*/