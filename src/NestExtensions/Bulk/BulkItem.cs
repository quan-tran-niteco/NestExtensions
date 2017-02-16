namespace NestExtensions.Bulk
{
    public class BulkItem<TDocument> where TDocument : class
    {
        public TDocument Document { get; set; }

        public string Id { get; set; }

        public string Routing { get; set; }

        public BulkOperation Operation { get; set; }
    }
}
