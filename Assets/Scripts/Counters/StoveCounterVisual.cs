using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start()
    {
        stoveCounter.OnStoveStateChanged += StoveCounter_OnStateChanged;
    }

    /// <summary>
    /// 根据信息是否显示烤制特效
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStoveStateChangedEventArgs e)
    {
        bool showVisual = e.stoveState == StoveCounter.StoveState.Frying ||
                          e.stoveState == StoveCounter.StoveState.Fried;
        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
