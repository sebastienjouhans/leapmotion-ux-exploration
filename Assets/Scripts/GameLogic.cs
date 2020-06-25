using Leap.Unity;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private string interactiveTag = "Interactive";
    [SerializeField] private GameObject pointerCircle = null;
    [SerializeField] private GameObject loaderCircle = null;
    [SerializeField] private GameObject leapController = null;
    [SerializeField] private GameObject imageViewerGO = null;
    [SerializeField] private Camera camera = null;


    private UnityEngine.UI.Image imageViewer = null;
    private LeapServiceProvider service = null;
    private Pointer pointer = null;
    private Loader loader = null;
    private int buttonID = int.MaxValue;

    private Vector3 offscreenVector = new Vector3(10000, 10000, 0);

    // Start is called before the first frame update
    private void Start()
    {
        service = leapController.GetComponent<LeapServiceProvider>();

        pointer = pointerCircle.GetComponent<Pointer>();
        pointer.OnCollisionEnter += PointerOnCollisionEnter;
        pointer.OnCollisionExit += PointerOnCollisionExit;
        pointer.transform.position = offscreenVector;

        loader = loaderCircle.GetComponent<Loader>();
        loader.OnLoadingComplete += LoaderOnLoadingComplete;

        imageViewer = imageViewerGO.GetComponent<Image>();
    }


    private void LoaderOnLoadingComplete()
    {
        var r = Resources.Load<Sprite>("Images/landscape"+buttonID);
        imageViewer.sprite = r;
    }

    private void PointerOnCollisionExit(Collider2D collision)
    {
        if (collision.GetComponent<UIButton>() == null)
        {
            return;
        }

        if (collision.CompareTag(interactiveTag))
        {
            var renderer = collision.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                buttonID = int.MaxValue;
                renderer.color = Color.white;
                loader.StopLoading();
            }
        }
    }

    private void PointerOnCollisionEnter(Collider2D collision)
    {
        if(collision.GetComponent<UIButton>() == null)
        {
            return;
        }

        if (collision.CompareTag(interactiveTag))
        {
            var renderer = collision.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                buttonID = collision.GetComponent<UIButton>().ID;
                loader.enabled = true;
                renderer.color = Color.red;
                loader.StartLoading();
            }
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

                    RaycastHit2D hit = Physics2D.Raycast(finger.TipPosition.ToVector3(), -Vector2.up);
                    if (hit.collider!=null)
                    {
                        // scale in relation of the hand position
                        var scale = Map(hand.PalmPosition.ToVector3().z, -10.24f, -9.90f, 0.02f, 0.05f);
                        if (scale < 0.01)
                        {
                            scale = 0.01f;
                        }

                        pointer.transform.localScale = new Vector3(scale, scale, 0);


                        // position
                        var positionX = Map(hit.point.x, -0.36f, 0.36f, -10, 10);
                        var positionY = Map(hit.point.y, -0.025f, 0.46f, -4.5f, 4.5f);
                        var pointerPosition = new Vector3(positionX, positionY, 0);

                        pointer.transform.position = pointerPosition;
                        loaderCircle.transform.position = pointerPosition;
                    }


                }
            }
        }
        else
        {
            pointer.transform.position = offscreenVector;
            loader.StopLoading();
        }
    }

    private static float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return  (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
