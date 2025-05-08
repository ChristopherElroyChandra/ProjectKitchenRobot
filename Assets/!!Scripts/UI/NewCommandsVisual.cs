using UnityEngine;

public class NewCommandsVisual : MonoBehaviour
{
    private float _initialYPos;
    private float _hiddenYPos;

    [SerializeField] float _animTime;

    [SerializeField] RectTransform _visualPanel;

    [SerializeField] VoidEventChannelSO _programStartedChannel;
    [SerializeField] VoidEventChannelSO _programStoppedChannel;

    private void Awake()
    {
        _initialYPos = transform.localPosition.y;
        _hiddenYPos = _initialYPos + GetComponent<RectTransform>().rect.height;
    }

    private void OnEnable()
    {
        _programStartedChannel.OnEventRaised += OnProgramStarted;
        _programStoppedChannel.OnEventRaised += OnProgramStopped;
    }

    private void OnDisable()
    {
        _programStartedChannel.OnEventRaised -= OnProgramStarted;
        _programStoppedChannel.OnEventRaised -= OnProgramStopped;
    }

    private void OnProgramStarted()
    {
        HidePanel();
    }

    private void OnProgramStopped()
    {
        ShowPanel();
    }

    private void HidePanel()
    {
        if (LeanTween.isTweening(this.gameObject) || LeanTween.isTweening(_visualPanel.gameObject))
        {
            LeanTween.cancel(this.gameObject);
            LeanTween.cancel(_visualPanel.gameObject);
        }

        LeanTween.moveLocalY(this.gameObject, _hiddenYPos, _animTime);
        LeanTween.moveLocalY(_visualPanel.gameObject, _hiddenYPos, _animTime);
    }

    private void ShowPanel()
    {
        if (LeanTween.isTweening(this.gameObject) || LeanTween.isTweening(_visualPanel.gameObject))
        {
            LeanTween.cancel(this.gameObject);
            LeanTween.cancel(_visualPanel.gameObject);
        }

        LeanTween.moveLocalY(this.gameObject, _initialYPos, _animTime);
        LeanTween.moveLocalY(_visualPanel.gameObject, _initialYPos, _animTime);
    }
}
