using System.Text.RegularExpressions;

public abstract class DebugCommand
{
    protected readonly DebugConsole console;
    private readonly Regex _pattern;

    public abstract string Id { get; }
    public abstract string Description { get; }
    public string Pattern => _pattern.ToString();

    protected DebugCommand(DebugConsole console, string pattern)
    {
        this.console = console;
        _pattern = new Regex(pattern, RegexOptions.IgnoreCase);
    }

    public bool TryExecute(string input)
    {
        Match match = _pattern.Match(input);
        if (!match.Success)
            return false;

        Execute(match);
        return true;
    }

    protected abstract void Execute(Match match);
}
