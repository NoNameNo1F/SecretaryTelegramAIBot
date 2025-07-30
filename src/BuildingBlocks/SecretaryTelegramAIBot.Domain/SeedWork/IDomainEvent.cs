namespace SecretaryTelegramAIBot.Domain.SeedWork
{
    public interface IDomainEvent
    {
        public Guid Id { get; set; }
    }
}