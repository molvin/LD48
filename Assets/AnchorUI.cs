using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public TickAgent Guy;
    public Transform TransformToFollow;
    public Vector2 offset;
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
        HPBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100 * Guy.CurrentHealth / Guy.ma)
        if (!TransformToFollow) return;
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(cam, objectToFollow.position) + offset;
    }
}
