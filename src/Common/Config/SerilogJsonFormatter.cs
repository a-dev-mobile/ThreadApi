using Serilog.Events;
using Serilog.Formatting;
using System.IO;
using Newtonsoft.Json; // Ensure Newtonsoft.Json is included in your project dependencies

public class SerilogJsonFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        var jsonWriter = new JsonTextWriter(output) { Formatting = Formatting.None };
        
            jsonWriter.WriteStartObject();

        // Write the timestamp in the desired format
            jsonWriter.WritePropertyName("time");
            jsonWriter.WriteValue(logEvent.Timestamp.ToString("yyyy-MM-ddTHH:mm:sszzz"));

        // Write the level
            jsonWriter.WritePropertyName("level");
            jsonWriter.WriteValue(logEvent.Level.ToString());

            // Write the message
            jsonWriter.WritePropertyName("message");
        jsonWriter.WriteValue(logEvent.RenderMessage());

            // Write exception if exists
        if (logEvent.Exception != null)
        {
                jsonWriter.WritePropertyName("exception");
            jsonWriter.WriteValue(logEvent.Exception.ToString());
        }

        // Serialize and write additional properties
        foreach (var property in logEvent.Properties)
            {
                jsonWriter.WritePropertyName(property.Key);
            jsonWriter.WriteRawValue(JsonConvert.SerializeObject(property.Value)); // Serialize each value directly to JSON
        }

            jsonWriter.WriteEndObject();
        jsonWriter.Flush(); // Manually flush data to output, do not dispose JsonTextWriter

        output.WriteLine(); // Ensure there is a newline after each log entry
    }
}
