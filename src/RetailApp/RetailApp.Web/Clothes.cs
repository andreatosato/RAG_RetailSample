﻿using System.Text.Json.Serialization;

namespace Microsoft.KernelMemory;

public class Clothes
{
    public string Result { get; set; }
    public List<Citation> Citations { get; set; }
}


public class Citation
{
    /// <summary>
    /// Link to the source, if available.
    /// </summary>
    [JsonPropertyName("link")]
    [JsonPropertyOrder(1)]
    public string Link { get; set; } = string.Empty;

    /// <summary>
    /// Link to the source, if available.
    /// </summary>
    [JsonPropertyName("index")]
    [JsonPropertyOrder(2)]
    public string Index { get; set; } = string.Empty;

    /// <summary>
    /// Link to the source, if available.
    /// </summary>
    [JsonPropertyName("documentId")]
    [JsonPropertyOrder(3)]
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// Link to the source, if available.
    /// </summary>
    [JsonPropertyName("fileId")]
    [JsonPropertyOrder(4)]
    public string FileId { get; set; } = string.Empty;

    /// <summary>
    /// Type of source, e.g. PDF, Word, Chat, etc.
    /// </summary>
    [JsonPropertyName("sourceContentType")]
    [JsonPropertyOrder(5)]
    public string SourceContentType { get; set; } = string.Empty;

    /// <summary>
    /// Name of the source, e.g. file name.
    /// </summary>
    [JsonPropertyName("sourceName")]
    [JsonPropertyOrder(6)]
    public string SourceName { get; set; } = string.Empty;

#pragma warning disable CA1056
    /// <summary>
    /// URL of the source, used for web pages and external data
    /// </summary>
    [JsonPropertyName("sourceUrl")]
    [JsonPropertyOrder(7)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceUrl { get; set; } = null;
#pragma warning restore CA1056

}

