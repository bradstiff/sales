using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class ListPageViewModel<T>
    {
        public ListPageViewModel(int totalCount, int page, List<T> records)
        {
            TotalCount = totalCount;
            Page = page;
            Rows = records;
        }

        public int TotalCount { get; private set; }
        public int Page { get; private set; }
        public List<T> Rows { get; private set; }
    }
}
