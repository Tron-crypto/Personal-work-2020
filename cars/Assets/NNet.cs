using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using MathNet.Numerics.LinearAlgebra;///r:MathNet.Numerics.dll;

public class NNet : MonoBehaviour
{
    public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 3);
    public List<Matrix<float>> hiddenLayer = new List<Matrix<float>>();
    public Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 2);
    public List<Matrix<float>> weights = new List<Matrix<float>>();
    public List<float> biases = new List<float>();
    public float fitness;

    public void Initialise (int hiddenLayerCount, int hiddenNeuronCount)
    {
        inputLayer.Clear();
        hiddenLayer.Clear();
        outputLayer.Clear();
        weights.Clear();
        biases.Clear();

        for (int i=0; i < hiddenLayerCount +1; i++)
        {
            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);

            hiddenLayer.Add(f);
            biases.Add(Random.Range(-1f, 1f));



            // weights

            if (i==0)
            {
                Matrix<float> inputToh1 = Matrix<float>.Build.Dense(3, hiddenNeuronCount);
                weights.Add(inputToh1);
            }
            Matrix<float> HiddenTohidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
            weights.Add(HiddenTohidden);
        }
        Matrix<float> Outputweight = Matrix<float>.Build.Dense(hiddenNeuronCount, 2);
        weights.Add(Outputweight);
        biases.Add(Random.Range(-1f, 1f));

        Randomweights();
    }

    public void Randomweights()
    {
        for (int i=0; i< weights.Count; i++)
        {
            for (int x=0; x<weights[i].RowCount; x++)
            {
                for (int y=0;y< weights[i].ColumnCount; y++)
                {
                    weights[i][x, y] = Random.Range(-1f, 1f);
                }
            }

        }
    }

    public (float, float) RunNetwork (float a, float b, float c)
    {
        inputLayer[0, 0] = a;
        inputLayer[0, 1] = b;
        inputLayer[0, 2] = c;
        inputLayer = inputLayer.PointwiseTanh();

        hiddenLayer[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();
        for (int i =1; i <hiddenLayer.Count; i++)
        {
            hiddenLayer[i] = ((hiddenLayer[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }
        outputLayer = ((hiddenLayer[hiddenLayer.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]).PointwiseTanh();
        return (Sigmoid(outputLayer[0,0]), (float)Math.Tanh(outputLayer[0,1]));
    }

    private float Sigmoid (float s)
    {
        return (1 / (1 + Mathf.Exp(-s)));
    }
  

}





//using System.Collections.Generic;
//using System;
//using System.IO;

//public class NeuralNetwork : IComparable<NeuralNetwork>
//{
//    private int[] layers;//layers
//    private float[][] neurons;//neurons
//    private float[][] biases;//biasses
//    private float[][][] weights;//weights
//    private int[] activations;//layers

//    public float fitness = 0;//fitness

//    public NeuralNetwork(int[] layers)
//    {
//        this.layers = new int[layers.Length];
//        for (int i = 0; i < layers.Length; i++)
//        {
//            this.layers[i] = layers[i];
//        }
//        InitNeurons();
//        InitBiases();
//        InitWeights();
//    }

//    private void InitNeurons()//create empty storage array for the neurons in the network.
//    {
//        List<float[]> neuronsList = new List<float[]>();
//        for (int i = 0; i < layers.Length; i++)
//        {
//            neuronsList.Add(new float[layers[i]]);
//        }
//        neurons = neuronsList.ToArray();
//    }

//    private void InitBiases()//initializes and populates array for the biases being held within the network.
//    {
//        List<float[]> biasList = new List<float[]>();
//        for (int i = 0; i < layers.Length; i++)
//        {
//            float[] bias = new float[layers[i]];
//            for (int j = 0; j < layers[i]; j++)
//            {
//                bias[j] = UnityEngine.Random.Range(-0.5f, 0.5f);
//            }
//            biasList.Add(bias);
//        }
//        biases = biasList.ToArray();
//    }

//    private void InitWeights()//initializes random array for the weights being held in the network.
//    {
//        List<float[][]> weightsList = new List<float[][]>();
//        for (int i = 1; i < layers.Length; i++)
//        {
//            List<float[]> layerWeightsList = new List<float[]>();
//            int neuronsInPreviousLayer = layers[i - 1];
//            for (int j = 0; j < neurons[i].Length; j++)
//            {
//                float[] neuronWeights = new float[neuronsInPreviousLayer];
//                for (int k = 0; k < neuronsInPreviousLayer; k++)
//                {
//                    //float sd = 1f / ((neurons[i].Length + neuronsInPreviousLayer) / 2f);
//                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
//                }
//                layerWeightsList.Add(neuronWeights);
//            }
//            weightsList.Add(layerWeightsList.ToArray());
//        }
//        weights = weightsList.ToArray();
//    }

//    public float[] FeedForward(float[] inputs)//feed forward, inputs >==> outputs.
//    {
//        for (int i = 0; i < inputs.Length; i++)
//        {
//            neurons[0][i] = inputs[i];
//        }
//        for (int i = 1; i < layers.Length; i++)
//        {
//            int layer = i - 1;
//            for (int j = 0; j < neurons[i].Length; j++)
//            {
//                float value = 0f;
//                for (int k = 0; k < neurons[i - 1].Length; k++)
//                {
//                    value += weights[i - 1][j][k] * neurons[i - 1][k];
//                }
//                neurons[i][j] = activate(value + biases[i][j]);
//            }
//        }
//        return neurons[neurons.Length - 1];
//    }

//    public float activate(float value)
//    {
//        return (float)Math.Tanh(value);
//    }

//    public void Mutate(int chance, float val)//used as a simple mutation function for any genetic implementations.
//    {
//        for (int i = 0; i < biases.Length; i++)
//        {
//            for (int j = 0; j < biases[i].Length; j++)
//            {
//                biases[i][j] = (UnityEngine.Random.Range(0f, chance) <= 5) ? biases[i][j] += UnityEngine.Random.Range(-val, val) : biases[i][j];
//            }
//        }

//        for (int i = 0; i < weights.Length; i++)
//        {
//            for (int j = 0; j < weights[i].Length; j++)
//            {
//                for (int k = 0; k < weights[i][j].Length; k++)
//                {
//                    weights[i][j][k] = (UnityEngine.Random.Range(0f, chance) <= 5) ? weights[i][j][k] += UnityEngine.Random.Range(-val, val) : weights[i][j][k];
//                }
//            }
//        }
//    }

//    public int CompareTo(NeuralNetwork other) //Comparing For NeuralNetworks performance.
//    {
//        if (other == null) return 1;

//        if (fitness > other.fitness)
//            return 1;
//        else if (fitness < other.fitness)
//            return -1;
//        else
//            return 0;
//    }

//    public NeuralNetwork copy(NeuralNetwork nn) //For creatinga deep copy, to ensure arrays are serialzed.
//    {
//        for (int i = 0; i < biases.Length; i++)
//        {
//            for (int j = 0; j < biases[i].Length; j++)
//            {
//                nn.biases[i][j] = biases[i][j];
//            }
//        }
//        for (int i = 0; i < weights.Length; i++)
//        {
//            for (int j = 0; j < weights[i].Length; j++)
//            {
//                for (int k = 0; k < weights[i][j].Length; k++)
//                {
//                    nn.weights[i][j][k] = weights[i][j][k];
//                }
//            }
//        }
//        return nn;
//    }

//    public void Load(string path)//this loads the biases and weights from within a file into the neural network.
//    {
//        TextReader tr = new StreamReader(path);
//        int NumberOfLines = (int)new FileInfo(path).Length;
//        string[] ListLines = new string[NumberOfLines];
//        int index = 1;
//        for (int i = 1; i < NumberOfLines; i++)
//        {
//            ListLines[i] = tr.ReadLine();
//        }
//        tr.Close();
//        if (new FileInfo(path).Length > 0)
//        {
//            for (int i = 0; i < biases.Length; i++)
//            {
//                for (int j = 0; j < biases[i].Length; j++)
//                {
//                    biases[i][j] = float.Parse(ListLines[index]);
//                    index++;
//                }
//            }

//            for (int i = 0; i < weights.Length; i++)
//            {
//                for (int j = 0; j < weights[i].Length; j++)
//                {
//                    for (int k = 0; k < weights[i][j].Length; k++)
//                    {
//                        weights[i][j][k] = float.Parse(ListLines[index]); ;
//                        index++;
//                    }
//                }
//            }
//        }
//    }

//    public void Save(string path)//this is used for saving the biases and weights within the network to a file.
//    {
//        File.Create(path).Close();
//        StreamWriter writer = new StreamWriter(path, true);

//        for (int i = 0; i < biases.Length; i++)
//        {
//            for (int j = 0; j < biases[i].Length; j++)
//            {
//                writer.WriteLine(biases[i][j]);
//            }
//        }

//        for (int i = 0; i < weights.Length; i++)
//        {
//            for (int j = 0; j < weights[i].Length; j++)
//            {
//                for (int k = 0; k < weights[i][j].Length; k++)
//                {
//                    writer.WriteLine(weights[i][j][k]);
//                }
//            }
//        }
//        writer.Close();
//    }
//}