using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using sudoku_maker.Services;

namespace sudoku_maker.Localization;

public class LocExtension : MarkupExtension
{
    public string Key { get; set; }

    public LocExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new Binding(nameof(LocalizationService.CurrentLanguage))
        {
            Source = LocalizationService.Instance,
            Converter = new LocConverter(Key),
            Mode = BindingMode.OneWay
        };
    }
}

public class LocConverter : IValueConverter
{
    private readonly string _key;

    public LocConverter(string key)
    {
        _key = key;
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return LocalizationService.Instance.Get(_key);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}