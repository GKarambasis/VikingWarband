using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAnimator : MonoBehaviour
{
    public GameObject VikingPanel, GodPanel;
    private Animator vikinganimator, godanimator;
    private GameObject Settings;

    public void Start()
    {
        if (VikingPanel != null)
        {
            vikinganimator = VikingPanel.GetComponent<Animator>();
        }

        if (GodPanel != null)
        {
            godanimator = GodPanel.GetComponent<Animator>();
        }

    }
    public void OpenVikingPanel()
    {
            if (vikinganimator != null)
            {
                bool isOpen = vikinganimator.GetBool("open");

                vikinganimator.SetBool("open", !isOpen);

                bool isGodOpen = godanimator.GetBool("open");

                if (isGodOpen)
                {
                    godanimator.SetBool("open", false);
                }
            }
    }

    public void OpenGodPanel()
    {
            if (godanimator != null)
            {
                bool isOpen = godanimator.GetBool("open");

                godanimator.SetBool("open", !isOpen);

                bool isVikingOpen = vikinganimator.GetBool("open");

                if (isVikingOpen)
                {
                    vikinganimator.SetBool("open", false);
                }

            }
    }
}
