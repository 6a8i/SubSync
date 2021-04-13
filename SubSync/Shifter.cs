using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubSync
{
    public class Shifter
    {
        async static public Task Shift(Stream input, Stream output, TimeSpan timeSpan, Encoding encoding, int bufferSize = 1024, bool leaveOpen = false)
        {
            // Always good to check if the object isn't null, then our code will not crash because of this.
            if (!input?.CanRead ?? false || output is null)
                return;

            // A regex pattern where it gets all the time frames in the file content seperated in three groups: timeframe, start and end.
            Regex regex = new Regex(@"(?<start>\d{1,2}:\d{1,2}:\d{1,2},\d{2,3}) --> (?<end>\d{1,2}:\d{1,2}:\d{1,2},\d{2,3})",
                RegexOptions.Compiled | RegexOptions.ECMAScript);

            // Initialize the writer and reader with the using clause, so they will be free when done.
            using StreamWriter swOutput = new StreamWriter(output, encoding, bufferSize, leaveOpen);
            using StreamReader srInput = new StreamReader(input);

            // Gets all the content from the file to the memory.
            var inputContent = await srInput.ReadToEndAsync();

            // Do the magic with the regex pattern and write back to the file, but not in the original file. It used another one.
            await swOutput.WriteAsync(
                regex.Replace(inputContent, delegate (Match m)
                {
                    var toReplace = m.Groups["0"].Value;
                    // Needed to format the string because in the assertion file doesn't use comma (,) but a dot (.) to split seconds from miliseconds.
                    string newValue = $"{TimeSpan.Parse(m.Groups["start"].Value.Replace(",", ".")).Add(timeSpan):hh\\:mm\\:ss\\.fff} --> {TimeSpan.Parse(m.Groups["end"].Value.Replace(",", ".")).Add(timeSpan):hh\\:mm\\:ss\\.fff}";

                    return m.Value.Replace(toReplace, newValue);
                })
            );
        }
    }
}
