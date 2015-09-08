using FacebookSystem.Models;
using FacebookSystem.Models.Enums;
using System.Linq;

namespace FacebookSystem.Services.Models
{
    public class FriendRequestViewModel
    {
        public int Id { get; set; }

        public FriendRequestStatus Status { get; set; }

        public MinifiedUserViewModel User { get; set; }
    }
}