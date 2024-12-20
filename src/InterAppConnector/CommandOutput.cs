﻿using System.Collections;
using System.Text.Json;
using InterAppConnector.DataModels;
using InterAppConnector.Enumerations;
using InterAppConnector.Exceptions;

namespace InterAppConnector
{
    /// <summary>
    /// Manage the output returned by the commands
    /// </summary>
    public class CommandOutput
    {
        /// <summary>
        /// Handles the various messages that can be returned by the application
        /// </summary>
        /// <param name="messageStatus">The message type</param>
        /// <param name="exitCode">The exit code</param>
        /// <param name="message">The raw object</param>
        public delegate void StatusEventHandler(CommandExecutionMessageType messageStatus, int exitCode, object message);

        public static event StatusEventHandler SuccessMessageEmitting;
        public static event StatusEventHandler InfoMessageEmitting;
        public static event StatusEventHandler WarningMessageEmitting;
        public static event StatusEventHandler ErrorMessageEmitting;

        internal static event StatusEventHandler SuccessMessageEmitted;
        internal static event StatusEventHandler InfoMessageEmitted;
        internal static event StatusEventHandler WarningMessageEmitted;
        internal static event StatusEventHandler ErrorMessageEmitted;

        /// <summary>
        /// Check if the method in event <paramref name="eventToAttach"/> is already added in <paramref name="targetEvent"/> invocation list.
        /// </summary>
        /// <param name="targetEvent">The target event</param>
        /// <param name="eventToAttach">The event to add</param>
        /// <returns><see langword="true"/> if the event is already attached, otherwise <see langword="false"/></returns>
        public static bool IsEventAlreadyAttached(Delegate targetEvent, Delegate eventToAttach)
        {
            bool isEventFound = false;
            if (targetEvent != null && eventToAttach != null)
            {
                /*
                 * It's important to compare the parameters used by targetEvent and eventToAdd.
                 * If the parameters and the name are the same, it is possible to say that 
                 * it is using the same delegate that is already added. The comparison between arrays should be done
                 * via IStructuralEquatable because this method check if the structure of the array is equal, and not
                 * the reference
                 */
                IStructuralEquatable eventToCompare = eventToAttach.Method.GetParameters();

                isEventFound = (from item in targetEvent.GetInvocationList()
                                where item.Method.Name == eventToAttach.Method.Name
                                && eventToCompare.Equals(item.Method.GetParameters(), StructuralComparisons.StructuralEqualityComparer)
                                select item).Any();
            }
            return isEventFound;
        }

        public static bool IsSuccessEventAlreadyAttached(Delegate eventToFind)
        {
            return IsEventAlreadyAttached(SuccessMessageEmitted, eventToFind);
        }

        public static bool IsWarningEventAlreadyAttached(Delegate eventToFind)
        {
            return IsEventAlreadyAttached(WarningMessageEmitted, eventToFind);
        }
        public static bool IsErrorEventAlreadyAttached(Delegate eventToFind)
        {
            return IsEventAlreadyAttached(ErrorMessageEmitted, eventToFind);
        }

        public static bool IsInfoEventAlreadyAttached(Delegate eventToFind)
        {
            return IsEventAlreadyAttached(InfoMessageEmitted, eventToFind);
        }

        internal static void ClearEvents()
        {
            SuccessMessageEmitted = null!;
            WarningMessageEmitted = null!;
            ErrorMessageEmitted = null!;
            InfoMessageEmitted = null!;
        }

