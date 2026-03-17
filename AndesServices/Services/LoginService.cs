using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text;
using System.Net.Http;
using AndesServices.DTOs.Login;
using AndesServices.DTOs;

namespace AndesServices.Services
{
    public class LoginService : ILoginService<User>
    {
        private readonly ILogger<LoginService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginService(ILogger<LoginService> logger, IHttpClientFactory httpClientFactory) // Inject logger via constructor
        {
            _logger = logger; // Assign the injected logger
            _httpClientFactory = httpClientFactory;
        }

        public async Task<User?> Login(string email, string password)
        {
            User user = new User
            {
                email = email,
                password = password
            };

            try
            {
                var client = _httpClientFactory.CreateClient("Andes-NoJWT");
                var userJson = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
                using (HttpResponseMessage res = await client.PostAsync("modules/mobileApp/login", userJson))
                {
                    try
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
                    }
                    catch (Exception)
                    {
                        await res.Content.ReadAsStringAsync();
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

        public async Task<User?> Login(string email, string password, Ref<string> mensaje)
        {
            User user = new User
            {
                email = email,
                password = password
            };

            try
            {
                var client = _httpClientFactory.CreateClient("Andes-NoJWT");
                var userJson = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
                using (HttpResponseMessage res = await client.PostAsync("modules/mobileApp/login", userJson))
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

        public async Task<OlvideContraseniaResponseDto?> OlvideContrasenia(OlvideContraseniaRequestDto request)
        {
            var client = _httpClientFactory.CreateClient("Andes-NoJWT");
            var response = await client.PostAsJsonAsync("modules/mobileApp/olvide-password", request);
            return await response.Content.ReadFromJsonAsync<OlvideContraseniaResponseDto>();
        }

        public async Task<ReestablecerPasswordResponseDto?> ReestablecerPassword(ReestablecerPasswordRequestDto request)
        {
            var client = _httpClientFactory.CreateClient("Andes-NoJWT");
            var response = await client.PostAsJsonAsync("modules/mobileApp/reestablecer-password", request);
            return await response.Content.ReadFromJsonAsync<ReestablecerPasswordResponseDto>();
        }

        public async Task<VerificarUsuarioRenaperResponseDto?> VerificarUsuarioRenaper(string dni, char sexo)
        {
            var client = _httpClientFactory.CreateClient("ApiXroadssAndes");
            return await client.GetFromJsonAsync<VerificarUsuarioRenaperResponseDto>($"/r1/OPTIC/GOB/GOB00001/GP-RENAPER/WS_RENAPER_DOCUMENTO/{dni}/{sexo.ToString().ToUpper()}");
        }

        public async Task<(RegistroResponseDto? response, string? errorMessage)> Registro(RegistroRequestDto dto)
        {
            var client = _httpClientFactory.CreateClient("Andes-NoJWT");
            var response = await client.PostAsJsonAsync("modules/mobileApp/registro", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                string message = await response.Content.ReadAsStringAsync();
                return (null, message);
            }

            var result = await response.Content.ReadFromJsonAsync<RegistroResponseDto>();

            return (result, null);
        }

        public async Task<ValidarCodigoActivacionResponseDto?> ValidarCodigoActivacion(ValidarCodigoActivacionRequestDto dto)
        {
            var client = _httpClientFactory.CreateClient("Andes-NoJWT");
            var response = await client.PostAsJsonAsync("modules/mobileApp/login", dto);

            return await response.Content.ReadFromJsonAsync<ValidarCodigoActivacionResponseDto>();
        }

        public async Task<(CrearContraseniaResponseDto? response, string? errorMessage)> CrearContrasenia(CrearContraseniaRequestDto dto)
        {
            var client = _httpClientFactory.CreateClient("Andes-NoJWT");
            var response = await client.PostAsJsonAsync("modules/mobileApp/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ValidarCodigoActivacionErrorDto>();
                return (null, res?.Error);
            }

            var result = await response.Content.ReadFromJsonAsync<CrearContraseniaResponseDto>();
            return (result, null);
        }
    }
}
