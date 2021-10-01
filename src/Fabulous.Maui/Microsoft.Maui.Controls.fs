﻿namespace Fabulous.Maui

open System.Runtime.CompilerServices
open Microsoft.Maui
open Microsoft.Maui.Graphics
open System.Collections.Generic
open Fabulous.Widgets.Controls
open Fabulous.Maui.Widgets
open Fabulous.Maui.Attributes

// MAUI CONTROLS

type ViewNode() =
    let mutable _attributes: Attribute[] = [||]

    interface IViewNode with
        member _.Attributes = _attributes
        member _.SetAttributes(attributes) =
            _attributes <- attributes

type FabulousApplication () =
    inherit ViewNode()

    let _windows = List<IWindow>()

    interface IApplication with
        member this.CreateWindow(activationState) = failwith "todo"
        member this.ThemeChanged() = Application.ThemeChanged.Execute(this, ())
        member this.Windows =
            match Application.Windows.TryGetValue(this) with
            | None -> ()
            | Some windowWidgets ->
                for widget in windowWidgets do
                    let view = unbox (widget.CreateView())
                    _windows.Add(view)

            _windows :> IReadOnlyList<_>
                
        
type FabulousWindow () =
    inherit ViewNode()

    let mutable _handler = Microsoft.Maui.Handlers.WindowHandler() :> IElementHandler

    interface IWindow with
        member this.Activated() = Window.Activated.Execute(this, ())
        member this.Created() = Window.Created.Execute(this, ())
        member this.Deactivated() = Window.Deactivated.Execute(this, ())
        member this.Destroying() = Window.Destroying.Execute(this, ())
        member this.Resumed() = Window.Resumed.Execute(this, ())
        member this.Stopped() = Window.Stopped.Execute(this, ())
        member this.Content = 
            match Window.Content.TryGetValue(this) with
            | None -> null
            | Some ValueNone -> null
            | Some (ValueSome widget) ->
                let view = unbox (widget.CreateView())
                view
        member this.Handler 
            with get() = _handler
            and set(v) = _handler <- v
        member this.Parent = failwith "todo"
        member this.Title = Window.Title.GetValue(this)
        
