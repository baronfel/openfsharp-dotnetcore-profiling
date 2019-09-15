module MemoryLeak.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Prometheus

let webApp = GET >=> route "/" >=> setStatusCode 200 >=> text "Hi"

let configureApp (app : IApplicationBuilder) =
    app.UseHttpMetrics()
        .UseMetricServer()
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")

    let stats = DotNetRuntime.DotNetRuntimeStatsBuilder.Default().StartCollecting()


    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .Build()
        .RunAsync()
    |> Async.AwaitTask
    |> Async.RunSynchronously
    printfn "done"
    0