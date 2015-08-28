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

        public static FriendRequestViewModel Create(FriendRequest request)
        {
            return new FriendRequestViewModel()
            {
                Id = request.Id,
                Status = request.Status,
                User = MinifiedUserViewModel.Create(request.From)
            };
        }
    }
}