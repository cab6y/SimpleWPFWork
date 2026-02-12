using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategoryList
{
    public class GetCategoryListQuery : IRequest<List<CategoryDto>>
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Limit { get; set; } = 10;
        public int Page { get; set; } = 0;
    }
}
