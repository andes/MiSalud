namespace AndesServices.Entities
{
    public class LoginReq
    {
        public string? email { get; set; }
        public string? password { get; set; }
    }

    public class LoginResp
    {
        public string? token { get; set; }
        public User? user { get; set; }

    }
}
