using GameStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestServerCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LDTimeLineBranchRequest trueTimeLine = new LDTimeLineBranchRequest();
        trueTimeLine.branchBlockIndex = 0;
        trueTimeLine.timeLine = new LDBlock[1];
        trueTimeLine.timeLine[0] = new LDBlock();
        trueTimeLine.timeLine[0].level = 10;
        trueTimeLine.timeLine[0].mods = new LDAttribute[1];
        trueTimeLine.timeLine[0].characters = new LDCharacter[1];
        trueTimeLine.timeLine[0].branches = new int[] { 1, 2, 3 };
        trueTimeLine.timeLine[0].characters[0].name = "Tjorven";
        trueTimeLine.timeLine[0].characters[0].color = 2;
        trueTimeLine.timeLine[0].characters[0].role = 2;
        trueTimeLine.timeLine[0].characters[0].attributes = new LDAttribute[1];
        trueTimeLine.timeLine[0].characters[0].timeLine = new LDInputFrame[2];
        trueTimeLine.timeLine[0].characters[0].timeLine[0] = new LDInputFrame { action = 1, cell = 2 };
        trueTimeLine.timeLine[0].characters[0].timeLine[1] = new LDInputFrame { action = 3, cell = 4 };
        Server.PushTimeLine(trueTimeLine);

        LDTimeLine data = Server.RequestTimeLine();
        Debug.Log(data.timeLine[0].level);
        Debug.Log(data.timeLine[0].branches);
        Debug.Log(data.timeLine[0].characters[0].name);
        Debug.Log(data.timeLine[0].characters[0].color);
        Debug.Log(data.timeLine[0].characters[0].role);
        Debug.Log(data.timeLine[0].characters[0].timeLine[0].action + " " + data.timeLine[0].characters[0].timeLine[0].cell);
        Debug.Log(data.timeLine[0].characters[0].timeLine[1].action + " " + data.timeLine[0].characters[0].timeLine[1].cell);
    }
}
