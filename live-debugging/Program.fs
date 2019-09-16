open FSharpx.Collections
open Newtonsoft.Json
open Newtonsoft.Json.Linq


/// A converter that handles json arrays that contain at least one item and makes an
/// FSharpx.Collections.NonEmptyList from the non-empty array
type NonEmptyListConverter() =
  inherit JsonConverter()

  let openNonEmptyListType = typedefof<NonEmptyList<int>>
  let openResizeArrayType = typedefof<ResizeArray<_>>
  let nelAssy = openNonEmptyListType.Assembly
  let mutable counter = 0

  let ofSeqFn = nelAssy.GetType("FSharpx.Collections.NonEmptyList").GetMethod("OfSeq")

  override __.CanConvert t = t.IsGenericType && t.GetGenericTypeDefinition() = openNonEmptyListType

  override __.WriteJson(writer, nonEmptyList, serializer) =
    let innerType = nonEmptyList.GetType().GetGenericArguments().[0]
    let enumerator: System.Collections.IEnumerator = (nonEmptyList :?> System.Collections.IEnumerable).GetEnumerator()
    writer.WriteStartArray()
    while enumerator.MoveNext() do
      serializer.Serialize(writer, enumerator.Current, innerType)
    writer.WriteEndArray()
  
  
  // TODO: This method has a bug that causes the stackoverflow
  [<System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)>]
  override __.ReadJson(reader, destTy, _existingValue, serializer) =
    if reader.TokenType = JsonToken.StartArray
    then
      let innerTy = destTy.GenericTypeArguments.[0]
      let listTy = openResizeArrayType.MakeGenericType([| innerTy |])

      let list = listTy.GetConstructor([||]).Invoke([||])

      let add =
        let addM = listTy.GetMethod("Add")
        fun item -> addM.Invoke(list, [| item |]) |> ignore

      ignore <| reader.Read() // advance past start array
      while reader.TokenType <> JsonToken.EndArray do
        let v = serializer.Deserialize(reader, innerTy)
        //ignore (reader.Read()) //TODO: uncomment this to fix the algorithm
        add (box v)

      box (ofSeqFn.MakeGenericMethod([| innerTy |]).Invoke(null, [| list |]))
    else
      failwithf "Unknown start token %s while deserializing nonempty list" (string reader.TokenType)

let converter = NonEmptyListConverter() :> JsonConverter

[<EntryPoint>]
let main argv = 
    let nonEmptyList = NonEmptyList.ofList [1;2;3;4;5;6;7]
    printfn "serializing %A" nonEmptyList
    let serialized = JsonConvert.SerializeObject(nonEmptyList, [| converter |])
    printfn "serialized to %s" serialized
    let deserialized = JsonConvert.DeserializeObject<NonEmptyList<int>>(serialized, [| converter |])
    printfn "deserialized as %A" deserialized
    
    if nonEmptyList = deserialized
    then 0 
    else 1