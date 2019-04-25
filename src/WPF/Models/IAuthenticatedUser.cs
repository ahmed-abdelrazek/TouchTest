namespace TouchTest.WPF.Models
{
    public interface IAuthenticatedUser
    {
        string AccessToken { get; set; }
        string Expires { get; set; }
        long ExpiresIn { get; set; }
        string Issued { get; set; }
        string TokenType { get; set; }
        string UserName { get; set; }
    }
}