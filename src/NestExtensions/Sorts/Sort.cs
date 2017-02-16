using Nest;
using System;
using System.Linq.Expressions;

namespace NestExtensions.Sorts
{
    public class Sort<TDocument> where TDocument : class
    {
        public Expression<Func<TDocument, object>> Path { get; set; }

        public SortOrder SortOrder { get; set; }

        public virtual SortDescriptor<TDocument> DescribeTo(SortDescriptor<TDocument> sortDescriptor)
        {
            if (sortDescriptor == null)
            {
                throw new ArgumentNullException(nameof(sortDescriptor));
            }

            return sortDescriptor.Field(Path, SortOrder);
        }
    }
}
