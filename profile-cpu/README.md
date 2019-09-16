# Profiling CPU Usage

This sample illustrates how to visualize CPU profiling information to make changes to your application.  The sample is derived from a situation I encountered while investigating the [FSharp.SystemTextJson] serialization library for JSON.  The scenario is that, while adding Benchmark.Net tests showing comparisons between Newtonsoft.Json and Fsharp.SystemTextJson for record and union (de)serialization, FSharp.SystemTextJson showed much worse results in both speed and memory. In this sample we'll run a similar profile setup and learn how to generate flamegraphs to identify hot spots in your program.

## Repository layout

This sample consists of a `.netcoreapp3.0` sample application that has the [FSharp.SystemTextJson] library installed. While the program runs it continuously attempts to (de)serialize a JSON string for a record type into an instance of that record.

It also contains the Dockerfile for building and running the application. The intent is that you will be able to run the application in a container, grab a profile from the app and analyze it.

## Capturing the trace

First you'll need to build the application: `./build.sh`.  This will result in a named container: `profile-cpu`.

Then, you'll run the application in its container: `./start.sh`.  This script will start the container with a particular name so that the trace script can connect to it consistently.

Finally, you'll capture a profile trace using `./trace.sh`.  This helper script will connect to the container, take a cpu profile with [dotnet-trace] for a few seconds, and copy it to the `trace` folder in this folder with a timestamp appended.

## Analyzing the dump

From here you can load the dump into [speedscope] for visualization. Once loaded, you are free to investigate. What you should be able to see is that the majority of the time spent in the application is in the `FSharp.Reflection.FSharpType` helper functions for creating instances of records and reading values from instances of records.

## End result

For further reading you could look at the source code of the functions involved and see that they are very standard runtime reflection calls (PropertyInfo, MethodInfo, etc), which of course is quite slow.  There's not really an easy way to sidestep usage of these functions aside from using IL generation at runtime to compute optimized readers and writers, which of course the wonderful Tarmil is [doing already](https://github.com/Tarmil/FSharp.SystemTextJson/pull/15)

## Extra credit

The provided set up requires you to take a trace and relocate that trace to a system with a browser so that you can run the NodeJs-based tool [speedscope] for visualization.  This can be awkward if you want to do analysis on a remote server, so for that scenario it's also possible to use the `perf record` and `perf report` tools.  This will give you a terminal-based UI for tree analysis that's also quite powerful.  For more details check out the ones on the dotnet/diagnostics repository [here](https://github.com/dotnet/diagnostics/blob/master/documentation/tutorial/app_running_slow_highcpu.md)

[FSharp.SystemTextJson]: https://github.com/Tarmil/FSharp.SystemTextJson
[speedscope]: https://github.com/jlfwong/speedscope
[dotnet-trace]: https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-trace-instructions.md
