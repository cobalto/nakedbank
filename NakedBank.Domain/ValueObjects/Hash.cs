namespace NakedBank.Domain
{
    public sealed class Hash
    {
        private string _hash;

        public Hash(string hash)
        {
            _hash = hash;
        }

        public override string ToString()
        {
            return _hash;
        }
    }
}