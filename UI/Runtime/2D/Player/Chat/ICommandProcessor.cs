using System;

namespace UI
{
    public interface ICommandProcessor
    {
        bool OnProcessCommand(string cmd);
    }
}
