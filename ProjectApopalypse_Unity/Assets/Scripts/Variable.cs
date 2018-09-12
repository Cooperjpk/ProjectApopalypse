using System.Collections;
using UnityEngine;

public class Variable : MonoBehaviour
{
    public int integer;

    public bool isLocked;
    public int minInt = 0;
    public int maxInt = 999;

    public int Integer
    {
        get
        {
            return integer;
        }
        set
        {
            if (isLocked)
            {
                Debug.Log("This variable is locked.");
            }
            else
            {
                integer = value;
                if (integer > maxInt)
                {
                    integer = maxInt;
                }
                else if (integer < minInt)
                {
                    integer = minInt;
                }
            }
        }
    }
}
