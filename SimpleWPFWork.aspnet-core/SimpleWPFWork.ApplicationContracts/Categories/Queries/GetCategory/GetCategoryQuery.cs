using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategory
{
    public class GetCategoryQuery : IRequest<CategoryDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
