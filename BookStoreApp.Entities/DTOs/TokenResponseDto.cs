using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Entities.DTOs
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }
    }
}
