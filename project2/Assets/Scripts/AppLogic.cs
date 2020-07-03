using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppLogic : MonoBehaviour
{
    [SerializeField] GameObject pointerGO = null;
    [SerializeField] GameObject imageCanvasGO = null;
    [SerializeField] string scene = null;
    [SerializeField] private GameObject leapController = null;

    private Pointer pointer = null;
    private Image image = null;
    private LeapServiceProvider service = null;

    private Vector3 offscreenVector = new Vector3(10000, 10000, 0);

    // Start is called before the first frame update
    void Start()
    {
        service = leapController.GetComponent<LeapServiceProvider>();
        pointer = pointerGO.GetComponent<Pointer>();
        pointer.OnSelectionComplete += PointerOnSelectionComplete;
        image = imageCanvasGO.GetComponent<Image>();
    }

    private void PointerOnSelectionComplete(int buttonID, string category)
    {
        if (category == "sub")
        {
            var r = Resources.Load<Sprite>("Images/" + scene + buttonID);
            image.sprite = r;
        }
        else if (category == "main")
        {
            var s = buttonID == 0 ? "carScene" : "landscapeScene";
            SceneManager.LoadScene(s);
        }
    }


    private void Update()
    {
        if (service == null)
        {
            return;
        }

        //hand
        if (service.CurrentFrame.Hands.Count > 0)
        {
            for (int i = 0; i < service.CurrentFrame.Hands[0].Fingers.Count; i++)
            {
                //index finger
                if (service.CurrentFrame.Hands[0].Fingers[i].Type == Leap.Finger.FingerType.TYPE_INDEX)
                {
                    var hand = service.CurrentFrame.Hands[0];
                    var finger = service.CurrentFrame.Hands[0].Fingers[i];
                    var fingerVector = finger.TipPosition.ToVector3();


                    // position
                    var positionX = Map(fingerVector.x, -0.36f, 0.36f, 0, Camera.main.pixelWidth);
                    var positionY = Map(fingerVector.y, -0.025f, 0.46f, 0, Camera.main.pixelHeight);
                    var pointerPosition = new Vector3(positionX, positionY, 0);

                    pointer.transform.position = pointerPosition;

                }
            }
        }
        else
        {
            pointer.transform.position = offscreenVector;
        }
    }

    private static float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
