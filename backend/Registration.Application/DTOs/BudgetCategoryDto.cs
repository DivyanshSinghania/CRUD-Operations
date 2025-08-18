namespace Registration.Application.DTOs
{
    public class BudgetCategoryDto
    {
        public int Id{ get; set; }
        public string CategoryType { get; set; } = string.Empty;
        public decimal BudgetAmount { get; set; }
        public int FinancialYear { get; set; }
    }
}