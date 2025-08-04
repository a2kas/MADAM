namespace Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockModel
{
    public int Id { get; set; }
    public int? WholesaleQuantity { get; set; }
    public decimal? RetailQuantity { get; set; }
    public decimal QtyToBuy => (WholesaleQuantity ?? 0) - (RetailQuantity ?? 0);
}
