using System.Text;
using System.Text.RegularExpressions;

public class HelpCommand : DebugCommand
{
    public HelpCommand(DebugConsole console)
        : base(console, @"^help$")
    {
    }

    public override string Id => "help";
    public override string Description => "List all available commands";

    protected override void Execute(Match match)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var cmd in console.Commands)
        {
            sb.AppendLine($"- {cmd.Id}: {cmd.Description}");
        }
        console.Log(sb.ToString().TrimEnd());
    }
}
