using System;
using System.Text.Json.Serialization;

namespace TranslationExtension.Providers;

/// <summary>
/// GLM API 请求模型
/// </summary>
public class GlmRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = "glm-4-flash";

    [JsonPropertyName("messages")]
    public GlmMessage[] Messages { get; set; } = Array.Empty<GlmMessage>();

    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;
}

/// <summary>
/// GLM 消息模型
/// </summary>
public class GlmMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// GLM API 响应模型
/// </summary>
public class GlmResponse
{
    [JsonPropertyName("choices")]
    public GlmChoice[]? Choices { get; set; }

    [JsonPropertyName("error")]
    public GlmError? Error { get; set; }
}

/// <summary>
/// GLM 选择项模型
/// </summary>
public class GlmChoice
{
    [JsonPropertyName("message")]
    public GlmMessage? Message { get; set; }
}

/// <summary>
/// GLM 错误模型
/// </summary>
public class GlmError
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
