using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessLogic.Domain
{
    public record PageParams
    {
        [BindRequired]
        public int CurrentPage { get; set; }

        [BindRequired]
        public int PageSize { get; set; }
    }
}
