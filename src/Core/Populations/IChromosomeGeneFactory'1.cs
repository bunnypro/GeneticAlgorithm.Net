namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public interface IChromosomeGeneFactory<out T>
    {
        T CreateGeneAt(int position);
    }
}