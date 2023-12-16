using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Graph;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FacesTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void FacesTestsSimplePasses()
    {
        var cube = new Cube(5,5,5);
        var squares = cube.AllSquares;
        var allHasFourNeighbors = squares.TrueForAll(square => square.Neighbors.Count() == 4);
        Assert.AreEqual(true, allHasFourNeighbors);
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
