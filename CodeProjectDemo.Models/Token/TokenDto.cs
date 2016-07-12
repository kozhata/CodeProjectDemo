using System;

namespace CodeProjectDemo.Models.Token
{
    public class TokenDto
    {
        public int Id { get; set; }

        public int UserFk { get; set; }

        public string AuthToken { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiresOn { get; set; }

        public RolesEnum Roles { get; set; }
    }
}