type FabulousVerticalStackLayout () =
    inherit ViewNode()

    let mutable _handler: IViewHandler = Microsoft.Maui.Handlers.LayoutHandler() :> IViewHandler
    let mutable _frame = Rectangle.Zero
    let mutable _desiredSize = Size.Zero
    let _children = List<IView>()

    interface IStackLayout with
        member this.Add(item: IView) = failwith "todo"
        member this.AnchorX = Transform.AnchorX.GetValue(this)
        member this.AnchorY = Transform.AnchorY.GetValue(this)
        member this.Arrange(bounds: Rectangle) =
            _frame <- Microsoft.Maui.Layouts.LayoutExtensions.ComputeFrame(this, bounds)
            if _handler <> null then _handler.NativeArrange(_frame)
            _frame.Size
        member this.AutomationId = View.AutomationId.GetValue(this)
        member this.Background = View.Background.GetValue(this)
        member this.Clear() = failwith "todo"
        member this.Clip = View.Clip.GetValue(this)
        member this.Contains(item: IView) = _children.Contains(item)
        member this.CopyTo(array: IView [], arrayIndex: int) = failwith "todo"
        member this.Count = _children.Count
        member this.DesiredSize = _desiredSize
        member this.FlowDirection = View.FlowDirection.GetValue(this)
        member this.Frame
            with get (): Rectangle = _frame
            and set (v: Rectangle): unit = _frame <- v
        member this.GetEnumerator() =
            _children.Clear()
            match Container.Children.TryGetValue(this) with
            | None -> ()
            | Some children ->
                for child in children do
                    let view = unbox (child.CreateView())
                    _children.Add(view)
        
            _children.GetEnumerator() :> System.Collections.Generic.IEnumerator<IView>

        member this.GetEnumerator() = _children.GetEnumerator() :> System.Collections.IEnumerator
        member this.Handler
            with get () = _handler :> IElementHandler
            and set (v: IElementHandler) = _handler <- (v :?> IViewHandler)
        member this.Handler
            with get () = _handler
            and set (v: IViewHandler) = _handler <- v
        member this.Height = View.Height.GetValue(this)
        member this.HorizontalLayoutAlignment = View.HorizontalLayoutAlignment.GetValue(this)
        member this.IgnoreSafeArea = SafeAreaView.IgnoreSafeArea.GetValue(this)
        member this.IndexOf(item: IView) = _children.IndexOf(item)
        member this.Insert(index: int, item: IView) = failwith "todo"
        member this.InvalidateArrange() = failwith "todo"
        member this.InvalidateMeasure() = failwith "todo"
        member this.IsEnabled = View.IsEnabled.GetValue(this)
        member this.IsReadOnly = true
        member this.Item
            with get (index: int): IView = _children.[index]
            and set (index: int) (v: IView): unit = failwith "todo"
        member this.LayoutManager = Microsoft.Maui.Layouts.VerticalStackLayoutManager(this) :> Microsoft.Maui.Layouts.ILayoutManager
        member this.Margin = View.Margin.GetValue(this)
        member this.Measure(widthConstraint: float, heightConstraint: float) =
            _desiredSize <- Microsoft.Maui.Layouts.LayoutExtensions.ComputeDesiredSize(this, widthConstraint, heightConstraint)
            _desiredSize
        member this.MinimumWidth = View.MinimumWidth.GetValue(this)
        member this.MinimumHeight = View.MinimumHeight.GetValue(this)
        member this.MaximumWidth = View.MaximumWidth.GetValue(this)
        member this.MaximumHeight = View.MaximumHeight.GetValue(this)
        member this.Opacity = View.Opacity.GetValue(this)
        member this.Padding = Layout.Padding.GetValue(this)
        member this.Parent: IElement = 
            raise (System.NotImplementedException())
        member this.Parent: IView = 
            raise (System.NotImplementedException())
        member this.Remove(item: IView) = failwith "todo"
        member this.RemoveAt(index: int) = failwith "todo"
        member this.Rotation = Transform.Rotation.GetValue(this)
        member this.RotationX = Transform.RotationX.GetValue(this)
        member this.RotationY = Transform.RotationY.GetValue(this)
        member this.Scale = Transform.Scale.GetValue(this)
        member this.ScaleX = Transform.ScaleX.GetValue(this)
        member this.ScaleY = Transform.ScaleY.GetValue(this)
        member this.Semantics = View.Semantics.GetValue(this)
        member this.Spacing = StackLayout.Spacing.GetValue(this)
        member this.TranslationX = Transform.TranslationX.GetValue(this)
        member this.TranslationY = Transform.TranslationY.GetValue(this)
        member this.VerticalLayoutAlignment = View.VerticalLayoutAlignment.GetValue(this)
        member this.Visibility = View.Visibility.GetValue(this)
        member this.Width = View.Width.GetValue(this)
        
