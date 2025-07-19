using TND.SGSR2;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private SGSR2_URP graphicOption;

    private void Start()
    {
        UpdateFOV(null);
        UpdateGraphic(null);
        
        Observer.AddEvent("FOV", UpdateFOV);
        Observer.AddEvent("Graphic", UpdateGraphic);
    }

    private void UpdateFOV(object data)
    {
        const int offset = 90 - 60;
        cam.fieldOfView = Mathf.RoundToInt(60 + offset * UserData.FOV);
    }

    private void UpdateGraphic(object data)
    {
        var value = UserData.GraphicOption;
        switch (value)
        {
            case 0:
                graphicOption.quality = SGSR2_Quality.Performance;
                break;
            case 1:
                graphicOption.quality = SGSR2_Quality.Balanced;
                break;
            case 2:
                graphicOption.quality = SGSR2_Quality.UltraQuality;
                break;
        }
    }
}