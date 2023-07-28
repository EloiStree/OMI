using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempOpenURL : MonoBehaviour
{

    public void OpenURl(string url) {
        Application.OpenURL(url);
    }
}
