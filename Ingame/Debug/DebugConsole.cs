using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole Instance { get; private set; }

    [Header("UI References")]
    public GameObject consoleRoot;
    public TMP_InputField inputField;
    public TMP_Text outputText;

    private readonly List<DebugCommand> _commands = new();

    public IReadOnlyList<DebugCommand> Commands => _commands;

    private bool _visible;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        RegisterDefaultCommands();
        consoleRoot.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Toggle();
        }

        if (_visible && Input.GetKeyDown(KeyCode.Return))
        {
            ProcessInput(inputField.text);
            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }

        if (_visible && Input.GetKeyDown(KeyCode.Tab))
        {
            AutoCompleteInput();
            inputField.caretPosition = inputField.text.Length;
        }
    }

    public void Toggle()
    {
        _visible = !_visible;
        consoleRoot.SetActive(_visible);
        if (_visible)
            inputField.ActivateInputField();
    }

    public void RegisterCommand(DebugCommand command)
    {
        _commands.RemoveAll(c => c.Id == command.Id);
        _commands.Add(command);
    }

    private void RegisterDefaultCommands()
    {
        RegisterCommand(new HelpCommand(this));
        RegisterCommand(new ClearCommand(this));
    }

    private void ProcessInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return;

        Log($"> {input}");

        foreach (var cmd in _commands)
        {
            try
            {
                if (cmd.TryExecute(input))
                    return;
            }
            catch (Exception e)
            {
                Log($"Error: {e.Message}");
                return;
            }
        }

        Log($"Unknown command: {input.Split(' ')[0]}");
    }

    public void Log(string message)
    {
        outputText.text += message + "\n";
    }

    public void ClearOutput()
    {
        outputText.text = string.Empty;
    }

    private void AutoCompleteInput()
    {
        string current = inputField.text;
        if (string.IsNullOrWhiteSpace(current))
            return;

        List<string> matches = new List<string>();
        foreach (var cmd in _commands)
        {
            if (cmd.Id.StartsWith(current, StringComparison.OrdinalIgnoreCase))
                matches.Add(cmd.Id);
        }

        if (matches.Count == 0)
            return;

        if (matches.Count == 1)
        {
            inputField.text = matches[0] + " ";
        }
        else
        {
            Log(string.Join(", ", matches));
        }
    }
}
