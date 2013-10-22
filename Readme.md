SyllalbeCounter - Implementation of a naive syllable counting algorithm
=======================================================================

Features
--------
Project contains a simple use class to count syllables and also an implementation of a Haiku finder.


Create an instance of the Syllable Counter class to use.
--------------------------------------------------------

```csharp
var syllableCounter = new SyllableCounter();

var syllableCount = syllableCounter.Count("blessed");
Assert.AreEqual(1, syllableCount);

syllableCount = syllableCounter.Count("blaster");
Assert.AreEqual(2, syllableCount);
```

Create an instance of the Haiku finder to use
---------------------------------------------

```csharp
var thisCounter = new HaikuFinder.HaikuFinder();
var isHaiku = thisCounter.IsHaiku("one two three four five, one two three four five, one two, one two three four five.");
Assert.AreEqual(true, isHaiku);
```


Method
------
The main syllable counting class uses a series of injected rules to appraise the total


Author
------
GC