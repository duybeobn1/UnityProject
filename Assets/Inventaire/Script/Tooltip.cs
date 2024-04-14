using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Text header_field;
    [SerializeField]
    private Text content_field;
    [SerializeField]
    private LayoutElement layoutElement;
    [SerializeField]
    private int max_character;
    [SerializeField]
    private RectTransform  recTransform;

    public void SetText(string content, string header ="")
    {
        if(header == "")
        {
            header_field.gameObject.SetActive(false);
        }else{
            header_field.gameObject.SetActive(true);
            header_field.text = header;
        }
        content_field.text = content;
        int header_length = header_field.text.Length;
        int content_length = content_field.text.Length;
        layoutElement.enabled = (header_length > max_character || content_length > max_character) ? true : false;
    }
    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;

        recTransform.pivot = new Vector2(pivotX,pivotY);
        transform.position = mousePosition;

    }
}
