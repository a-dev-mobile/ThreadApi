using Serilog.Events;
using Serilog.Formatting;
using System.IO;
using System.Text;
using Newtonsoft.Json; // Ensure Newtonsoft.Json is included in your project dependencies

public class SerilogJsonFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        var sb = new StringBuilder();
        sb.Append("{");

        // Write the timestamp in the desired format
        sb.Append("\"time\":\"");
        sb.Append(logEvent.Timestamp.ToString("yyyy-MM-ddTHH:mm:sszzz"));
        sb.Append("\",");

        // Write the level
        sb.Append("\"level\":\"");
        sb.Append(logEvent.Level.ToString());
        sb.Append("\",");

        // Write the message, ensuring proper escaping of characters
        sb.Append("\"message\":\"");
        sb.Append(EscapeJsonString(logEvent.RenderMessage()));
        sb.Append("\",");

        // Write exception if exists, ensuring proper escaping
        if (logEvent.Exception != null)
        {
            sb.Append("\"exception\":\"");
            sb.Append(EscapeJsonString(logEvent.Exception.ToString()));
            sb.Append("\",");
        }

        // Serialize and write additional properties, ensuring complex objects are handled properly
        foreach (var property in logEvent.Properties)
            {
            sb.Append("\"");
            sb.Append(EscapeJsonString(property.Key));
            sb.Append("\":");
            sb.Append(JsonConvert.SerializeObject(property.Value.ToString(), Formatting.None));
            sb.Append(",");
        }

        // Remove the last comma if present
        if (sb[sb.Length - 1] == ',')
        {
            sb.Length--;
        }

        sb.Append("}");
        output.WriteLine(sb.ToString());
    }

    // Method to escape JSON strings correctly
    private string EscapeJsonString(string value)
    {
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
    }
}
