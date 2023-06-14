using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dzajna {
public class SoulCountUI : MonoBehaviour {
   [SerializeField] private TextMeshProUGUI soulCountText;

   public void SetSoulCountText(int _soulCount) {
      soulCountText.text = _soulCount.ToString();
   }
}

}
