using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public enum TransitionState
    {
        None,
        Transitioning,
        Complete
    }

    public TransitionState CurrentState { get; private set; }

    public delegate void TransitionEventHandler();

    public event TransitionEventHandler OnTransitionStart;
    public event TransitionEventHandler OnTransitionComplete;

    public float transitionTime;
    [SerializeField] private Animator anim;

    private static TransitionManager instance;

    public static TransitionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TransitionManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(TransitionManager).Name;
                    instance = obj.AddComponent<TransitionManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartTransition()
    {
        if (CurrentState == TransitionState.Transitioning)
            return;

        anim.SetBool("Transitioning", true);

        CurrentState = TransitionState.Transitioning;
        OnTransitionStart?.Invoke();
        StartCoroutine(Transition());
    }

    private void CompleteTransition() 
    {
        if (CurrentState != TransitionState.Transitioning)
            return;

        anim.SetBool("Transitioning", false);

        CurrentState = TransitionState.Complete;
        OnTransitionComplete?.Invoke();
    }

    private void ResetTransition()
    {
        CurrentState = TransitionState.None;
    }

    private IEnumerator Transition()
    {
        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        CompleteTransition();
        ResetTransition();
    }

    public bool IsTransitioning()
    {
        return CurrentState == TransitionState.Transitioning;
    }
}
