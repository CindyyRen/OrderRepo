namespace onlineOrder.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; } // 从 Product 导航属性取
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // 加上
    }
}
