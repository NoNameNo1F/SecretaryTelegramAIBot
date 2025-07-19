using SecretaryTelegramAIBot.Domain.SeedWork;

namespace SecretaryTelegramAIBot.Domain.Entities.BrandAggregate
{
    public class Brand : IAggregateRoot, ISoftDelete, ITrackable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}