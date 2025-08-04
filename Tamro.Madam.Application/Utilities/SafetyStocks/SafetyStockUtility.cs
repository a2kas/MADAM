namespace Tamro.Madam.Application.Utilities.SafetyStocks;
public static class SafetyStockUtility
{
    private const string NarcoticsItemGroup = "NAR";
    private const string PsychotropicsItemGroup = "PSY";
    private const int SpecialGroupBuyDays = 30;
    private const int RegularBuyDays = 10;

    public static int GetCheckDays(string itemGroup)
    {
        return itemGroup == PsychotropicsItemGroup || itemGroup == NarcoticsItemGroup
            ? SpecialGroupBuyDays
            : RegularBuyDays;
    }
}
