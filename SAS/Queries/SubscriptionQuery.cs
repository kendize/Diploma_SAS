using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAS.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public class SubscriptionQuery : ISubscriptionQuery
    {
        private readonly ApplicationContext db;
        private readonly IMapper mapper;
        public SubscriptionQuery(ApplicationContext context,
                                 IMapper mapper)
        {
            db = context;
            this.mapper = mapper;
        }

        public async Task<UserCourseDTO> GetSubscriptionById(string Id)
        {
            UserCourseDTO result = await db.UserCourses.FirstOrDefaultAsync(x => x.Id == Id);
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<UserCourseDTO> FindSubscription(SubscriptionRequestDTO request)
        {
            UserCourseDTO result = await db.UserCourses.FirstOrDefaultAsync(x => x.CourseId == request.CourseId &&
                                                                                 x.UserId == request.UserId);
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<SubscriptionResponseDTO> GetAllUserSubscriptions(string UserId)
        {
            var subscriptions = await db.UserCourses.Where(sub => sub.UserId == UserId)
                                             .ToListAsync();

            SubscriptionResponseDTO result = new SubscriptionResponseDTO
            {
                userCourses = mapper.Map<List<UserCourseDTO>, List<UserCourseResponseDTO>>(subscriptions)
            };

            return result;
        }
    }
}
