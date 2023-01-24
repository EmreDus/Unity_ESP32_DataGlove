using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour , IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Pause = !Pause;
    }

    [SerializeField] private Image uiFill;
    [SerializeField] private Text uiText;
    [SerializeField] private Text uiText2;
    [SerializeField] private GameObject uiCanvas;

    public int Duration;

    private int remainingDuration;

    public string Open = "Open Your Hand";
    public string Close = "Close Your Hand";

    public bool Pause;

    private void Start()
    {
        Being(Duration);
    }

    private void Being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(remainingDuration >= 0)
        {
            if (!Pause)
            {
                uiText.text = $"{remainingDuration % 10}";
                uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
                remainingDuration--;
                yield return new WaitForSeconds(1f);
            }

            while(remainingDuration >= 5)
            {
                uiText2.text = Open;
            }
            while(remainingDuration < 5)
            {
                uiText2.text = Close;
            }
            yield return null;
        }
        OnEnd();
    }

    private void OnEnd()
    {
        //End Time , if want Do something
        uiCanvas.SetActive(false);
        remainingDuration = 10;
        print("End");
    }
}
