namespace Fabulous

open System
open Fabulous

module Memo =

    type internal MemoData =
        {
          /// Captures data that memoization depends on
          KeyData: obj

          // comparer that remembers KeyType internally
          KeyComparer: obj -> obj -> bool

          /// wrapped untyped lambda that users provide
          CreateWidget: obj -> Widget

          /// Captures type of data that memoization depends on
          KeyType: Type

          /// Captures type of the marker memoized function produces
          MarkerType: Type }

    type Memoized<'t> = { phantom: 't }

    let private MemoAttributeKey = AttributeDefinitionStore.getNextKey ()
    let internal MemoWidgetKey = WidgetDefinitionStore.getNextKey ()

    let inline private getMemoData (widget: Widget) : MemoData =
        match widget.ScalarAttributes with
        | ValueSome attrs when attrs.Length = 1 -> attrs.[0].Value :?> MemoData
        | _ -> failwith "Memo widget cannot have extra attributes"

    let internal canReuseMemoizedWidget prev next =
        (getMemoData prev).MarkerType = (getMemoData next).MarkerType

    let private compareAttributes (prev: MemoData, next: MemoData) : ScalarAttributeComparison =
        match (prev.KeyType = next.KeyType, prev.MarkerType = next.MarkerType) with
        | true, true ->
            match next.KeyComparer next.KeyData prev.KeyData with
            | true -> ScalarAttributeComparison.Identical
            | false -> ScalarAttributeComparison.Different
        | _ -> ScalarAttributeComparison.Different

    let private updateNode (data: MemoData voption, node: IViewNode) : unit =
        match data with
        | ValueSome memoData ->
            let memoizedWidget = memoData.CreateWidget memoData.KeyData

            let prevWidget =
                match node.MemoizedWidget with
                | Some widget -> ValueSome(widget)
                | _ -> ValueNone

            node.MemoizedWidget <- Some memoizedWidget

            Reconciler.update node.TreeContext.CanReuseView prevWidget memoizedWidget node

        | ValueNone -> ()

    let internal MemoAttribute =
        { Key = MemoAttributeKey
          Name = "MemoAttribute"
          Convert = id
          ConvertValue = id
          Compare = compareAttributes
          UpdateNode = updateNode }

    AttributeDefinitionStore.set MemoAttributeKey MemoAttribute




    let private widgetDefinition: WidgetDefinition =
        { Key = MemoWidgetKey
          Name = "Memo"
          TargetType = Unchecked.defaultof<_> // Memo isn't allowed in lists, so this will never get called
          CreateView =
              fun (widget, context, parentNode) ->

                  let memoData = getMemoData widget

                  let memoizedWidget = memoData.CreateWidget memoData.KeyData

                  let memoizedDef =
                      WidgetDefinitionStore.get memoizedWidget.Key

                  let struct (node, view) =
                      memoizedDef.CreateView(memoizedWidget, context, parentNode)

                  // store widget that was used to produce this node
                  // to pass it to reconciler later on
                  node.MemoizedWidget <- Some memoizedWidget
                  struct (node, view) }

    WidgetDefinitionStore.set MemoWidgetKey widgetDefinition
