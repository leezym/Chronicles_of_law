using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE.LogicalLines
{
    public static class LogicalLineUtils
    {
        public static class Encapsulation
        {
            public struct EncapsulatedData
            {
                public List<string> lines;
                public int startingIndex;
                public int endingIndex;
            }

            private const char CHOICE_IDENTIFIER = '-';
            private const char ENCAPSULATION_START = '{';
            private const char ENCAPSULATION_END = '}';

            public static EncapsulatedData RipEncapsulationData(Conversation conversation, int startingIndex)
            {
                int encapsulationDepth = 0;
                EncapsulatedData data = new EncapsulatedData{ lines = new List<string>(), startingIndex = startingIndex, endingIndex = 0 };

                for(int i = startingIndex; i < conversation.Count; i++)
                {
                    string line = conversation.GetLines()[i];
                    data.lines.Add(line);

                    if(IsEncapsulationStart(line))
                    {
                        encapsulationDepth++;
                        continue;
                    }

                    if(IsEncapsulationEnd(line))
                    {
                        encapsulationDepth--;
                        if(encapsulationDepth == 0)
                        {
                            data.endingIndex = i;
                            break;
                        }
                    }
                }

                return data;
            }

            public static bool IsChoiceStart(string line) => line.Trim().StartsWith(CHOICE_IDENTIFIER);
            public static bool IsEncapsulationStart(string line) => line.Trim().StartsWith(ENCAPSULATION_START);
            public static bool IsEncapsulationEnd(string line) => line.Trim().StartsWith(ENCAPSULATION_END);
        }
    }
}