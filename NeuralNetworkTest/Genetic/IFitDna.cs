namespace NeuralNetworkTest.Genetic
{
    public interface IFitDna : IDna
    {
        double Fitness { get; set; }
    }
}