using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don.Service
{
    public class BizPagedResult<T> : BizResultBase
    {
        public IEnumerable<T> Rows { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; } = 15;
        /// <summary>
        /// 记录总数
        /// </summary>
        public int Records { get; set; }

        public dynamic Userdata { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int Total
        {
            get
            {
                if (PageSize <= 0)
                {
                    PageSize = 15;
                }
                return (Records + PageSize - 1) / PageSize;
            }
        }

        public BizPagedResult<T> Succeed(int pageIndex, int pageSize, IEnumerable<T> data, int recordCount)
        {
            Page = pageIndex;
            PageSize = pageSize;
            Rows = data;
            Records = recordCount;
            Code = 0;
            return this;
        }

        public async Task<BizPagedResult<T>> SucceedAsync(IQueryable<T> queryable, int pageIndex, int pageSize)
        {
            var pagedQuery = queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            Page = pageIndex;
            PageSize = pageSize;
            Rows = await pagedQuery.ToListAsync();
            Records = await queryable.CountAsync();
            Code = 0;
            return this;
        }
    }
}