type FabulousLabel () =
    inherit ViewNode()

    let mutable _handler = Microsoft.Maui.Handlers.LabelHandler() :> IViewHandler
    let mutable _frame = Rectangle.Zero
    let mutable _desiredSize = Size.Zero

    interface ILabel with
        member this.AnchorX = Transform.AnchorX.GetValue(this)
        member this.AnchorY = Transform.AnchorY.GetValue(this)
        member this.Arrange(bounds: Rectangle) =
            _frame <- Microsoft.Maui.Layouts.LayoutExtensions.ComputeFrame(this, bounds)
            if _handler <> null then _handler.NativeArrange(_frame)
            _frame.Size
        member this.AutomationId = View.AutomationId.GetValue(this)
        member this.Background = View.Background.GetValue(this)
        member this.CharacterSpacing = TextStyle.CharacterSpacing.GetValue(this)
        member this.Clip = View.Clip.GetValue(this)
        member this.DesiredSize = _desiredSize
        member this.FlowDirection = View.FlowDirection.GetValue(this)
        member this.Font = TextStyle.Font.GetValue(this)
        member this.Frame
            with get () = _frame
            and set (v: Rectangle): unit = _frame <- v
        member this.Handler
            with get () = _handler :> IElementHandler
            and set (v: IElementHandler): unit = _handler <- (v :?> IViewHandler)
        member this.Handler
            with get () = _handler
            and set (v: IViewHandler): unit = _handler <- v
        member this.Height = View.Height.GetValue(this)
        member this.HorizontalLayoutAlignment = View.HorizontalLayoutAlignment.GetValue(this)
        member this.HorizontalTextAlignment = TextAlignment.HorizontalTextAlignment.GetValue(this)
        member this.InvalidateArrange() = failwith "todo"
        member this.InvalidateMeasure() = failwith "todo"
        member this.IsEnabled = View.IsEnabled.GetValue(this)
        member this.LineBreakMode = Label.LineBreakMode.GetValue(this)
        member this.LineHeight = Label.LineHeight.GetValue(this)
        member this.Margin = View.Margin.GetValue(this)
        member this.MaxLines = Label.MaxLines.GetValue(this)
        member this.Measure(widthConstraint: float, heightConstraint: float) =
            _desiredSize <- Microsoft.Maui.Layouts.LayoutExtensions.ComputeDesiredSize(this, widthConstraint, heightConstraint)
            _desiredSize
        member this.MinimumWidth = View.MinimumWidth.GetValue(this)
        member this.MinimumHeight = View.MinimumHeight.GetValue(this)
        member this.MaximumWidth = View.MaximumWidth.GetValue(this)
        member this.MaximumHeight = View.MaximumHeight.GetValue(this)
        member this.Opacity = View.Opacity.GetValue(this)
        member this.Padding = Layout.Padding.GetValue(this)
        member this.Parent: IElement = 
            raise (System.NotImplementedException())
        member this.Parent: IView = 
            raise (System.NotImplementedException())
        member this.Rotation = Transform.Rotation.GetValue(this)
        member this.RotationX = Transform.RotationX.GetValue(this)
        member this.RotationY = Transform.RotationY.GetValue(this)
        member this.Scale = Transform.Scale.GetValue(this)
        member this.ScaleX = Transform.ScaleX.GetValue(this)
        member this.ScaleY = Transform.ScaleY.GetValue(this)
        member this.Semantics = View.Semantics.GetValue(this)
        member this.Text = Text.Text.GetValue(this)
        member this.TextColor = TextStyle.TextColor.GetValue(this)
        member this.TextDecorations = Label.TextDecorations.GetValue(this)
        member this.TranslationX = Transform.TranslationX.GetValue(this)
        member this.TranslationY = Transform.TranslationY.GetValue(this)
        member this.VerticalLayoutAlignment = View.VerticalLayoutAlignment.GetValue(this)
        member this.VerticalTextAlignment = TextAlignment.VerticalTextAlignment.GetValue(this)
        member this.Visibility = View.Visibility.GetValue(this)
        member this.Width = View.Width.GetValue(this)

