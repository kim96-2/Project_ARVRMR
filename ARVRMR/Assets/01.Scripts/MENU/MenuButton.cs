using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float highlightScale = 1.3f;

    [SerializeField] float animSpeed = 10f;

    bool isIn;

    RectTransform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<RectTransform>();

        isIn = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //tr.localScale = Vector3.Lerp(tr.localScale, Vector3.one * highlightScale, animSpeed * Time.deltaTime);
        isIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //tr.localScale = Vector3.Lerp(tr.localScale, Vector3.one, animSpeed * Time.deltaTime);
        isIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIn)
        {
            tr.localScale = Vector3.Lerp(tr.localScale, Vector3.one * highlightScale, animSpeed * Time.deltaTime);
        }
        else
        {
            tr.localScale = Vector3.Lerp(tr.localScale, Vector3.one, animSpeed * Time.deltaTime);
        }
    }
}
