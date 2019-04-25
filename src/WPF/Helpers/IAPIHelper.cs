using System.Net.Http;
using System.Threading.Tasks;
using TouchTest.WPF.Models;

namespace TouchTest.WPF.Helpers
{
    public interface IAPIHelper
    {
        HttpClient APIClient { get; set; }

        string BearerAuth { get; set; }

        Task<AuthenticatedUser> AuthAsync(string user, string pass);
    }
}