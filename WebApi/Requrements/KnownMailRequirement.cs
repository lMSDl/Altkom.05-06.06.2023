using Microsoft.AspNetCore.Authorization;

namespace WebApi.Requrements
{
    public class KnownMailRequirement : IAuthorizationRequirement
    {
        public string Domain { get; set; }

        public KnownMailRequirement(string domain)
        {
            Domain = domain;
        }

    }
}
