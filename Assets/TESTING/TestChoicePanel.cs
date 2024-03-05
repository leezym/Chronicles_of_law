using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChoicePanel : MonoBehaviour
{
    public ChoicePanel panel;
    // Start is called before the first frame update
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            panel = ChoicePanel.Instance;
            
            string[] choices = new string[]
            {
                "Witness? Is that camera on?",
                "Oh, nah!",
                "I didn`t see nothin'!",
                "Matta Fact- I'm blind in my left eye and 43% blind in my right eye."
            };

            panel.Show("Did you wiyness anything strange?", choices);
        }
    }
}
