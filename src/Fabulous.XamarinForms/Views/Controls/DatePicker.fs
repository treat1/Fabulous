namespace Fabulous.XamarinForms

open System.Runtime.CompilerServices
open Fabulous
open Xamarin.Forms

type IDatePicker =
    inherit IView

module DatePicker =
    let WidgetKey = Widgets.register<DatePicker> ()

    let CharacterSpacing =
        Attributes.defineBindable<float> DatePicker.CharacterSpacingProperty

    let Date =
        Attributes.defineBindable<System.DateTime> DatePicker.DateProperty

    let FontAttributes =
        Attributes.defineBindable<Xamarin.Forms.FontAttributes> DatePicker.FontAttributesProperty

    let FontFamily =
        Attributes.defineBindable<string> DatePicker.FontFamilyProperty

    let FontSize =
        Attributes.defineBindable<float> DatePicker.FontSizeProperty

    let Format =
        Attributes.defineBindable<string> DatePicker.FormatProperty

    let MaximumDate =
        Attributes.defineBindable<System.DateTime> DatePicker.MaximumDateProperty

    let MinimumDate =
        Attributes.defineBindable<System.DateTime> DatePicker.MinimumDateProperty

    let TextColor =
        Attributes.defineAppThemeBindable<Color> DatePicker.TextColorProperty

    let TextTransform =
        Attributes.defineBindable<Xamarin.Forms.TextTransform> DatePicker.TextTransformProperty

    let DateSelected =
        Attributes.defineEvent<DateChangedEventArgs>
            "DatePicker_DateSelected"
            (fun target -> (target :?> DatePicker).DateSelected)

[<AutoOpen>]
module DatePickerBuilders =
    type Fabulous.XamarinForms.View with
        static member inline DatePicker<'msg>(date: System.DateTime, onDateSelected: System.DateTime -> 'msg) =
            WidgetBuilder<'msg, IDatePicker>(
                DatePicker.WidgetKey,
                DatePicker.Date.WithValue(date),
                DatePicker.DateSelected.WithValue(fun args -> onDateSelected args.NewDate |> box)
            )

[<Extension>]
type DatePickerModifiers =
    [<Extension>]
    static member inline characterSpacing(this: WidgetBuilder<'msg, #IDatePicker>, value: float) =
        this.AddScalar(DatePicker.CharacterSpacing.WithValue(value))

    [<Extension>]
    static member inline font
        (
            this: WidgetBuilder<'msg, #IDatePicker>,
            ?size: double,
            ?namedSize: NamedSize,
            ?attributes: FontAttributes,
            ?fontFamily: string
        ) =

        let mutable res = this

        match size with
        | None -> ()
        | Some v -> res <- res.AddScalar(DatePicker.FontSize.WithValue(v))

        match namedSize with
        | None -> ()
        | Some v -> res <- res.AddScalar(DatePicker.FontSize.WithValue(Device.GetNamedSize(v, typeof<DatePicker>)))

        match attributes with
        | None -> ()
        | Some v -> res <- res.AddScalar(DatePicker.FontAttributes.WithValue(v))

        match fontFamily with
        | None -> ()
        | Some v -> res <- res.AddScalar(DatePicker.FontFamily.WithValue(v))

        res

    [<Extension>]
    static member inline format(this: WidgetBuilder<'msg, #IDatePicker>, value: string) =
        this.AddScalar(DatePicker.Format.WithValue(value))

    [<Extension>]
    static member inline minimumDate(this: WidgetBuilder<'msg, #IDatePicker>, value: System.DateTime) =
        this.AddScalar(DatePicker.MinimumDate.WithValue(value))

    [<Extension>]
    static member inline maximumDate(this: WidgetBuilder<'msg, #IDatePicker>, value: System.DateTime) =
        this.AddScalar(DatePicker.MaximumDate.WithValue(value))

    [<Extension>]
    static member inline textColor(this: WidgetBuilder<'msg, #IDatePicker>, light: Color, ?dark: Color) =
        this.AddScalar(DatePicker.TextColor.WithValue(AppTheme.create light dark))

    [<Extension>]
    static member inline textTransform(this: WidgetBuilder<'msg, #IDatePicker>, value: TextTransform) =
        this.AddScalar(DatePicker.TextTransform.WithValue(value))