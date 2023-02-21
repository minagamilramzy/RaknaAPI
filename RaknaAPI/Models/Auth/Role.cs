using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace RaknaAPI.Models.Auth
{
    public class Role:IdentityRole<Guid>
    {
    }
}
