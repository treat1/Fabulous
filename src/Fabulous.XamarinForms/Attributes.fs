namespace Fabulous.XamarinForms

open Fabulous
open Fabulous.Attributes
open Xamarin.Forms
open System

module XamarinFormsAttributeComparers =
    let widgetComparer (struct (prev: Widget, curr: Widget)) =
        AttributeComparison.Different (ValueSome (box curr))

module XamarinFormsAttributes =
    let defineWithConverter<'inputType, 'modelType> name defaultWith (convert: 'inputType -> 'modelType) (compare: struct ('modelType * 'modelType) -> AttributeComparison) (updateTarget: struct (ViewTreeContext * 'modelType voption * obj) -> unit) =
        let key = AttributeDefinitionStore.getNextKey()
        let definition =
            { Key = key
              Name = name
              DefaultWith = defaultWith
              Convert = convert
              Compare = compare
              UpdateTarget = updateTarget }
        AttributeDefinitionStore.set key definition
        definition

    let defineCollection<'elementType> name (updateTarget: struct (ViewTreeContext * 'elementType array voption * obj) -> unit) =
        defineWithConverter<'elementType seq, 'elementType array> name (fun () -> Array.empty) Seq.toArray AttributeComparers.collectionComparer updateTarget

    let defineWidgetCollection name updateTarget =
        defineCollection<Widget> name updateTarget

    let inline define<'T when 'T: equality> name defaultValue updateTarget =
        defineWithConverter<'T, 'T> name defaultValue id AttributeComparers.equalityComparer updateTarget

    let defineBindableWithComparer<'inputType, 'modelType> (bindableProperty: Xamarin.Forms.BindableProperty) (convert: 'inputType -> 'modelType) (comparer: struct ('modelType * 'modelType) -> AttributeComparison) =
        let key = AttributeDefinitionStore.getNextKey()
        let definition =
            { Key = key
              Name = bindableProperty.PropertyName
              DefaultWith = fun () -> Unchecked.defaultof<'modelType>
              Convert = convert
              Compare = comparer
              UpdateTarget = fun struct(_, newValueOpt, target) ->
                match newValueOpt with
                | ValueNone -> (target :?> BindableObject).ClearValue(bindableProperty)
                | ValueSome v -> (target :?> BindableObject).SetValue(bindableProperty, v) }
        AttributeDefinitionStore.set key definition
        definition

    let inline defineBindableWidget (bindableProperty: Xamarin.Forms.BindableProperty) =
        let key = AttributeDefinitionStore.getNextKey()
        let definition =
            { Key = key
              Name = bindableProperty.PropertyName
              DefaultWith = fun () -> Unchecked.defaultof<Widget>
              Convert = id
              Compare = XamarinFormsAttributeComparers.widgetComparer
              UpdateTarget = fun struct(context, newValueOpt, target) ->
                match newValueOpt with
                | ValueNone -> (target :?> BindableObject).ClearValue(bindableProperty)
                | ValueSome widget ->
                    let widgetDefinition = WidgetDefinitionStore.get widget.Key
                    let view = widgetDefinition.CreateView (widget, context)
                    (target :?> BindableObject).SetValue(bindableProperty, view) }
        AttributeDefinitionStore.set key definition
        definition

    let inline defineBindable<'T when 'T: equality> bindableProperty =
        defineBindableWithComparer<'T, 'T> bindableProperty id AttributeComparers.equalityComparer

    let defineEventNoArg name (getEvent: obj -> IEvent<EventHandler, EventArgs>) =
        let key = AttributeDefinitionStore.getNextKey()
        let definition =
            { Key = key
              Name = name
              DefaultWith = fun () -> null
              Convert = id
              Compare = AttributeComparers.noCompare
              UpdateTarget = fun struct (context, newValueOpt, target) ->
                let event = getEvent target
                let viewNodeData = (target :?> Xamarin.Forms.BindableObject).GetValue(ViewNode.ViewNodeProperty) :?> ViewNodeData

                match viewNodeData.TryGetHandler(key) with
                | None -> ()
                | Some handler -> event.RemoveHandler handler

                match newValueOpt with
                | ValueNone ->
                    viewNodeData.SetHandler(key, ValueNone)

                | ValueSome msg ->
                    let handler = EventHandler(fun _ _ ->
                        context.Dispatch msg
                    )
                    event.AddHandler handler
                    viewNodeData.SetHandler(key, ValueSome handler) }
        AttributeDefinitionStore.set key definition
        definition