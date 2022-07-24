using Entities;

namespace NetCoreUrunSitesi.Services
{
    public class AppUsersApiService
    {
        private readonly HttpClient _httpClient;

        public AppUsersApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AppUser>> GetAllAppUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<AppUser>>("AppUsers");
        }
        public async Task<AppUser> AddAsync(AppUser appUser)
        {
            var response = await _httpClient.PostAsJsonAsync("AppUsers", appUser);

            if (!response.IsSuccessStatusCode) return null;

            var responseBody = await response.Content.ReadFromJsonAsync<AppUser>();

            return responseBody;
        }
        public async Task<AppUser> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AppUser>($"AppUsers/{id}");
        }
        public async Task<bool> UpdateAsync(AppUser appUser)
        {
            var response = await _httpClient.PutAsJsonAsync("AppUsers", appUser);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"AppUsers/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
