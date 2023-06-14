using UnityEngine;
using UnityEngine.UI;

namespace Dzajna {
public class FPBar : MonoBehaviour {
    
    private Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    public void SetMaxFP(int _maxFP) {
        slider.maxValue = _maxFP;
        slider.value = _maxFP;
    }

    public void SetCurrentFP(float _currentFP) {
        slider.value = _currentFP;
    }
}
}