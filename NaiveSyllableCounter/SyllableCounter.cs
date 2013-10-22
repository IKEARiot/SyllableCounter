using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NaiveSyllableCounter
{
    public class RuleVisitor : ISyllable
    {
        private int _syllableCount = 0;
        private string _candidateWord = "";

        #region ISyllable Members

        public int Count
        {
            get
            {
                return _syllableCount;
            }
            set
            {
                _syllableCount = value;
            }
        }

        public string CandidateWord
        {
            get
            {
                return _candidateWord;
            }
            set
            {
                _candidateWord = value;
            }
        }

        #endregion

        public RuleVisitor()
        {
        }
        public RuleVisitor(string candidateWord)
        {
            _candidateWord = candidateWord;
        }
    }

    public class SyllableCounter
    {
        public IEnumerable<ISyllableRule> Rules { get; set; }

        private int VowelRule(ISyllable thisCounter)
        {
            List<string> vowels = new List<string> { "a", "e", "i", "o", "u", "y" };
            var vowelCount = thisCounter.CandidateWord.Count(x => vowels.Contains(x.ToString()));
            return vowelCount;
        }

        public SyllableCounter()
        {
            var rules = new List<ISyllableRule>();
            rules.Add(new VowelRule());
            rules.Add(new DiphThongRule());
            rules.Add(new PronouncedESuffix());
            rules.Add(new SuffixExceptionRule("ed", SuffixExceptionRule.SuffixRule.SubOne));
            rules.Add(new SuffixExceptionRule("sed", SuffixExceptionRule.SuffixRule.SubOne));
            rules.Add(new SuffixExceptionRule("ier", SuffixExceptionRule.SuffixRule.AddOne));
            rules.Add(new SuffixExceptionRule("ian", SuffixExceptionRule.SuffixRule.AddOne));
            rules.Add(new SuffixExceptionRule("lio", SuffixExceptionRule.SuffixRule.AddOne));
            rules.Add(new SuffixExceptionRule("ae", SuffixExceptionRule.SuffixRule.AddOne));
            rules.Add(new SuffixExceptionRule("ithm", SuffixExceptionRule.SuffixRule.AddOne));
            rules.Add(new SuffixExceptionRule("ythm", SuffixExceptionRule.SuffixRule.AddOne));

            //for simplicity, oh shit must be last
            rules.Add(new OhShitRule());

            this.Rules = rules;
        }

        public int Count(string candidate)
        {
            int count = 0;
            var words = candidate.Split(' ');

            foreach (var word in words)
            {
                ISyllable _visitor = new RuleVisitor();
                _visitor.CandidateWord = StripPunctuation(word);

                foreach (var item in Rules)
                {
                    _visitor.Count += item.Calculate(_visitor);
                }

                count += _visitor.Count;
            }
            return count;
        }

        private string StripPunctuation(string candidate)
        {
            return new string(candidate.Where(c => !char.IsPunctuation(c)).ToArray());
        }

    }

    public interface ISyllable
    {
        int Count { get; set; }
        string CandidateWord { get; set; }
    }

    public class VowelRule : ISyllableRule
    {
        List<string> vowels = new List<string> { "a", "e", "i", "o", "u", "y" };

        #region ISyllableRule Members

        public int Calculate(ISyllable counter)
        {
            var vowelCount = counter.CandidateWord.Count(x => vowels.Contains(x.ToString()));
            return vowelCount;
        }

        #endregion
    }

    public class PronouncedESuffix : ISyllableRule
    {
        static HashSet<string> SuffixE;

        public PronouncedESuffix()
        {
            if (SuffixE == null)
            {
                SuffixE = new HashSet<string>();
                SuffixE.Add("hyperbole");
                SuffixE.Add("forte");
            }
        }

        #region ISyllableRule Members

        public int Calculate(ISyllable counter)
        {
            var candidateWord = counter.CandidateWord.ToLower();

            if (candidateWord.EndsWith("e") && (candidateWord.Length > 1))
            {
                if (SuffixE.Contains(candidateWord))
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }

    public class DiphThongRule : ISyllableRule
    {
        List<string> vowels = new List<string> { "a", "e", "i", "o", "u", "y" };
        static HashSet<string> naiveDiphthongs;

        public DiphThongRule()
        {
            if (naiveDiphthongs == null)
            {
                //cartesian product of vowels v. vowels
                var quickDiphthongs = (from first in vowels
                                       from second in vowels
                                       select first.ToString() + second.ToString()).ToList();

                naiveDiphthongs = new HashSet<string>();
                quickDiphthongs.ForEach(x => naiveDiphthongs.Add(x));
            }
        }

        #region ISyllableRule Members
        public int Calculate(ISyllable counter)
        {
            return -CountDipthongs(counter.CandidateWord);
        }
        #endregion

        private int CountDipthongs(string candidate)
        {
            string temp;
            int result = 0;

            for (int i = 0; i < candidate.Length; i++)
            {
                if (i + 2 <= candidate.Length)
                {
                    temp = candidate.Substring(i, 2);
                    if (naiveDiphthongs.Contains(temp))
                    {
                        result++;

                        //getting in a mess here...contrast words with tripthongs like aeolian and quaint.
                        if (i + 3 <= candidate.Length)
                        {
                            temp = candidate.Substring(i + 2, 1);
                            if (vowels.Contains(temp))
                            {
                                // this is a tripthong...did it start with a Q?
                                i = i + candidate.Substring(i, 1) == "q" ? 0 : 1;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }

    public class SuffixExceptionRule : ISyllableRule
    {
        private string _suffix = "";
        private SuffixRule _rule;
        private int _arbitraryResult;

        public SuffixExceptionRule(string suffix)
        {
            _suffix = suffix;
        }

        public SuffixExceptionRule(string suffix, int userdefinedValue)
            : this(suffix)
        {
            _arbitraryResult = userdefinedValue;
        }

        public SuffixExceptionRule(string suffix, SuffixRule rule)
            : this(suffix)
        {
            _rule = rule;
        }

        #region ISyllableRule Members

        public int Calculate(ISyllable counter)
        {
            int result = 0;

            if (counter.CandidateWord.EndsWith(_suffix) && (counter.CandidateWord.Length > _suffix.Length))
            {
                if (_rule == SuffixRule.UserDefined)
                {
                    result = _arbitraryResult;
                }
                else
                {
                    switch (_rule)
                    {
                        case SuffixRule.AddNothing:
                            result = 0;
                            break;
                        case SuffixRule.AddOne:
                            result = 1;
                            break;
                        case SuffixRule.SubOne:
                            result = -1;
                            break;
                    }
                }
            }
            else
            {
                result = 0;
            }


            return result;
        }

        #endregion

        public enum SuffixRule
        {
            UserDefined = 0,
            AddNothing = 1,
            AddOne = 2,
            SubOne = 3
        }

    }

    public class OhShitRule : ISyllableRule
    {
        #region ISyllableRule Members

        public int Calculate(ISyllable counter)
        {
            return (counter.Count == 0) ? 1 : 0;
        }

        #endregion
    }

    public interface ISyllableRule
    {
        int Calculate(ISyllable counter);
    }

}
