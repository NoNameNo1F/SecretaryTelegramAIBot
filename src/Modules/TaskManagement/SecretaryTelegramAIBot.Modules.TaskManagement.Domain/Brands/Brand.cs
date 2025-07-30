using SecretaryTelegramAIBot.Domain.SeedWork;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Brands
{
    public class Brand : IAggregateRoot, ITrackable, ISoftDelete 
    {
        public BrandId Id  { get; private set; }
        public string Name { get; private set; }
        public string NormalizedName { get; private set; }
        public EBrandType Type { get; private set; }
        
        private readonly List<Note> _notes = new();
        public IReadOnlyCollection<Note> Notes => _notes.AsReadOnly();
        
        private readonly List<BrandAttribute> _brandAttributes = new();
        public IReadOnlyCollection<BrandAttribute> BrandAttributes => _brandAttributes.AsReadOnly();
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        
        private Brand() { }
    }
}