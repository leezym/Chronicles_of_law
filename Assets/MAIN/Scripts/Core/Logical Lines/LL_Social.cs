using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static DIALOGUE.LogicalLines.LogicalLineUtils.Encapsulation;

namespace DIALOGUE.LogicalLines
{
    public class LL_Social : ILogicalLine
    {
        public string keyword => "social";

        public IEnumerator Execute(DIALOGUE_LINE line)
        {
            var currentConversation = DialogueSystem.Instance.conversationManager.conversation;
            var progress = DialogueSystem.Instance.conversationManager.conversationProgress;
            EncapsulatedData data = RipEncapsulationData(currentConversation, progress);
            List<Social> socials = GetChoicesFromData(data);

            string title = line.dialogueData.rawData;
            ChoicePanel panel = ChoicePanel.Instance;
            string[] socialTitles = socials.Select(c => c.title).ToArray();

            panel.Show(title, socialTitles);

            while(panel.isWaitingOnUserChoice)
                yield return null;
            
            Social selectedChoice = socials[panel.lastDecision.answerIndex];

            Conversation newConversation = new Conversation(selectedChoice.resultLines);
            DialogueSystem.Instance.conversationManager.conversation.SetProgress(data.endingIndex);
            DialogueSystem.Instance.conversationManager.EnqueuePriority(newConversation);
        }

        public bool Matches(DIALOGUE_LINE line)
        {
            return (line.hasSpeaker && line.speakerData.name.ToLower() == keyword);
        }

        private List<Social> GetChoicesFromData(EncapsulatedData data)
        {
            List<Social> socials = new List<Social>();
            int encapsulationDepth = 0;
            bool isFirstChoice = true;

            Social social = new Social
            {
                title = string.Empty,
                resultLines = new List<string>()
            };

            foreach (var line in data.lines.Skip(1))
            {
                if(IsChoiceStart(line) && encapsulationDepth == 1)
                {
                    if(!isFirstChoice)
                    {
                        socials.Add(social);
                        social = new Social
                        {
                            title = string.Empty,
                            resultLines = new List<string>()
                        };
                    }     

                    social.title = line.Trim().Substring(1);
                    isFirstChoice = false;

                    continue;
                }

                AddLineToResults(line, ref social, ref encapsulationDepth);                
            }

            if(!socials.Contains(social))
                socials.Add(social);

            return socials;
        }

        private void AddLineToResults(string line, ref Social social, ref int encapsulationDepth)
        {
            line.Trim();

            if(IsEncapsulationStart(line))
            {
                if(encapsulationDepth > 0)
                    social.resultLines.Add(line);

                encapsulationDepth++;
                return;
            }

            if(IsEncapsulationEnd(line))
            {
                encapsulationDepth--;

                if(encapsulationDepth > 0)
                    social.resultLines.Add(line);
                
                return;
            }

            social.resultLines.Add(line);
        }        

        private struct Social
        {
            public string title;
            public List<string> resultLines;
        }
    }
}