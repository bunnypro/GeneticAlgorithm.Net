using System.Collections.Generic;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IEvolutionStrategy
    {
        /// <summary>
        /// This method responsible for preparing population before being evolved
        /// eg. Population Initialization,
        ///     Store Population object to be used in Execute method
        /// </summary>
        /// <param name="population"></param>
        void Prepare(IPopulation population);
        
        void Execute(int evolutionNumber);
    }
}