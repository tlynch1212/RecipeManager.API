using Microsoft.ML.Data;

namespace RecipeManager.Core.Recommendations
{
    public interface ITrainer
    {
        RegressionMetrics TestModel();
        RegressionMetrics Train(int numberOfIterations, int approximationRank, double learningRate);
    }
}