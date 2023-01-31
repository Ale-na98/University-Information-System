using System.Collections.Generic;

namespace DataAccess
{
    public struct Page<T>
    {
        public IList<T> Data { get; set; }
        public int TotalPages { get; set; }
    }
}
