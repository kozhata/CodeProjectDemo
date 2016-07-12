namespace CodeProjectDemo.Helpers
{
    using Models.RestBase;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http.OData.Query;

    public static class ODataQueryExtensions
    {
        static ODataQuerySettings _querySettings;

        public static ODataQuerySettings QuerySettings
        {
            get
            {
                if (_querySettings != null)
                {
                    return _querySettings;
                }

                _querySettings = new ODataQuerySettings
                {
                    PageSize = 1000
                };

                return _querySettings;
            }
        }

        public static async Task<PagedResponse<T>> ApplyQueryAsync<T>(this IQueryable<T> query, ODataQueryOptions<T> queryOptions) where T : class
        {
            if (queryOptions.Filter != null)
            {
                query = queryOptions.Filter.ApplyTo(query, QuerySettings) as IQueryable<T>;
            }

            int totalCount = await query.CountAsync();

            if (queryOptions.OrderBy != null)
            {
               query = queryOptions.OrderBy.ApplyTo(query) as IQueryable<T>;
            }

            if (queryOptions.Skip != null)
            {
                query = queryOptions.Skip.ApplyTo(query, QuerySettings) as IQueryable<T>;
            }

            if (queryOptions.Top != null)
            {
                query = queryOptions.Top.ApplyTo(query, QuerySettings) as IQueryable<T>;
            }

            List<T> list = await Task.Run(() =>
            {
                return query.ToList();
            });

            return new PagedResponse<T>
            {
                Data = list,
                TotalRecords = totalCount,
                TotalDisplayRecords = list.Count
            };
        }
    }
}
