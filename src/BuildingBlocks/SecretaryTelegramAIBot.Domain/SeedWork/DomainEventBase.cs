namespace SecretaryTelegramAIBot.Domain.SeedWork
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public Guid Id { get; set; }

        protected DomainEventBase()
        {
        }
        
        protected DomainEventBase(Guid id)
        {
            Id = id;
        }
    }
}