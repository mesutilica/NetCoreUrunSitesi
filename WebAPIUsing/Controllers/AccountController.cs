using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.User.FindFirst("UserGuid");
            if (id is null)
            {
                return RedirectToAction("Login");
            }
            var model = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "Auth/GetUserByUserGuid/" + id.Value);
            if (model is null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(AppUser appUser)
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //_httpClient.DefaultRequestHeaders.Authorization.Parameter.Insert(0,"test");
                    appUser.CreateDate = DateTime.Now;
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "AppUsers", appUser);
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> LoginAsync(LoginViewModel appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "Auth/Login", appUser);
                    //string stringJWT = await response.Content.ReadAsStringAsync(); // 
                    Token jwt = await response.Content.ReadFromJsonAsync<Token>(); // JsonConvert.DeserializeObject<Token>(stringJWT);
                    if (!response.IsSuccessStatusCode) //  == null
                    {
                        ModelState.AddModelError("", "Giriş Başarısız!");
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", jwt.AccessToken);
                        var claims = new List<Claim>() // Claim = hak
                        {
                            new(ClaimTypes.Name, "Admin"),
                            new(ClaimTypes.Email, appUser.Email),
                            new(ClaimTypes.Role, jwt.IsAdmin ? "Admin" : "User"),
                            //new("UserId", account.Id.ToString()),
                            new("UserGuid", jwt.UserGuid),
                            new("RefreshToken", jwt.RefreshToken),
                            new("Expiration", jwt.Expiration.ToString())
                        };
                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTime.UtcNow.AddDays(7),
                            IsPersistent = true
                        };
                        ClaimsPrincipal principal = new(userIdentity);
                        await HttpContext.SignInAsync(principal, authProperties);
                        return Redirect(string.IsNullOrEmpty(HttpContext.Request.Query["ReturnUrl"]) ? "/Account" : HttpContext.Request.Query["ReturnUrl"]);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(appUser);
        }
        public async Task<IActionResult> LogoutAsync()
        {
            HttpContext.Session.Remove("token");
            await HttpContext.SignOutAsync(); // Çıkış işlemi
            return RedirectToAction("Index", "Account");
        }
    }
}

/*var anotherKey = "test";
            HttpClient httpClient = new HttpClient
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