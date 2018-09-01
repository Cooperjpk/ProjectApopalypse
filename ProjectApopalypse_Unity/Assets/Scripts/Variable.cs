using System.Collections;
using UnityEngine;

public class Variable : MonoBehaviour
{
    public int variable;

    bool isLocked;
    int minVar = 0;
    int maxVar = 999;

    public int Variable
    {
        get
        {
            return variable;
        }
        set
        {
            if (isLocked)
            {
                Debug.Log("This variable is locked.");
                return variable;
            }
            else
            {
                variable = value;
                if (variable > maxVar)
                {
                    variable = maxVar;
                }
                else if (variable < minVar)
                {
                    variable = minVar;
                }
            }
        }
    }
}
