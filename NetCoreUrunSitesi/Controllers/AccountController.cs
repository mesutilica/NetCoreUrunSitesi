using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIUsing.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;
        private static readonly string Username = "test";
        private static readonly string Password = "test@123";
        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/";
        }

        public async Task<IActionResult> Index()
        {
            var anotherKey = "test";
            /*HttpClient httpClient = new HttpClient
             {
                 BaseAddress = new Uri("https://google.com/")
             };
             _httpClient.DefaultRequestHeaders.Add($"Authorization", $"Basic {Base64Encode($"{Username}:{Password}")}");
             _httpClient.DefaultRequestHeaders.Add($"anotherKey", $"{anotherKey}");
             HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("user/123").ConfigureAwait(false);
             //For Get Method
            var response = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
             //For Post Method
            AppUser user = new AppUser(1, "ABC");
             HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("/post", user).ConfigureAwait(false);
             UserDetail userDetail = await httpResponseMessage.Content.ReadAsAsync<UserDetail>().ConfigureAwait(false);
            */
            //setup reusable http client
            /*
            HttpClient client = new HttpClient();
            Uri baseUri = new Uri(_apiAdres);
            client.BaseAddress = baseUri;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;

            //Post body content
            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            var content = new FormUrlEncodedContent(values);

            var authenticationString = $"{clientId}:{clientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/oauth2/token");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = content;

            //make the request
            var task = client.SendAsync(requestMessage);
            var response = task.Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseBody);
            */
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //_httpClient.DefaultRequestHeaders.Authorization.Parameter.Insert(0,"test");
                    appUser.CreateDate = DateTime.Now;
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "AppUsers", appUser);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Kayıt Başarısız!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(appUser);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(AppUser appUser)
        {
            return View();
        }
    }
}
/*var request = (HttpWebRequest)WebRequest.Create("Istek Atılacak URL");
            var ornekFormBody = new
            {
                adi = "fatih",
                soyadi = "gürdal",
                email = "f.gurdal@hotmail.com.tr",
                //....
            };
            var data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(ornekFormBody));
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Headers.Add(HttpRequestHeader.Authorization, "TOKEN"); //Oturum bilgisini göndermek için
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();*/
//Gelen Sonucun JSON hali
//var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
//Sonucuda istediğin tipe çevirebilirsin JsonConvert.DeserializeObject metodu ile