type FabulousButton () =
    inherit ViewNode()

    let mutable _handler = Microsoft.Maui.Handlers.ButtonHandler() :> IViewHandler
    let mutable _frame = Rectangle.Zero
    let mutable _desiredSize = Size.Zero

    interface IButton with
        member this.AnchorX = Transform.AnchorX.GetValue(this)
        member this.AnchorY = Transform.AnchorY.GetValue(this)
        member this.Arrange(bounds: Rectangle) =
            _frame <- Microsoft.Maui.Layouts.LayoutExtensions.ComputeFrame(this, bounds)
            if _handler <> null then _handler.NativeArrange(_frame)
            _frame.Size
        member this.AutomationId = View.AutomationId.GetValue(this)
        member this.Background = View.Background.GetValue(this)
        member this.CharacterSpacing = TextStyle.CharacterSpacing.GetValue(this)
        member this.Clicked() = Button.Clicked.Execute(this, ())
        member this.Clip = View.Clip.GetValue(this)
        member this.DesiredSize = _desiredSize
        member this.FlowDirection = View.FlowDirection.GetValue(this)
        member this.Font = TextStyle.Font.GetValue(this)
        member this.Frame
            with get () = _frame
            and set (v: Rectangle): unit = _frame <- v
        member this.Handler
            with get () = _handler :> IElementHandler
            and set (v: IElementHandler): unit = _handler <- (v :?> IViewHandler)
        member this.Handler
            with get () = _handler
            and set (v: IViewHandler): unit = _handler <- v
        member this.Height = View.Height.GetValue(this)
        member this.HorizontalLayoutAlignment = View.HorizontalLayoutAlignment.GetValue(this)
        member this.ImageSource = Button.ImageSource.GetValue(this)
        member this.ImageSourceLoaded() = Button.ImageSourceLoaded.Execute(this, ())
        member this.InvalidateArrange() = failwith "todo"
        member this.InvalidateMeasure() = failwith "todo"
        member this.IsEnabled = View.IsEnabled.GetValue(this)
        member this.Margin = View.Margin.GetValue(this)
        member this.Measure(widthConstraint: float, heightConstraint: float) =
            _desiredSize <- Microsoft.Maui.Layouts.LayoutExtensions.ComputeDesiredSize(this, widthConstraint, heightConstraint)
            _desiredSize
        member this.MinimumWidth = View.MinimumWidth.GetValue(this)
        member this.MinimumHeight = View.MinimumHeight.GetValue(this)
        member this.MaximumWidth = View.MaximumWidth.GetValue(this)
        member this.MaximumHeight = View.MaximumHeight.GetValue(this)
        member this.Opacity = View.Opacity.GetValue(this)
        member this.Padding = Layout.Padding.GetValue(this)
        member this.Parent: IElement = 
            raise (System.NotImplementedException())
        member this.Parent: IView = 
            raise (System.NotImplementedException())
        member this.Pressed() = Button.Pressed.Execute(this, ())
        member this.Released() = Button.Released.Execute(this, ())
        member this.Rotation = Transform.Rotation.GetValue(this)
        member this.RotationX = Transform.RotationX.GetValue(this)
        member this.RotationY = Transform.RotationY.GetValue(this)
        member this.Scale = Transform.Scale.GetValue(this)
        member this.ScaleX = Transform.ScaleX.GetValue(this)
        member this.ScaleY = Transform.ScaleY.GetValue(this)
        member this.Semantics = View.Semantics.GetValue(this)
        member this.Text = Text.Text.GetValue(this)
        member this.TextColor = TextStyle.TextColor.GetValue(this)
        member this.TranslationX = Transform.TranslationX.GetValue(this)
        member this.TranslationY = Transform.TranslationY.GetValue(this)
        member this.VerticalLayoutAlignment = View.VerticalLayoutAlignment.GetValue(this)
        member this.Visibility = View.Visibility.GetValue(this)
        member this.Width = View.Width.GetValue(this)

// WIDGETS

