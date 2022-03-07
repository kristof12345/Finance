namespace Finance.Models;

public class PriceIndex
{
    public string CurrencyId { get; set; }

    public DateTime Date { get; set; }

    public decimal Value { get; set; }
}

