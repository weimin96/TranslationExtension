using System.Text.Json.Serialization;
using TranslationExtension;

namespace TranslationExtension;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(TranslationSettings))]
internal sealed partial class TranslationSettingsContext : JsonSerializerContext
{
}
