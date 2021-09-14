using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour, IObserver
{
    [SerializeField] GameObject destroyFX;
    [SerializeField] GameObject chooseFX;
    [SerializeField] GameObject[] supportFX;
    [Tooltip("distance to diamond follow Z axis")]
    [SerializeField] int offsetToDiamondFollowZAxis = -1;

    private GreenDestroyDiamondFX greenDestroyDiamondFX;

    // Start is called before the first frame update
    void Start()
    {
        InputSystem.Instance.RegisterObserver (InputEvent.DeleteChoose, this);
        InputSystem.Instance.RegisterObserver (InputEvent.OnChoose, this);

        Board.Instance.RegisterObserver (BoardEvent.EarnDiamonds, this);
        Board.Instance.RegisterObserver (BoardEvent.Invert, this);

        Support.Instance.RegisterObserver (SupportEvent.SendSupport, this);

        if (!destroyFX)
        {
            Debug.LogWarning("misisng destroy effect");
        }
        else
        {
            if (!destroyFX.gameObject.TryGetComponent<GreenDestroyDiamondFX>(out greenDestroyDiamondFX))
            {
                Debug.LogWarning("missing green destroy script in destroy effect");
            }
        }

        if (!chooseFX)
        {
            Debug.LogWarning("missing choose effect");
        }

        if (supportFX.Length < 2)
        {
            Debug.LogWarning("missing support effect");
        }
    }

    private void OnDestroy() {
        if (InputSystem.IsEnable) InputSystem.Instance.RemoveAllRegister(this);
        if (Board.IsEnable) Board.Instance.RemoveAllRegister (this);
        if (Support.IsEnable) Support.Instance.RemoveAllRegister (this);
    }

    private void HandleDeleteChoose ()
    {
        if (!chooseFX) return;
        chooseFX.SetActive(false);
    }

    private void HandleOnChoose(object data)
    {
        if (!chooseFX) return;

        if (data == null)
        {
            Debug.LogWarning ("There are no choosen diamond");
            return;
        }

        try
        {
            List<Diamond> choosenDiamond = (List<Diamond>)data;
            Vector3 position = new Vector3 (choosenDiamond[0].transform.position.x, 
                choosenDiamond[0].transform.position.y, offsetToDiamondFollowZAxis);
            chooseFX.transform.position = position;
            chooseFX.SetActive(true);
        }
        catch (System.InvalidCastException)
        {
            Debug.LogWarning("faile to cast value");
            return;
        }
    }

    private void HandleEarnDiamonds(object data)
    {
        if (!greenDestroyDiamondFX) return;

        if (data == null)
        {
            Debug.LogWarning ("There are no position of diamond earned");
            return;
        }

        try
        {
            HashSet<Vector2Int> positions = (HashSet<Vector2Int>)data;

            foreach (var position in positions)
            {
                GreenDestroyFXPool.Instance.GetObject(greenDestroyDiamondFX).transform.position 
                = new Vector3(position.x, position.y, offsetToDiamondFollowZAxis);
            }
        }
        catch (System.InvalidCastException)
        {
            Debug.LogWarning("faile to cast value");
            return;
        }
    }

    private void HandleSendSupport (object data)
    {
        if (supportFX.Length != 2) return;

        if (data == null)
        {
            Debug.LogWarning("therer are no couple diamond support");
            return;
        }

        try
        {
            Diamond[] coupleSupport = (Diamond[])data;
            Vector3 position1 = new Vector3(coupleSupport[0].transform.position.x, 
                coupleSupport[0].transform.position.y, offsetToDiamondFollowZAxis);
            Vector3 position2 = new Vector3(coupleSupport[1].transform.position.x, 
                coupleSupport[1].transform.position.y, offsetToDiamondFollowZAxis);    
            
            supportFX[0].transform.position = position1;
            supportFX[1].transform.position = position2;

            supportFX[0].SetActive(true);
            supportFX[1].SetActive(true);
        }
        catch (System.InvalidCastException)
        {
            Debug.LogWarning("faile to cast value");
            return;
        }
    }

    private void HandleInvert ()
    {
        if (supportFX.Length != 2) return;

        supportFX[0].SetActive(false);
        supportFX[1].SetActive(false);
    }

    public void OnNotify(object key, object data)
    {
        
        if (key.GetType() == typeof(InputEvent))
        {
            switch ((InputEvent)key)
            {
                case InputEvent.DeleteChoose:
                    HandleDeleteChoose();
                    break;
                case InputEvent.OnChoose:
                    HandleOnChoose (data);
                    break;
                default:
                    Debug.LogWarning ("invalute input event");
                    break;
            }
        }
        else if (key.GetType() == typeof(BoardEvent))
        {
            switch ((BoardEvent)key)
            {
                case BoardEvent.EarnDiamonds:
                    HandleEarnDiamonds (data);
                    break;
                case BoardEvent.Invert:
                    HandleInvert ();
                    break;
                default:
                    Debug.LogWarning ("invalute board event");
                    break;
            }
        }
        else if (key.GetType() == typeof(SupportEvent))
        {
            switch ((SupportEvent)key)
            {
                case SupportEvent.SendSupport:
                    HandleSendSupport (data);
                    break;
                default:
                    Debug.LogWarning ("invalute board event");
                    break;
            }
        }
    }
}
