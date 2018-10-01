using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour {

    public string technicalName;
    public string displayName;
    public string description;
    //Lock functionality should not be driven by passive data, it should be driven by unit data.
    public bool isLocked = false;

}
