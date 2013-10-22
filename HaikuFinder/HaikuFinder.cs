using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaiveSyllableCounter;

namespace HaikuFinder
{
    public class HaikuFinder
    {
        SyllableCounter _syllableCounter;

        public HaikuFinder()
        {
            _syllableCounter = new SyllableCounter();
        }

        public string OutputAsHaiku(string candidate)
        {
            StringBuilder thisOutput = new StringBuilder();
            int syllablesEncountered = 0;

            if (IsHaiku(candidate) == false)
            {
                return "No Haiku input";
            }
            else
            {
                var words = candidate.Split(' ');
                foreach (var item in words)
                {
                    thisOutput.Append(item + " ");
                    syllablesEncountered += _syllableCounter.Count(item);
                    switch (syllablesEncountered)
                    {
                        case 5:
                        case 12:
                        case 17:
                            thisOutput.Append(Environment.NewLine);
                            break;
                    }
                }
                return thisOutput.ToString();
            }
        }

        public bool IsHaiku(string candidate)
        {
            var words = candidate.Split(' ');
            int syllablesEncountered = 0;
            bool fiveSeen = false;
            bool twelveSeen = false;
            bool seventeenSeen = false;

            if (_syllableCounter.Count(candidate) == 17)
            {
                foreach (var item in words)
                {
                    syllablesEncountered += _syllableCounter.Count(item);
                    switch (syllablesEncountered)
                    {
                        case 5:
                            fiveSeen = true;
                            break;
                        case 12:
                            twelveSeen = true;
                            break;
                        case 17:
                            seventeenSeen = true;
                            break;
                    }
                }
                return (fiveSeen && twelveSeen && seventeenSeen) ? true : false;
            }
            else
            {
                return false;
            }
        }


    }

}

