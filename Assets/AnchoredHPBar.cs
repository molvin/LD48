using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchoredHPBar : MonoBehaviour
{
    public TickAgent Guy;
    public Vector3 offset;
    public Image HPBar;
    public TMPro.TextMeshProUGUI Name;

    private RectTransform rectTransform = null;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (!Guy) return;
        HPBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100f * ((float)Guy.CurrentHealth / Guy.MaxHealth));
        Name.text = (Guy is PlayableAgent) ? ((PlayableAgent)Guy).Name : "";
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(cam, Guy.transform.position + offset);
    }
}
