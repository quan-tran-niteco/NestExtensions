using Nest;
using System;

namespace NestExtensions.Sorts
{
    public class ScoreSort<TDocument> : Sort<TDocument> where TDocument : class
    {
        public override SortDescriptor<TDocument> DescribeTo(SortDescriptor<TDocument> sortDescriptor)
        {
            if (sortDescriptor == null)
            {
                throw new ArgumentNullException(nameof(sortDescriptor));
            }

            return sortDescriptor.Field("_score", SortOrder);
        }
    }
}
