using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NaiveSyllableCounter;
using HaikuFinder;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var syllableCounter = new SyllableCounter();

            var syllableCount = syllableCounter.Count("blessed");
            Assert.AreEqual(1, syllableCount);

            syllableCount = syllableCounter.Count("blaster");
            Assert.AreEqual(2, syllableCount);

            // 3 apparently
            syllableCount = syllableCounter.Count("ambrosia");
            Assert.AreEqual(3, syllableCount);

            syllableCount = syllableCounter.Count("nastier");
            Assert.AreEqual(3, syllableCount);

            syllableCount = syllableCounter.Count("sappier");
            Assert.AreEqual(3, syllableCount);

            syllableCount = syllableCounter.Count("choir");
            Assert.AreEqual(1, syllableCount);

            syllableCount = syllableCounter.Count("queer");
            Assert.AreEqual(1, syllableCount);

            syllableCount = syllableCounter.Count("query");
            Assert.AreEqual(2, syllableCount);

            syllableCount = syllableCounter.Count("jumped");
            Assert.AreEqual(1, syllableCount);

            syllableCount = syllableCounter.Count("over");
            Assert.AreEqual(2, syllableCount);

            syllableCount = syllableCounter.Count("Australian");
            Assert.AreEqual(4, syllableCount);

            syllableCount = syllableCounter.Count("Polio");
            Assert.AreEqual(3, syllableCount);

            syllableCount = syllableCounter.Count("The Australian platypus is seemingly a hybrid of a mammal and reptilian creature.");
            Assert.AreEqual(26, syllableCount);

            syllableCount = syllableCounter.Count("mocha");
            Assert.AreEqual(2, syllableCount);

            syllableCount = syllableCounter.Count("jalapeno");
            Assert.AreEqual(4, syllableCount);

            syllableCount = syllableCounter.Count("hyperbole");
            Assert.AreEqual(4, syllableCount);

            syllableCount = syllableCounter.Count("forte");
            Assert.AreEqual(2, syllableCount);

            syllableCount = syllableCounter.Count("thermopylae");
            Assert.AreEqual(4, syllableCount);

            syllableCount = syllableCounter.Count("formulae");
            Assert.AreEqual(3, syllableCount);

            syllableCount = syllableCounter.Count("antennae");
            Assert.AreEqual(3, syllableCount);

            syllableCount = syllableCounter.Count("tae");
            Assert.AreEqual(1, syllableCount);

            syllableCount = syllableCounter.Count("hour");
            Assert.AreEqual(1, syllableCount);

            syllableCount = syllableCounter.Count("fire");
            Assert.AreEqual(1, syllableCount);

            // this passesby luck of a diphthong
            syllableCount = syllableCounter.Count("layer");
            Assert.AreEqual(1, syllableCount);

            syllableCount = syllableCounter.Count("rhythm");
            Assert.AreEqual(2, syllableCount);

            syllableCount = syllableCounter.Count("algorithm");
            Assert.AreEqual(4, syllableCount);

            syllableCount = syllableCounter.Count("logorithm");
            Assert.AreEqual(4, syllableCount);

            syllableCount = syllableCounter.Count("aeolian");
            Assert.AreEqual(4, syllableCount);
        }


        [TestMethod]
        public void TestMethod2()
        {
            var thisCounter = new HaikuFinder.HaikuFinder();            
            var isHaiku = thisCounter.IsHaiku("one two three four five, one two three four five, one two, one two three four five.");
            
            Assert.AreEqual(true, isHaiku);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var thisCounter = new HaikuFinder.HaikuFinder();
            var asHaiku = thisCounter.OutputAsHaiku("one two three four five, one two three four five, one two, one two three four five.");
            Console.WriteLine(asHaiku);
        }
    }
}
