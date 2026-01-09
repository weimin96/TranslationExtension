using System.Text.Json.Serialization;
using TranslationExtension;
using TranslationExtension.Providers;

namespace TranslationExtension;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(TranslationSettings))]
[JsonSerializable(typeof(DeepSeekRequest))]
[JsonSerializable(typeof(DeepSeekResponse))]
[JsonSerializable(typeof(GlmRequest))]
[JsonSerializable(typeof(GlmResponse))]
[JsonSerializable(typeof(MinimaxRequest))]
[JsonSerializable(typeof(MinimaxResponse))]
[JsonSerializable(typeof(MinimaxContent))]
internal sealed partial class TranslationSettingsContext : JsonSerializerContext
{
}
