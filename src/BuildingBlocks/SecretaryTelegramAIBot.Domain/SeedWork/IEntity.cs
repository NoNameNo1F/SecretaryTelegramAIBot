namespace SecretaryTelegramAIBot.Domain.SeedWork
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}