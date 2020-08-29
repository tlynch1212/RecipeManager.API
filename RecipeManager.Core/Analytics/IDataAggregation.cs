namespace RecipeManager.Core.Analytics
{
    public interface IDataAggregation
    {
        TableData GetMostInteraction(int count);
        TableData GetRateDifferences();
        TableData GetTopRated(int count);
    }
}