
open System
open System.Text.Json
open System.Text.Json.Serialization

type MyRecord =
    { Name: string
      Active: bool
      CreatedDate: DateTimeOffset }

let sample = 
    { Name = "Sample" 
      Active = false
      CreatedDate = DateTimeOffset.UtcNow }

let options: JsonSerializerOptions = 
    let o = JsonSerializerOptions()
    o.Converters.Add(JsonRecordConverter())
    o

[<EntryPoint>]
let main _argv =
    let mutable counter = 0
    while true do
        let serialized = JsonSerializer.Serialize(sample, options)
        //let deserialized = JsonSerializer.Deserialize(serialized, options)
        
        if counter % 100_000 = 0 
        then ()
            // printfn "finished iteration %d" counter
            // printfn "%A" deserialized
        counter <- counter + 1
    0
