using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastModificationTime { get; set; }
    }
}
