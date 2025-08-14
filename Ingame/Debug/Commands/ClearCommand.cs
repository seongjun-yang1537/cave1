using System.Text.RegularExpressions;

public class ClearCommand : DebugCommand
{
    public ClearCommand(DebugConsole console)
        : base(console, @"^clear$")
    {
    }

    public override string Id => "clear";
    public override string Description => "Clear console output";

    protected override void Execute(Match match)
    {
        console.ClearOutput();
    }
}