type [<Struct>] ApplicationWidget<'msg> (attributes: Attribute[]) =
    static do ControlWidget.register<ApplicationWidget<'msg>, FabulousApplication>()

    static member inline Create(windows: seq<#IWindowWidget<'msg>>) =
        ApplicationWidget<'msg> ([| Application.Windows.WithValue(windows |> Seq.map (fun w -> w :> IWindowWidget)) |])
            
    interface IApplicationControlWidget<'msg> with
        member _.Attributes = attributes
        member _.CreateView() = ControlWidget.createView<FabulousApplication> attributes
        
type [<Struct>] WindowWidget<'msg> (attributes: Attribute[]) =
    static do ControlWidget.register<WindowWidget<'msg>, FabulousWindow>()
    
    static member inline Create(title: string, content: #IViewWidget<'msg>) =
        WindowWidget<'msg> (
            [| Window.Title.WithValue(title)
               Window.Content.WithValue(ValueSome (content :> IViewWidget)) |]
        )
        
    interface IWindowControlWidget<'msg> with
        member _.Attributes = attributes
        member _.CreateView() = ControlWidget.createView<FabulousWindow> attributes
        
type [<Struct>] VerticalStackLayoutWidget<'msg> (attributes: Attribute[]) =
    static do ControlWidget.register<VerticalStackLayoutWidget<'msg>, FabulousVerticalStackLayout>()
    
    static member inline Create(children: seq<IViewWidget<'msg>>) =        
        VerticalStackLayoutWidget<'msg> ([| Container.Children.WithValue(children |> Seq.map (fun c -> c :> IViewWidget)) |])
        
    interface IViewControlWidget<'msg> with
        member _.Attributes = attributes
        member _.CreateView() = ControlWidget.createView<FabulousVerticalStackLayout> attributes
        
type [<Struct>] LabelWidget<'msg> (attributes: Attribute[]) =
    static do ControlWidget.register<LabelWidget<'msg>, FabulousLabel>()
    
    static member inline Create(text: string) =
        LabelWidget<'msg> ([| Text.Text.WithValue(text) |])
        
    interface IViewControlWidget<'msg> with
        member _.Attributes = attributes
        member _.CreateView() = ControlWidget.createView<FabulousLabel> attributes
        
type [<Struct>] ButtonWidget<'msg> (attributes: Attribute[]) =
    static do ControlWidget.register<ButtonWidget<'msg>, FabulousButton>()
    
    static member inline Create(text: string, clicked: 'msg) =
        ButtonWidget<'msg>
            [| Text.Text.WithValue(text)
               Button.Clicked.WithValue(ignore) |]

    interface IViewControlWidget<'msg> with
        member _.Attributes = attributes
        member _.CreateView() = ControlWidget.createView<FabulousButton> attributes

// EXTENSIONS
    
[<Extension>] 
type IViewWidgetExtensions () =
    [<Extension>]
    static member inline background(this: #IViewControlWidget<'msg>, value: Paint) =
        this.AddAttribute(View.Background.WithValue(value))
    [<Extension>]
    static member inline font(this: #IViewControlWidget<'msg>, value: Font) =
        this.AddAttribute(TextStyle.Font.WithValue(value))
        
[<Extension>]
type LabelWidgetExtensions () =
    [<Extension>]
    static member inline textColor<'msg>(this: LabelWidget<'msg>, value: Color) =
        this.AddAttribute(TextStyle.TextColor.WithValue(value))
        
[<Extension>]
type VerticalStackLayoutExtensions () =
    [<Extension>]
    static member inline spacing<'msg>(this: VerticalStackLayoutWidget<'msg>, value: float) =
        this.AddAttribute(StackLayout.Spacing.WithValue(value))
        
// PUBLIC API

[<AbstractClass; Sealed>]
type View private () =
    static member inline Application<'msg>(windows) = ApplicationWidget<'msg>.Create(windows)
    static member inline Window<'msg>(title, content) = WindowWidget<'msg>.Create(title, content)
    static member inline VerticalStackLayout<'msg>(children) = VerticalStackLayoutWidget<'msg>.Create(children)
    static member inline Label<'msg>(text) = LabelWidget<'msg>.Create(text)
    static member inline Button<'msg>(text, clicked) = ButtonWidget<'msg>.Create(text, clicked)
    