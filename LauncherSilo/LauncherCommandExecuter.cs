using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Misc;

using LauncherSilo.Core.Models;
using LauncherSilo.Models;

namespace LauncherSilo
{
    public class LauncherCommandExecuter
    {
        private string ExeExtension = ".exe";

        public void Execute(LauncherCommandInfo CommandInfo)
        {
            string[] Commands = ParseCommandList(CommandInfo);
            int CommnadCount = Commands.Count();
            foreach (var Command in Commands.Select((command, index) => new { command, index }))
            {
                bool IsImmediately = Command.index != CommnadCount - 1 || (App.AppConfig.IsImmediately && CommandInfo.IsImmediately);
                try
                {
                    if (App.AppConfig.IsCopyToClipboard || !IsImmediately)
                    {
                        System.Windows.Forms.Clipboard.Clear();
                        System.Threading.Thread.Sleep(100);
                        System.Windows.Forms.Clipboard.SetText(Command.command);
                    }
                }
                catch (Exception ex)
                {
                    LogStatics.Debug(ex.ToString());
                }
                switch (CommandInfo.Type)
                {
                    case CommandExecuteType.None:
                        break;
                    case CommandExecuteType.Command:
                        foreach (var CommandModule in App.PluginManagerInstance.CommandExecuteModules)
                        {
                            if (CommandModule.CanExecute(Command.command, IsImmediately))
                            {
                                CommandModule.Execute(Command.command, IsImmediately);
                            }
                        }
                        break;
                    case CommandExecuteType.FileOpen:
                        ExecuteFileOpen(Command.command);
                        break;
                }

            }
        }

        public string[] ParseCommandList(LauncherCommandInfo CommandInfo)
        {
            string[] Commands = CommandInfo.Command.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
            int CommandCount = Commands.Count();
            List<string> FormattedCommands = new List<string>();
            bool IsArg = false;
            foreach (string RawCommand in Commands)
            {
                if (IsArg)
                {
                    FormattedCommands[FormattedCommands.Count - 1] += " " + RawCommand.TrimStart();
                }
                else
                {
                    FormattedCommands.Add(RawCommand.TrimStart());
                }
                IsArg = System.IO.Path.HasExtension(RawCommand) && System.IO.Path.GetExtension(RawCommand).Contains(ExeExtension);
            }
            return FormattedCommands.ToArray();
        }
        public void ParseFileCommand(string Command, out string FilePath, out string Args)
        {
            FilePath = string.Empty;
            Args = string.Empty;
            if (Command.Contains(ExeExtension))
            {
                int Index = Command.IndexOf(ExeExtension) + ExeExtension.Length;
                Args = Command.Substring(Index);
                FilePath = Command.Remove(Index);
            }
            else
            {
                FilePath = Command;
            }
            FilePath = System.IO.Path.GetFullPath(FilePath);
        }

        private void ExecuteFileOpen(string Command)
        {
            ParseFileCommand(Command, out string FilePath, out string Args);
            if (!System.IO.File.Exists(FilePath) && !System.IO.Directory.Exists(FilePath))
            {
                return;
            }
            Process.Start(FilePath, Args);
        }

    }

}
