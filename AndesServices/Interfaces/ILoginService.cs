using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface ILoginService<T>
    {
        Task<User> Login(string email, string password, Ref<string> mensaje);
        Task<bool> Logout(string token);
        Task<bool> Register(T user);
        Task<bool> UpdateUser(T user);
        Task<bool> DeleteUser(string id);
        Task<T> GetUserById(string id);
        Task<List<T>> GetAllUsers();
    }
}
