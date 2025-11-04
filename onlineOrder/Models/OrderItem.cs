namespace onlineOrder.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        //EF Core 知道 OrderId 是指向 Order.Id,这是命名约定：外键字段名 = 导航属性名 + Id → 自动匹配
        public int OrderId { get; set; }  // 外键

        //导航属性（Navigation Property），它的作用不是让你去提交数据，而是 在 C# 里表示关系。
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
