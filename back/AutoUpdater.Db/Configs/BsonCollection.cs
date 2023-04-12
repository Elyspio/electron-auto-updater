namespace AutoUpdater.Db.Configs
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        private string _collectionName;
        public BsonCollectionAttribute(string collectionName)
        {
            _collectionName = collectionName;
        }
        public string CollectionName => _collectionName;
    }
}
