using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace CodeProjectDemo.Demo.Extensions
{
    public static class Roles
    {
        public static IEnumerable<Claim> CreateRolesBasedOnClaims(this ClaimsIdentity identity)
        {
            List<Claim> claims = new List<Claim>();

            if (identity.HasClaim(c => c.Type == "FTE" && c.Value == "1") &&
                identity.HasClaim(ClaimTypes.Role, "Admin")/*&& identity.HasClaim("FTE","1")*/)
            {
                claims.Add(new Claim(ClaimTypes.Role, "IncidentResolvers"));
            }

            return claims;
        }
    }
}