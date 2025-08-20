using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathGeneratorMono : MonoBehaviour
{
    int[][] presetPaths = new int[5][];


    void Start()
    {
        presetPaths[0] = new int[] {5, 12, 11, 10, 9, 16, 23, 24, 31, 38, 45};
        presetPaths[1] = new int[] {6, 13, 12, 11, 18, 25, 32, 39, 38, 37, 44};
        presetPaths[2] = new int[] {0, 1, 2, 9, 10, 17, 24, 31, 38, 37, 36, 43};
        presetPaths[3] = new int[] {6, 13, 12, 11, 10, 9, 16, 15, 14, 21, 28, 35, 42};
        presetPaths[4] = new int[] {3, 10, 17, 18, 25, 32, 31, 30, 37, 44};
    }

    public int[] GetRandomPath()
    {
        System.Random rnd = new System.Random();
        int i  = rnd.Next(0, 5);

        return presetPaths[i];
    }

}

