namespace ExpenseApp.Engine.Request
{
    public class ExpenseGetRequest
    {
        public int UserId { get; set; }

        public int ExpenseId { get; set; }

        public int LastExpenseActionId { get; set; }

    }
}
