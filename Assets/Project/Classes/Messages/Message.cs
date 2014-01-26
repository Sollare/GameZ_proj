using UnityEngine;
using System.Collections;

/// <summary>
/// Структура для хранения некоего сообщения
/// </summary>
public class Message  
{
    /// <summary>
    /// Тип сообщения
    /// </summary>
    public enum MessageType
    {
        Info,
        Warning,
        Error
    }

    public Message(string message, MessageType messageType, object data)
    {
        LocalizationMessage = message;
        Type = messageType;
        Data = data;
    }

    /// <summary>
    /// Содержимое сообщения
    /// </summary>
    public readonly object Data;

    /// <summary>
    /// Локализованное имя сообщения
    /// </summary>
    public readonly string LocalizationMessage;

    /// <summary>
    /// Тип сообщения
    /// </summary>
    public MessageType Type { get; private set; }
}
