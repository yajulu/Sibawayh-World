using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using static UIPopupController;

namespace _YajuluSDK._Scripts.UI
{
    public class PopUpManager : Singleton<PopUpManager>
    {
        [SerializeField] private List<UIPopupController> popupContollerList;
        //[SerializeField] private ListPool<UIPopupController> popupPool;
        [SerializeField, ReadOnly] private List<UIPopupController> availablePopup;

        [SerializeField] private Queue<PopupRequest> popupRequestQueue;        

        private PopupRequest _dummyRequest;

        private void Start()
        {
            //popupPool = new ObjectPool<UIPopupController>(() =>
            //{
            //    return pop
            //});

            foreach (var controller in popupContollerList)
            {
                controller.OnPopUpOpened += Controller_OnPopUpOpened;
                controller.OnPopUpClosed += Controller_OnPopUpClosed;
            }
        }

        private void OnDestroy()
        {
            foreach (var controller in popupContollerList)
            {
                controller.OnPopUpOpened -= Controller_OnPopUpOpened;
                controller.OnPopUpClosed -= Controller_OnPopUpClosed;
            }
        }

        private void Controller_OnPopUpClosed(UIPopupController obj)
        {
            availablePopup.Add(obj);
            CheckQueue();
        }

        private void Controller_OnPopUpOpened(UIPopupController obj)
        {
            availablePopup.Remove(obj);
            //TODO: Handler
        }

        private void CheckQueue()
        {
            if (popupRequestQueue.TryDequeue(out _dummyRequest))
            {
                PerformPopupRequest(_dummyRequest);
            }
        }

        private void PerformPopupRequest(PopupRequest request)
        {
            availablePopup[0].ShowPopup(request);
        }

        public void RequestPopUp(PopupRequest request)
        {
            Debug.Log(availablePopup);
            if (availablePopup.Count > 0)
            {
                PerformPopupRequest(request);
            }
            else
            {
                popupRequestQueue.Enqueue(request);
            }
        }


        //public void RequestConfirmPurchasePopup()
        //{
        //    var request = new PopupRequest
        //    {
        //        //ButtonsConfig = new List<PopupButtonAction>
        //        //{
        //        //    //new PopupButtonAction
        //        //    //{
        //        //    //    buttonAction
        //        //    //}
        //        //}
        //    };
        //}

        [Button]
        private void SetRefs()
        {
            popupContollerList = null;
            availablePopup = null;
            popupContollerList = transform.GetComponentsInChildren<UIPopupController>(true).ToList();
            availablePopup = transform.GetComponentsInChildren<UIPopupController>(true).ToList();
        }
    }
}
