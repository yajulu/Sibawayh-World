using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _YajuluSDK._Scripts.UI
{
    /// <summary>
    /// A standard toggle that has an on / off state.
    /// </summary>
    /// <remarks>
    /// The toggle component is a Selectable that controls a child graphic which displays the on / off state.
    /// When a toggle event occurs a callback is sent to any registered listeners of UI.Toggle._onValueChanged.
    /// </remarks>
    [AddComponentMenu("UI/Tab", 30)]
    [RequireComponent(typeof(RectTransform))]
    [ShowOdinSerializedPropertiesInInspector]
    public class UITab : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement, ISerializationCallbackReceiver, ISupportsPrefabSerialization
    {
        /// <summary>
        /// Display settings for when a toggle is activated or deactivated.
        /// </summary>
        public enum TabTransition
        {
            /// <summary>
            /// Show / hide the toggle instantly
            /// </summary>
            None,

            /// <summary>
            /// Fade the toggle in / out smoothly.
            /// </summary>
            Fade
        }

        /// <summary>
        /// UnityEvent callback for when a toggle is toggled.
        /// </summary>
        [Serializable]
        public delegate void UITabEvent(bool val);

        /// <summary>
        /// Transition mode for the toggle.
        /// </summary>
        [TitleGroup("Properties")] public TabTransition tabTransition = TabTransition.Fade;
        [SerializeField, TitleGroup("Properties")] private float m_animationDuration;
        

        /// <summary>
        /// Graphic the toggle should be working with.
        /// </summary>
        [TitleGroup("Refs")] public Graphic graphic;

        [SerializeField, TitleGroup("Refs")] private TextMeshProUGUI m_TabText;

        [SerializeField, TitleGroup("Refs")]
        private UITabGroup m_Group;

        /// <summary>
        /// Group the toggle belongs to.
        /// </summary>
        public UITabGroup group
        {
            get { return m_Group; }
            set
            {
                SetTabGroup(value, true);
                PlayEffect(true);
            }
        }

        /// <summary>
        /// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// //Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
        /// //Set your own Text in the Inspector window
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     Toggle m_Toggle;
        ///     public Text m_Text;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the Toggle GameObject
        ///         m_Toggle = GetComponent<Toggle>();
        ///         //Add listener for when the state of the Toggle changes, to take action
        ///         m_Toggle.onValueChanged.AddListener(delegate {
        ///                 ToggleValueChanged(m_Toggle);
        ///             });
        ///
        ///         //Initialise the Text to say the first state of the Toggle
        ///         m_Text.text = "First Value : " + m_Toggle.isOn;
        ///     }
        ///
        ///     //Output the new state of the Toggle into Text
        ///     void ToggleValueChanged(Toggle change)
        ///     {
        ///         m_Text.text =  "New Value : " + m_Toggle.isOn;
        ///     }
        /// }
        /// ]]>
        ///</code>
        /// </example>
        [HideInInspector] public UITabEvent OnValueChanged;

        // Whether the toggle is on
        [Tooltip("Is the toggle currently on or off?")]
        [SerializeField, TitleGroup("Properties")]
        private bool m_IsOn;
        
        [SerializeField, TitleGroup("Properties"), OnValueChanged(nameof(UpdateTabText))] private string m_TabName;

        [SerializeField,TitleGroup("Properties"),  InlineEditor]
        private Image m_TabIcon;

        protected UITab()
        {}

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

#endif // if UNITY_EDITOR

        public virtual void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
            {
                OnValueChanged?.Invoke(m_IsOn);
            }
#endif
        }

        public virtual void LayoutComplete()
        {}

        public virtual void GraphicUpdateComplete()
        {}

        protected override void OnDestroy()
        {
            if (m_Group != null)
            {
                m_Group.EnsureValidState();
                m_Group.UnregisterToggle(this, true);
            }
            
            base.OnDestroy();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetTabGroup(m_Group, false);
            PlayEffect(true);
        }

        protected override void OnDisable()
        {
            SetTabGroup(null, false);
            base.OnDisable();
        }
        
        protected override void OnDidApplyAnimationProperties()
        {
            // Check if isOn has been changed by the animation.
            // Unfortunately there is no way to check if we don�t have a graphic.
            if (graphic != null)
            {
                bool oldValue = !Mathf.Approximately(graphic.canvasRenderer.GetColor().a, 0);
                if (m_IsOn != oldValue)
                {
                    m_IsOn = oldValue;
                    Set(!oldValue);
                }
            }

            base.OnDidApplyAnimationProperties();
        }

        public void SetTabGroup(UITabGroup newGroup, RectTransform tabContentTransform)
        {
            SetTabGroup(newGroup, true, tabContentTransform);
        }

        private void SetTabGroup(UITabGroup newGroup, bool setMemberValue, RectTransform tabContentTransform = null)
        {
            // Sometimes IsActive returns false in OnDisable so don't check for it.
            // Rather remove the toggle too often than too little.
            if (m_Group != null && newGroup != m_Group)
            {
                m_Group.UnregisterToggle(this);
            }
            
            // At runtime the group variable should be set but not when calling this method from OnEnable or OnDisable.
            // That's why we use the setMemberValue parameter.
            if (setMemberValue)
                m_Group = newGroup;

            // Only register to the new group if this Toggle is active.
            if (newGroup != null && IsActive())
                newGroup.RegisterToggle(this, tabContentTransform);

            // If we are in a new group, and this toggle is on, notify group.
            // Note: Don't refer to m_Group here as it's not guaranteed to have been set.
            if (newGroup != null && isOn && IsActive())
                newGroup.NotifyToggleOn(this);
        }
        
        /// <summary>
        /// Whether the toggle is currently active.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// /Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
        /// //Set your own Text in the Inspector window
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     Toggle m_Toggle;
        ///     public Text m_Text;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the Toggle GameObject
        ///         m_Toggle = GetComponent<Toggle>();
        ///         //Add listener for when the state of the Toggle changes, and output the state
        ///         m_Toggle.onValueChanged.AddListener(delegate {
        ///                 ToggleValueChanged(m_Toggle);
        ///             });
        ///
        ///         //Initialize the Text to say whether the Toggle is in a positive or negative state
        ///         m_Text.text = "Toggle is : " + m_Toggle.isOn;
        ///     }
        ///
        ///     //Output the new state of the Toggle into Text when the user uses the Toggle
        ///     void ToggleValueChanged(Toggle change)
        ///     {
        ///         m_Text.text =  "Toggle is : " + m_Toggle.isOn;
        ///     }
        /// }
        /// ]]>
        ///</code>
        /// </example>

        public bool isOn
        {
            get { return m_IsOn; }

            set
            {
                Set(value);
            }
        }

        public string TabName
        {
            get => m_TabName;
            set
            {
                m_TabName = value;
                m_TabText.text = m_TabName;
            }
        }

        private void UpdateTabText(string value)
        {
            TabName = value;
        }

        
        /// <summary>
        /// Set isOn without invoking onValueChanged callback.
        /// </summary>
        /// <param name="value">New Value for isOn.</param>
        public void SetIsOnWithoutNotify(bool value)
        {
            Set(value, false);
        }

        void Set(bool value, bool sendCallback = true)
        {
            if (m_IsOn == value)
                return;

            // if we are in a group and set to true, do group logic
            m_IsOn = value;
            if (m_Group != null && m_Group.isActiveAndEnabled && IsActive())
            {
                if (m_IsOn || (!m_Group.AnyTogglesOn() && !m_Group.allowSwitchOff))
                {
                    m_IsOn = true;
                    m_Group.NotifyToggleOn(this, sendCallback);
                }
            }

            // Always send event when toggle is clicked, even if value didn't change
            // due to already active toggle in a toggle group being clicked.
            // Controls like Dropdown rely on this.
            // It's up to the user to ignore a selection being set to the same value it already was, if desired.
            PlayEffect(tabTransition == TabTransition.None);
            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("Tab.value", this);
                OnValueChanged?.Invoke(m_IsOn);
            }
        }

        /// <summary>
        /// Play the appropriate effect.
        /// </summary>
        private void PlayEffect(bool instant)
        {
            if (graphic == null)
                return;
            var value = m_IsOn ? 1f : 0f;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                graphic.canvasRenderer.SetAlpha(value);
                m_TabText.canvasRenderer.SetAlpha(value);
                graphic.transform.localScale = Vector3.one;
            }
            else
#endif
            {
                
                var duration = instant ? 0f : m_animationDuration;
                graphic.CrossFadeAlpha(value,duration , true);
                m_TabText.DOFade(value, duration)
                    .SetEase(Ease.Linear);
                graphic.transform.DOScale(value, duration)
                    .SetEase(Ease.OutBack);
                // transform.DOScaleX(m_IsOn ? 2f : 1f, instant ? 0f : 0.2f);
            }

        }

        /// <summary>
        /// Assume the correct visual state.
        /// </summary>
        protected override void Start()
        {
            PlayEffect(true);
        }

        private void InternalToggle()
        {
            if (!IsActive())
                return;

            isOn = !isOn;
        }

        /// <summary>
        /// React to clicks.
        /// </summary>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            InternalToggle();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            InternalToggle();
        }

        [SerializeField, HideInInspector]
        private SerializationData serializationData;

        SerializationData ISupportsPrefabSerialization.SerializationData { get { return this.serializationData; } set { this.serializationData = value; } }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
        }
    }
}
