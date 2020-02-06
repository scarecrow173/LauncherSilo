using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace LauncherSilo.Core.Models
{
    [Serializable]
    public enum CommandExecuteType
    {
        None,
        Command,
        FileOpen
    }
    [Serializable]
    public class LauncherCommandInfo
    {
        public string UID { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public CommandExecuteType Type { get; set; } = CommandExecuteType.None;

        public bool IsImmediately { get; set; } = false;
        public string Command { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }
}
