namespace SimpleWPFWork.Domain.Entities
{
    public class BaseEntity : ISoftDelete
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastModificationTime { get; set; }
        // ISoftDelete implementation
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? DeleterUserId { get; set; }
    }
}
