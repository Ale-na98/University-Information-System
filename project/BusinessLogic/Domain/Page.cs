using System.Collections.Generic;

namespace BusinessLogic.Domain
{
    public struct Page<T>
    {
        public IList<T> Data { get; set; }
        public int TotalPages { get; set; }
    }
}
