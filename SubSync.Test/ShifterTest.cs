using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace SubSync.Test
{
    public class ShifterTest
    {
        [Fact]
        public async Task TestShiftSubtitle()
        {
            var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            var inputPath = Path.Combine(root, "The.Matrix.1999.BluRay.720p.Malay.srt");
            var outputPath = Path.Combine(root, "The.Matrix.1999.BluRay.720p.Malay - Copy.srt");
            var assertPath = Path.Combine(root, "The.Matrix.1999.BluRay.720p.Malay - Assert.srt");
            var timeSpan = TimeSpan.FromMilliseconds(123);
            var encoding = System.Text.Encoding.UTF8;

            using (var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
            using (var outputStream = new FileStream(outputPath, FileMode.Truncate, FileAccess.ReadWrite))
            {
                await Shifter.Shift(inputStream, outputStream, timeSpan, encoding);
            }

            using (var outputStream = new FileStream(outputPath, FileMode.Open, FileAccess.Read))
            using (var outputReader = new StreamReader(outputPath, encoding, true))
            using (var assertStream = new FileStream(assertPath, FileMode.Open, FileAccess.Read))
            using (var assertReader = new StreamReader(assertStream, encoding, true))
            {
                var assertLine = default(string);
                var outputLine = default(string);

                while (null != (assertLine = await assertReader.ReadLineAsync()))
                {
                    Assert.False(outputReader.EndOfStream);
                    outputLine = await outputReader.ReadLineAsync();
                    Assert.Equal(assertLine, outputLine);
                }
            }
        }
    }
}
