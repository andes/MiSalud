using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text;

namespace AndesServices.Services
{
    public class LoginService : ILoginService<User>
    {
        private readonly ILogger<LoginService> _logger; // Correctly define the logger field

        private IConfiguration? _configuration { get; }

        public LoginService(IConfiguration? configuration, ILogger<LoginService> logger) // Inject logger via constructor
        {
            _configuration = configuration;
            _logger = logger; // Assign the injected logger
        }

        public async Task<User?> Login(string email, string password, Ref<string> mensaje)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            User user = new User
            {
                email = email,
                password = password
            };

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/mobileApp/login"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/mobileApp/login";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");
                    var userJson = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
                    using (HttpResponseMessage res = await client.PostAsync(url, userJson))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            LoginResp? loginResp = await res.Content.ReadFromJsonAsync<LoginResp>();
                            if (loginResp == null || loginResp.user == null)
                            {
                                _logger.LogError("Error: loginResp es null o user es null");
                                return null;
                            }

                            loginResp.user.token = loginResp.token;

                            _logger.LogInformation("OK: Todo ok");
                            return loginResp.user;
                        }
                        else
                        {
                            mensaje.Value = await res.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Se produjo un error"); 
            }
            user = new User();
            return user;
        }

        public Task<bool> Logout(string token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Register(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