        /// <summary>
        /// Return a positive message that an operation has been completed successfully
        /// </summary>
        /// <typeparam name="T">The message type</typeparam>
        /// <param name="message">The message to return</param>
        /// <param name="statusCode">The status code</param>
        /// <returns>The raw string</returns>
        public static string Ok<T>(T message, int statusCode = (int)CommandExecutionMessageType.Success - 1)
        {
            CommandResult<T> result = new CommandResult<T>();
            result.Message = message;
            result.MessageStatus = CommandExecutionMessageType.Success;
            Environment.ExitCode = statusCode;

            if (SuccessMessageEmitting != null)
            {
                SuccessMessageEmitting.Invoke(result.MessageStatus, statusCode, result);
            }
            
            if (SuccessMessageEmitted != null)
            {
                SuccessMessageEmitted.Invoke(result.MessageStatus, statusCode, result);
            }

            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Return an informative message regarding an operation
        /// </summary>
        /// <typeparam name="T">The message type</typeparam>
        /// <param name="message">The message to return</param>
        /// <param name="statusCode">The status code</param>
        /// <returns>The raw string</returns>
        public static string Info<T>(T message, int statusCode = (int)CommandExecutionMessageType.Info - 1)
        {
            CommandResult<T> result = new CommandResult<T>();
            result.Message = message;
            result.MessageStatus = CommandExecutionMessageType.Info;
            Environment.ExitCode = statusCode;

            if (InfoMessageEmitting != null)
            {
                InfoMessageEmitting.Invoke(result.MessageStatus, statusCode, result);
            }

            if (InfoMessageEmitted != null)
            {
                InfoMessageEmitted.Invoke(result.MessageStatus, statusCode, result);
            }

            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Return a warning about an operation
        /// </summary>
        /// <typeparam name="T">The message type</typeparam>
        /// <param name="message">The message to return</param>
        /// <param name="statusCode">The status code</param>
        /// <returns>The raw string</returns>
        public static string Warning<T>(T message, int statusCode = (int)CommandExecutionMessageType.Warning - 1)
        {
            CommandResult<T> result = new CommandResult<T>();
            result.Message = message;
            result.MessageStatus = CommandExecutionMessageType.Warning;
            Environment.ExitCode = statusCode;

            if (WarningMessageEmitting != null) 
            {
                WarningMessageEmitting.Invoke(result.MessageStatus, statusCode, result);
            }
            
            if (WarningMessageEmitted != null)
            {
                WarningMessageEmitted.Invoke(result.MessageStatus, statusCode, result);
            }

            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Return a negative message that an operation has been completed with an error
        /// </summary>
        /// <typeparam name="T">The message type</typeparam>
        /// <param name="message">The message to return</param>
        /// <param name="statusCode">The status code</param>
        /// <returns>The raw string</returns>
        public static string Error<T>(T message, int statusCode = (int)CommandExecutionMessageType.Failed - 1)
        {
            CommandResult<T> result = new CommandResult<T>();
            result.Message = message;
            result.MessageStatus = CommandExecutionMessageType.Failed;
            Environment.ExitCode = statusCode;

            if (ErrorMessageEmitting != null)
            {
                ErrorMessageEmitting.Invoke(result.MessageStatus, statusCode, result);
            }

            if (ErrorMessageEmitted != null)
            {
                ErrorMessageEmitted.Invoke(result.MessageStatus, statusCode, result);
            }

            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Extract only the <seealso cref="CommandResult{ResultType}.Message"/> property from a <seealso cref="CommandResult{ResultType}"/> 
        /// object without checking its type
        /// </summary>
        /// <param name="message"></param>
        /// <returns>a generic object taken from <seealso cref="CommandResult{ResultType}.Message"/> property</returns>
        /// <exception cref="TypeMismatchException">Exception raised when the object is not a <seealso cref="CommandResult{ResultType}"/> object</exception>
        /// <exception cref="ArgumentException">Exception raised when <paramref name="message"/> is <see langword="null" /> or the type is a type that does not have a generic type as parameter </exception>
        public static object ExtractMessageObject(object message)
        {
            object result;
            if (message != null)
            {
                Type objectType = message.GetType();
                Type[] genericArgumentType = objectType.GetGenericArguments();
                if (genericArgumentType.Length == 1)
                {
                    if (message.GetType() == (typeof(CommandResult<>).MakeGenericType(genericArgumentType[0])))
                    {
                        result = message.GetType().GetProperty("Message").GetValue(message);
                    }
                    else
                    {
                        throw new TypeMismatchException((typeof(CommandResult<>).MakeGenericType(genericArgumentType[0])).Name, message.GetType().Name, JsonSerializer.Serialize(message), "The expected type is " + (typeof(CommandResult<>).MakeGenericType(objectType)).Name + ", but the type of the object is " + message.GetType().Name);
                    }
                }
                else
                {
                    throw new ArgumentException("The object type must be " + (typeof(CommandResult<>).Name + " with a generic type set. The actual type does not have a generic parammeter"));
                }
            }
            else
            {
                throw new ArgumentException("The object must not be null");
            }
            return result;
        }

        /// <summary>
        /// Parse the raw string into an object that can be easily used in applications
        /// </summary>
        /// <typeparam name="T">The expected type of the string parsed</typeparam>
        /// <param name="messageToParse"> The message parsed</param>
        /// <returns>The parsed message</returns>
        /// <exception cref="TypeMismatchException">Raised when the type defined in </exception>
        /// <exception cref="MalformedMessageException"></exception>
        public static CommandResult<T> Parse<T>(string messageToParse)
        {
            CommandResult<T> result = null;
            try
            {
                CommandResult<T> parsedMessage = JsonSerializer.Deserialize<CommandResult<T>>(messageToParse);
                result = parsedMessage;
            }
            catch
            {
                JsonDocument document;
                try
                {
                    document = JsonDocument.Parse(messageToParse);
                }
                catch (Exception)
                {
                    throw new MalformedMessageException(messageToParse, "Error in message. The original message is " + messageToParse);
                }

                /*
                 * It's useless to check if document is set or not. At this point, document is always set. 
                 * Instead, focus on the property of the object
                 */
                JsonElement messageType;
                if (document.RootElement.TryGetProperty("MessageType", out messageType))
                {
                    if (typeof(T).FullName != messageType.GetString())
                    {
                        throw new TypeMismatchException(typeof(T).FullName, document.RootElement.GetProperty("MessageType").GetString(), messageToParse, "There is a type mismatch between declared type and message type. The expected type is " + typeof(T).FullName + " but the message type is " + document.RootElement.GetProperty("MessageType").GetString());
                    }
                    else
                    {
                        throw new MalformedMessageException(messageToParse, "Error in message. The original message is " + messageToParse);
                    }
                }
                else
                {
                    throw new MalformedMessageException(messageToParse, "The MessageType is not set The original message is " + messageToParse);
                }
            }
            return result;
        }
    }
}
