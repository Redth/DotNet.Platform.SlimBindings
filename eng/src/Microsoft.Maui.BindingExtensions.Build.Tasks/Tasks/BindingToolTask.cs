﻿using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.Maui.BindingExtensions.Build.Tasks
{
    public abstract class BindingToolTask : ToolTask
    {
        public abstract string TaskPrefix { get; }

        protected string WorkingDirectory { get; private set; }

        StringBuilder toolOutput = new StringBuilder();

        public BindingToolTask()
        {
            WorkingDirectory = Directory.GetCurrentDirectory();
        }

        public override bool Execute()
        {
            try
            {
                bool taskResult = RunTask();
                if (!taskResult && !string.IsNullOrEmpty(toolOutput.ToString()))
                {
                    Log.LogCodedError($"{TaskPrefix}0000", toolOutput.ToString().Trim());

                }
                toolOutput.Clear();
                return taskResult;
            }
            catch (Exception ex)
            {
                Log.LogCodedError($"{TaskPrefix}0001", ex.ToString());
                return false;
            }
        }

        protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
        {
            base.LogEventsFromTextOutput(singleLine, messageImportance);
            toolOutput.AppendLine(singleLine);
        }

        public virtual bool RunTask() => base.Execute();

        protected object ProjectSpecificTaskObjectKey(object key) => (key, WorkingDirectory);
    }
}
