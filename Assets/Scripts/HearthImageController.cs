using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HearthImageController : MonoBehaviour
{
    [SerializeField] private Image hearthImage;

    public void SetHearthImageActive(bool isActive)
    {
        hearthImage.enabled = isActive;
    }
}
