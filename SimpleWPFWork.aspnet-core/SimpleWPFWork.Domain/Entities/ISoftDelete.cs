using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.Domain.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletionTime { get; set; }
        Guid? DeleterUserId { get; set; }  // Veya Guid, ihtiyacınıza göre
    }
}
