using Gene.Middleware.Entities.Identity;
using System;

namespace Gene.Middleware.Bases
{
    public interface IEntity<TKey> : IIdentified<TKey>
    {
        public Status? Status { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public TKey CreatedUserId { get; set; }
        public TKey UpdatedUserId { get; set; }

        public User CreatedUser { get; set; }
        public User UpdatedUser { get; set; }
    }
}
