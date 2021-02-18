using UnityEngine;

public enum UiAnimationTypes
{
    Move,
    Scale,
    ScaleX,
    ScaleY,
    Fade
}

public class UiTweener : MonoBehaviour
{
    public GameObject objectToAnimate;

    public UiAnimationTypes animationType;
    public LeanTweenType easeType;
    public float duration;
    public float delay;

    public bool loop;
    public bool pingpong;

    public bool startPositionOffset;
    public Vector3 from;
    public Vector3 to;

    private LTDescr _tweenObject;

    public bool shownOnEnable;
    public bool workOnDisable;

    public void OnEnable()
    {
        if (shownOnEnable)
        {
            Show();
        }
    }

    public void Show()
    {
        HandleTween();
    }

    public void HandleTween()
    {
        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        switch (animationType)
        {
            case UiAnimationTypes.Fade:
                Fade();
                break;
            case UiAnimationTypes.Move:
                MoveAbsolute();
                break;
            case UiAnimationTypes.Scale:
                Scale();
                break;
            case UiAnimationTypes.ScaleX:
                Scale();
                break;
            case UiAnimationTypes.ScaleY:
                Scale();
                break;
        }

        _tweenObject.setDelay(delay);
        _tweenObject.setEase(easeType);

        if (loop)
        {
            _tweenObject.loopCount = int.MaxValue;
        }

        if (pingpong)
        {
            _tweenObject.setLoopPingPong();
        }
    }

    public void Fade()
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        if (startPositionOffset)
        {
            objectToAnimate.GetComponent<CanvasGroup>().alpha = from.x;
        }

        _tweenObject = LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), to.x, duration);
    }

    public void MoveAbsolute()
    {
        if (startPositionOffset)
        {
            objectToAnimate.GetComponent<RectTransform>().anchoredPosition = from;
        }

        _tweenObject = LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), to, duration);
    }

    public void Scale()
    {
        if (startPositionOffset)
        {
            objectToAnimate.GetComponent<RectTransform>().localScale = from;
        }

        _tweenObject = LeanTween.scale(objectToAnimate, to, duration);
    }

    private void SwapDirection()
    {
        var temp = from;
        from = to;
        to = temp;
    }

    public void Disable()
    {
        SwapDirection();
        
        HandleTween();

        _tweenObject.setOnComplete(() =>
        {
            SwapDirection();

            gameObject.SetActive(false);
        });
        
        
    }
}
