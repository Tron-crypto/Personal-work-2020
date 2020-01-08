using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LineraAlgebra;

public class NewBehaviourScript : MonoBehaviour {

    [Header("Refrences")]
    public carControls controller;

    [Header("Controls")]
    public int initalPopulation = 85;
    [Range(0.0f, 1.0f)]
    public float mutationRate = 0.055f;

    [Header("crossover controls")]
    public int bestAgentSelection = 8;
    public int worstAgentSelecion = 3;
    public int numberToCrossover;

    private List<int> genePool = new List<int>();
    private int naturallySelected;

    private NNet[] population;

    [Header("public view")]
    public int currentGeneration;
    public int currentGenome;

    private void CreatePopulation()
    {
        population = new NNet[initalPopulation];
        FillPopulationWithRandomValues(population, 0);
        ResetToCurrentGenome();
    }

    private void ResetToCurrentGenome()
    {
        controller.ResetWithNetwork(population[currentGenome]);
    }
    private void FillPopulationWithRandomValues (NNet[] newPopulation, int startingIndex)
    {
        while (startingIndex< initalPopulation)
        {
            newPopulation[startingIndex] = new NNet();
            newPopulation[startingIndex].Initialise(controller.LAYERS, controller.NEURONS);
            startingIndex++;
        }
    }

    public void Death (float fitness, NNet network)
    {
        if (currentGeneration < population.Length-1)
        {

            population[currentGenome].fitness - fitness;
            currentGenome++;
            ResetToCurrentGenome();
        }
    }
}
