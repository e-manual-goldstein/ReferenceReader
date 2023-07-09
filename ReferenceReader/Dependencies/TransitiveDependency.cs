namespace ReferenceReader.Dependencies
{
    public class TransitiveDependency : ITransitiveDependency
    {
        public string Name { get; }

        public TransitiveDependency(string name)
        {
            Name = name;
        }
    }
}