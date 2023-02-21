using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace RaknaAPI.Models.Auth
{
    public class User:IdentityUser<Guid>
    {
    }
}
