using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace _YajuluSDK._Scripts.Tools
{
    public class BuiltInConsole : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI consoleText;
        
        private StringBuilder _builder;

        private string[] _colors =
        {
            "red",
            "red",
            "yellow",
            "#7A7372",
            "red"
        };
        private void Awake()
        {
            _builder = new StringBuilder();
            consoleText.SetText(_builder);
        }

        private void OnEnable()
        {
            Application.logMessageReceived += OnlogMessageReceived;
        }
        
        private void OnDisable()
        {
            Application.logMessageReceived -= OnlogMessageReceived;
        }

        private void OnlogMessageReceived(string condition, string stacktrace, LogType type)
        {
            _builder.Append($"\n-----\n<b><color={_colors[(int)type]}>{type.ToString()}</color></b>:: {condition}");
            // _builder.AppendLine($"<size=-15>{stacktrace}</size>");
            consoleText.SetText(_builder);
        }
    }
}
