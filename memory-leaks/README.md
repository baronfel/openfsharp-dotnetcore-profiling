# Diagnosing memory leaks

This sample illustrates how to diagnose a memory leak in an application.  The sample is derived from a situation I encountered at my job.  The scenario is that, while executing on the .Net Core 2.2 Runtime, the implementation of the `System.Diagnostics.EventListener` type appears to leak quite a bit of memory, up to ~1GB per process, seemingly unnecessarily.  In this sample we'll run the application, collect, and investigate dumps to find the root cause.

## Repository layout

This sample consists of a `.netcoreapp2.2` [Giraffe] application that has the [prometheus-net.DotNetRuntime] library installed to export metrics around the .Net Core runtime Garbage Collector, ThreadPool, and JIT.

It also contains two Dockerfiles for building and running the application, one for the 2.2 runtime and one for the 3.0 runtime. The intent is that you can capture dumps from both scenarios and investigate them.

## Capturing the dumps

First you'll need to build the application for the runtime you care to test: `./build.sh 2.2` or `./build.sh 3.0`
This will result in a named container: `memory-leak:2.2` or `memory-leak:3.0`.

Then, you'll run the application in its container: `./run.sh 2.2` or `./run.sh 3.0`.  These scripts will start the container with a particular name so that the following scripts can connect.

Finally, you'll capture a dump using `./dump.sh 2.2` or `./dump.sh 3.0`.  These helper scripts will connect to the container, take a memory dump, and copy it to the `dump` folder in this sample with the version and timestamp appended.  You should take repeated memory dumps a bit apart, since the only way to confirm a leak is to see the upward trend over time in memory allocated.

## Analyzing the dump

From here you can load the dump into [lldb] with SoS loaded (via `[dotnet-sos] install`), or directly into [dotnet-dump]. Once loaded, you are free to investigate.  I'd suggest using commands like 

* `dumpheap`:  get lists of the counts and sizes of instances of CLR types 
  * expect to see `System.String` and `system.Char[]` get pretty large!
  * narrow down to specific types (`-type <typename>` argument)
  * `-min` or `-max` to only return types that have instances at least that big in memory
* `dumpobj <object address>`: get details of an object instance
* `gcroot <object address>`: get

## End result

You should be able to see that there are some _very_ large strings, and if you're in [lldb] you should be able to use `memory read <object location>+0xc` to read the first few bytes of the string. You can add the `--count <count>` parameter to read even more bytes, and you should see that it's XML.  Specifically the XML from the .Net Runtime event source.  Read over and over again.

## Next steps

From here you've identified that the event listener is to blame, so you log/find [an issue](https://github.com/djluck/prometheus-net.DotNetRuntime/issues/6#). But until that's solved, you have tp make some choices

* if metrics are important, derive them some other way
* disable this metrics collection
* ???

[prometheus-net.DotNetRuntime](https://github.com/djluck/prometheus-net.DotNetRuntime)
[Giraffe](https://github.com/giraffe-fsharp/Giraffe)
[lldb]: https://lldb.llvm.org/
[dotnet-sos]: https://github.com/dotnet/diagnostics/blob/master/documentation/installing-sos-instructions.md
[dotnet-dump]: https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-dump-instructions.md
