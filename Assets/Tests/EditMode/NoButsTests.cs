using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NoButsTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void Answer()
    {
        string correctAnswer = "Yes";
        string incorrectAnswer = "...but?";
        Assert.AreEqual(expected: correctAnswer, actual: incorrectAnswer);
        //Assert.AreEqual(expected: correctAnswer, actual: NoButs.answer);
    }

    
}
