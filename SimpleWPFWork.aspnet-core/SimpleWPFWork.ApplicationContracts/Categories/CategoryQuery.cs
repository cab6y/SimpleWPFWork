namespace SimpleWPFWork.ApplicationContracts.Categories
{
    public class CategoryQuery
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Limit = 10;
        public int Page = 0;
    }
}
