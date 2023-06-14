using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FacesTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void FacesTestsSimplePasses()
    {
        // Use the Assert class to test conditions
        var cube = new Cube(5,5,5);
        var squares = cube.Faces.Select(face => face.Squares);
        var allHasFourNeighbors = squares.ToList().TrueForAll(squareFace =>
        {
            var width = squareFace.GetLength(0);
            var height = squareFace.GetLength(1);
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    if (squareFace[w, h].Neighbors.Count() != 4)
                    {
                        return false;
                    }
                }
            }

            return true;
        });
        Assert.AreEqual(allHasFourNeighbors, true);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator FacesTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
