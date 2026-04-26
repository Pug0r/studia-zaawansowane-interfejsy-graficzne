using System;
using System.Collections.Generic;
using System.Text;

namespace Lab06_gamelib
{

    public record LogEntry(string message);

    public class GameLog
    {
        private List<LogEntry> logEntries = new List<LogEntry>();

        public void Log(string message)
        {
            logEntries.Add(new LogEntry(message));
        }

        public List<LogEntry> GetLogs()
        {
            return logEntries;
        }
    }
}
