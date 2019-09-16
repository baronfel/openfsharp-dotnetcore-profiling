# Diagnosing incorrect code

This sample illustrates how to use [lldb] and [dotnet-sos] to inspect logic errors that happen in your application. The sample is derived from a situation I encountered while writing a custom `JsonConverter` for the `NonEmptyList` type from [FSharpx.Collections].  The scenario is that I was having trouble parsing a list of items due to a logic error I had introduced. In this sample we'll develop a similar `JsonConverter` and experience the same issue I saw, then debug it using [lldb].

## Repository layout

This sample consists of a `.netcoreapp3.0` sample application that has the [Newtonsoft.Json] and [FSharpx.Collections] libraries installed. While the program runs attempts to serialize and deserialize an instance of a `NonEmptyList`.

It also contains the Dockerfile for building and running the application. The intent is that you will be able to run the application in a container and run it under [lldb] to analyze the failure.

## Running the application

First you'll need to build the application: `./build.sh`.  This will result in a named container: `live-debug`.

Then, you'll run the application in its container: `./run.sh`.  This script will start the container, which runs the application under [lldb]'s active debugger.

Once the container launches, type `lldb LiveDebug` to load the program into [lldb]. Once [lldb] is loaded, type `run` to run the application. It should seemingly hang almost immediately, after which you can `CTRL+C` to halt execution and inspect the state of the program.

## Analyzing the application

From here you can inspect the stacktraces of the various threads to see if you can intuit what the erroneous callstack is.  You should use the following commands:

* `thread backtrace [all]`
  * dump the stack trace for the current thread (or all threads if `all` is provided)
* `thread select [thread_number]`
  * set the thread that's selected for various other commands
* `clrstack`
  * dump the managed stack trace for the selected CLR thread

## End result

After a bit of time, you should be able to find a stack containing our calls. This is our failing stack. The loop will in json reading, so we should inspect the `ReadJson` method of our converter where you will indeed find that the particular element of the array we're consuming isn't being 'advanced', so we always stay at the current position.

[lldb]: https://lldb.llvm.org/
[dotnet-sos]: https://github.com/dotnet/diagnostics/blob/master/documentation/installing-sos-instructions.md
[Newtonsoft.Json]: https://github.com/JamesNK/Newtonsoft.Json
[FSharpx.Collections]: https://github.com/fsprojects/FSharpx.Collections