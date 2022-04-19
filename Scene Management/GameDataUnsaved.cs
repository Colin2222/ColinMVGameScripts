using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataUnsaved : MonoBehaviour
{
    public Dictionary<string,bool> gameData = new Dictionary<string,bool>(){
        // main questlines
        {"Main_FirstQuest_Started",false},
        {"Main_FirstQuest_Completed",false},
        {"test1",true},

        // BLUECAVE DATA
        {"Bluecave_0_RockCracked", false},

        {"Bluecave_1_BrownBag_Grabbed", false},
        {"Bluecave_1_Rock_Cracked", true},
        {"Bluecave_1_BlockTest_Active", true}
    };
}

