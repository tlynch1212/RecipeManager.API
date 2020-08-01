using Microsoft.ML.Data;

namespace RecipeManager.Core.Recommendations
{
    public interface ITrainer
    {
        RegressionMetrics Train();
    }
}