using SecretaryTelegramAIBot.Domain.SeedWork;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Brands
{
    public class BrandAttribute
    {
        internal BrandAttributeId Id { get; private set; }
        internal BrandId BrandId { get; private set; } 
        public string Name { get; private set; }
        public string NormalizedName { get; private set; }
        public string Description { get; private set; }
        public string Value { get; private set; }

        private BrandAttribute() { }

        private BrandAttribute(
            BrandId brandId, 
            string name, 
            string normalizedName,
            string description,
            string value)
        {
            Id = new BrandAttributeId(0);
            BrandId = brandId;
            Name = name;
            NormalizedName = normalizedName;
            Description = description;
            Value = value;
        }

        internal static BrandAttribute CreateNew(
            BrandId brandId,
            string name,
            string normalizedName,
            string description,
            string value)
        {
            return new BrandAttribute(brandId, name, normalizedName, description, value);
        }
        
        // internal void UpdateCommand(string name)
        suy nghi them di zzz
    }
}