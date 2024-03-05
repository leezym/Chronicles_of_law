using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using COMMANDS;
using CHARACTERS;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.Instance;
        private Coroutine process = null;
        public bool isRunning => process != null;
        private TextArchitect architect = null;
        private bool userPrompt = false;

        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));

            return process;
        }

        public void StopConversation()
        {
            if(!isRunning)
                return;
            
            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for(int i = 0; i < conversation.Count; i++)
            {
                //Dont show any blank lines or to run any logic on them
                if(string.IsNullOrWhiteSpace(conversation[i]))
                    continue;

                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                //Show dialogue
                if(line.hasDialogue)
                    yield return Line_RunDialogue(line);

                //Run any commands
                if(line.hasCommands)
                    yield return Line_RunCommands(line);

                //Wait for user input if dialogue wasa in this line
                if(line.hasDialogue)
                {
                    //Wait for user input
                    yield return WaitForUserInput();

                    CommandManager.Instance.StopAllProcesses();
                }
            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            //Show or hide the speaker name if there is one present
            if(line.hasSpeaker)
                HandleSpeakerLogic(line.speakerData);

            //If the dialogue box is not visible - make sure it becomes visible automatically
            if(!dialogueSystem.dialogueContainer.isVisible)
                dialogueSystem.dialogueContainer.Show();
                
            //Build dialogue
            yield return BuildLineSegments(line.dialogueData);
        }

        private void HandleSpeakerLogic(DL_SPEAKER_DATA speakerData)
        {
            bool characterMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition || speakerData.isCastingExpressions);

            Character character = CharacterManager.Instance.GetCharacter(speakerData.name, createIfDoesNotExist: characterMustBeCreated);

            if(speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing))
                character.Show();

            //Add character name to the UI
            dialogueSystem.ShowSpeakerName(speakerData.name);
            
            //Now customize the dialogue fot this character -if applicable
            DialogueSystem.Instance.ApplySpeakerDataToDialogueContainer(speakerData.name);

            //Cast position
            if(speakerData.isCastingPosition)
                character.MoveToPosition(speakerData.castPosition);
            
            //Cast expression
            if(speakerData.isCastingExpressions)
            {
                foreach(var ce in speakerData.CastExpressions)
                {
                    if(ce.layer == 2)
                        character.OnReceiveCastingExpression(character.layersCount == 1 ? 0 : 1, ce.expression);
                    else
                        character.OnReceiveCastingExpression(ce.layer, ce.expression);
                }
            }
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;

            foreach(DL_COMMAND_DATA.Command command in commands)
            {
                if(command.waitForCompletion)
                    yield return CommandManager.Instance.Execute(command.name, command.arguments);
                else
                    CommandManager.Instance.Execute(command.name, command.arguments);
            }

            yield return null;
        }

        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for(int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];
                
                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);
                
                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch(segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.B:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.EB:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.EC:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;  
                default:
                    break;
            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            //Build the dialogue
            if(!append)
                architect.Build(dialogue);
            else
                architect.Append(dialogue);

            //Wait for the dialogue to complete
            while(architect.isBuilding)
            {
                if(userPrompt)
                {
                    if(!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();

                    userPrompt = false;                  
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            dialogueSystem.prompt.Show();

            while(!userPrompt)
                yield return null;

            dialogueSystem.prompt.Hide();
            
            userPrompt = false;
        }
    }
}
