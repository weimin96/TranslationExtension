using System.Text.Json.Serialization;
using TranslationExtension;
using TranslationExtension.Providers;

namespace TranslationExtension;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(TranslationSettings))]
[JsonSerializable(typeof(DeepSeekRequest))]
[JsonSerializable(typeof(DeepSeekResponse))]
internal sealed partial class TranslationSettingsContext : JsonSerializerContext
{
}
