# SubSync
It is a very little .NET Standard 2.1 library that shifts the time frame from a srt file. It was used only for the purpose of learning and testing for a vacancy at Sindus Andritz. 

### Programming Language
C Sharp 8.0

### Plataform
.NET Standard 2.1

### How to Use
There is two ways you can use this:
  - You can call `Shifter.Shift(Stream input, Stream output, TimeSpan timeSpan, Encoding encoding, int bufferSize = 1024, bool leaveOpen = false)`.
  - Or you can use the test method to use and understand what the `Shifter.Shift` method does to change the times in the `.srt` file.
