using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistersController : MonoBehaviour
{
    public Stack<string> stack = new Stack<string>();

    // Registers values
    public Dictionary<string, int> registers = new Dictionary<string, int>(){
        { "ax", 0 },
        { "bx", 0 },
        { "cx", 0 },
        { "dx", 0 }
    };

    //public int eaxVal = 0;
    //public int ebxVal = 0;
    //public int ecxVal = 0;
    //public int edxVal = 0;

    // Registers text values
    [SerializeField]
    public Text eaxText;
    [SerializeField]
    public Text ebxText;
    [SerializeField]
    public Text ecxText;
    [SerializeField]
    public Text edxText;

    // Console
    [SerializeField]
    public Text consoleText;

    // Console input field
    [SerializeField]
    public InputField consoleInput;

    // Stack Text
    [SerializeField]
    public Text stackText;

    private void Start()
    {
        stackText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && consoleInput.text != "")
        {
            GetCommand();
        }
    }

    // Gets command from input and executes if it exists or displays error message
    public void GetCommand()
    {
        consoleInput.text = consoleInput.text.ToLower();

        DisplayLog("$ " + consoleInput.text);

        string[] newCommand = consoleInput.text.Split(' ');

        switch (newCommand[0])
        {
            case "mov":
                // Check if the second parameter is a number
                if (!registers.ContainsKey(newCommand[2]))
                {
                    ExecuteMovCommandWithANumber(newCommand[1], newCommand[2]);
                }
                else
                {
                    if (!registers.ContainsKey(newCommand[1]))
                    {
                        DisplayLog("-Taki rejestr nie istnieje !");
                    }
                    else
                    {
                        ExecuteMovCommand(newCommand[2], newCommand[1]);
                    }
                }
                break;
            case "push":
                // Check if the second parameter is a number
                if (!registers.ContainsKey(newCommand[1]))
                {
                    ExecutePushCommandWithANumber(newCommand[1]);
                }
                else
                {
                    ExecutePushCommand(newCommand[1]);
                }
                break;
            case "pop":
                // Check if the choosen register exists
                if (!registers.ContainsKey(newCommand[1]))
                {
                    DisplayLog("-Nieprawidłowe polecenie !");
                }
                else
                {
                    ExecutePopCommand(newCommand[1]);
                }
                break;
            default:
                DisplayLog("-Nieprawidłowe polecenie !");
                break;
        }

        consoleInput.text = "";
        consoleInput.Select();
        consoleInput.ActivateInputField();
        consoleInput.Select();
    }

    // Display log to the console
    public void DisplayLog(string logText)
    {
        consoleText.text += logText + "\n";
    }

    // Update register values and show them to the user
    public void UpdateAllRegisters()
    {
        eaxText.text = registers["ax"].ToString();
        ebxText.text = registers["bx"].ToString();
        ecxText.text = registers["cx"].ToString();
        edxText.text = registers["dx"].ToString();

        DisplayLog("-Wykonano");
    }

    // COMMANDS EXECUTION

    // Execute MOV command and update register values
    public void ExecuteMovCommand(string from, string to)
    {
        registers[to] = registers[from];

        UpdateAllRegisters();
    }

    // Execute MOV command with a number and update register values
    public void ExecuteMovCommandWithANumber(string to, string number)
    {
        registers[to] = int.Parse(number);

        UpdateAllRegisters();
    }

    // Add value from a register to stack
    public void ExecutePushCommand(string val)
    {
        string valToPush = registers[val].ToString();
        stack.Push(valToPush);

        stackText.text = valToPush + "\n" + stackText.text;

        DisplayLog("-Wykonano");
    }

    // Add a number value to stack
    public void ExecutePushCommandWithANumber(string val)
    {
        stack.Push(val);

        stackText.text = val + "\n" + stackText.text;

        DisplayLog("-Wykonano");
    }

    // Execute pop from stack to a register
    public void ExecutePopCommand(string to)
    {
        if (stack.Count <= 0)
        {
            DisplayLog("-Błąd, stos jest pusty !");
            return;
        }
        string valFromStack = stack.Pop();
        registers[to] = int.Parse(valFromStack);

        stackText.text = stackText.text.Remove(0, valFromStack.Length + 1);

        UpdateAllRegisters();
    }
}
