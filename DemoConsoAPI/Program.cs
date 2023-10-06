using DemoConsoAPI;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

string url = "https://localhost:7004/api/";

HttpClient client = new HttpClient();
client.BaseAddress = new Uri(url);

List<Game> gamelist = new List<Game>();

using (HttpResponseMessage response = client.GetAsync("game").Result)
{
    if (response.IsSuccessStatusCode)
    {
        string json = response.Content.ReadAsStringAsync().Result;
        gamelist = JsonConvert.DeserializeObject<List<Game>>(json);
    }
    else
    {
        Console.WriteLine(response.StatusCode);
    }
}

foreach(Game g in gamelist)
{
    Console.WriteLine($"{g.Id} - {g.Title}");
}


string email = "admin@mail.com";
string password = "Test1234!";
string token = "";
string jsonToSend = JsonConvert.SerializeObject(new { email, password });
HttpContent content = new StringContent(jsonToSend, Encoding.UTF8, "application/json");

using(HttpResponseMessage response = client.PostAsync("user/login", content).Result)
{
    if(response.IsSuccessStatusCode)
    {
         token = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine(token);
    }
}

List<Game> favoris = new List<Game>();

client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
using (HttpResponseMessage response = client.GetAsync("game/favoris/1").Result)
{
    if (response.IsSuccessStatusCode)
    {
        string json = response.Content.ReadAsStringAsync().Result;
        favoris = JsonConvert.DeserializeObject<List<Game>>(json);
    }
    else
    {
        Console.WriteLine(response.StatusCode);
    }
}

foreach (Game g in favoris)
{
    Console.WriteLine($"{g.Id} - {g.Title}");
}