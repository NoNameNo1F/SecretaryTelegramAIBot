namespace SecretaryTelegramAIBot.Domain.SeedWork
{
    public interface ISoftDelete
    {
        public DateTime DeletedAt { get; set; }
        public Guid DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}