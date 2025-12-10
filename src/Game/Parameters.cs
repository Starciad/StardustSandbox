using System;

namespace StardustSandbox
{
    internal static class Parameters
    {
        private enum ArgumentParameterType : byte
        {
            Flag = 0,
            String = 1,
            Int = 2,
            Bool = 3
        }

        private sealed class ArgumentDefinition(string name, string[] aliases, ArgumentParameterType parameterType, Action<object> callback)
        {
            internal string Name => name;
            internal string[] Aliases => aliases;
            internal ArgumentParameterType ParameterType => parameterType;
            internal Action<object> Callback => callback;
        }

        internal static bool SkipIntro { get; private set; }
        internal static bool ShowChunks { get; private set; }
        internal static bool CreateException { get; private set; }

        private static int position = 0;
        private static int length;
        private static string[] arguments;

        private static bool isInitialized = false;

        private static ArgumentDefinition[] argumentDefinitions;

        private static ReadOnlySpan<char> GetCurrentArgument()
        {
            return arguments[position];
        }

        private static bool MoveNext()
        {
            if (position + 1 < length)
            {
                position++;
                return true;
            }

            return false;
        }

        private static ArgumentDefinition FindArgumentDefinition(string key)
        {
            for (int i = 0; i < argumentDefinitions.Length; i++)
            {
                ArgumentDefinition def = argumentDefinitions[i];

                if (string.Equals(def.Name, key, StringComparison.OrdinalIgnoreCase))
                {
                    return def;
                }

                for (int j = 0; j < def.Aliases.Length; j++)
                {
                    if (string.Equals(def.Aliases[j], key, StringComparison.OrdinalIgnoreCase))
                    {
                        return def;
                    }
                }
            }

            return null;
        }

        private static void ProcessArgument(ReadOnlySpan<char> value)
        {
            string argKey = value.ToString();

            ArgumentDefinition definition = FindArgumentDefinition(argKey);
            if (definition is null)
            {
                return;
            }

            object parameter = null;

            if (definition.ParameterType != ArgumentParameterType.Flag)
            {
                if (!MoveNext())
                {
                    // Parameter expected but not found
                    return;
                }

                ReadOnlySpan<char> paramSpan = GetCurrentArgument();

                switch (definition.ParameterType)
                {
                    case ArgumentParameterType.String:
                        parameter = paramSpan.ToString();
                        break;

                    case ArgumentParameterType.Int:
                        if (int.TryParse(paramSpan, out int intValue))
                        {
                            parameter = intValue;
                        }

                        break;

                    case ArgumentParameterType.Bool:
                        if (bool.TryParse(paramSpan, out bool boolValue))
                        {
                            parameter = boolValue;
                        }

                        break;

                    default:
                        break;
                }
            }

            definition.Callback?.Invoke(parameter);
        }

        private static ArgumentDefinition CreateArgument(string name, string[] aliases, ArgumentParameterType parameterType, Action<object> callback)
        {
            return new ArgumentDefinition(name, aliases, parameterType, callback);
        }

        private static void RegisterArguments()
        {
            argumentDefinitions =
            [
                CreateArgument("--skip-intro", ["-si"], ArgumentParameterType.Flag,
                    (value) =>
                    {
                        SkipIntro = true;
                    }
                ),

                CreateArgument("--show-chunks", ["-sc"], ArgumentParameterType.Flag,
                    (value) =>
                    {
                        ShowChunks = true;
                    }
                ),

                CreateArgument("--create-exception", ["-ce"], ArgumentParameterType.Flag,
                    (value) =>
                    {
                        CreateException = true;
                    }
                )
            ];
        }

        private static void Initialize()
        {
            RegisterArguments();

            if (length <= 0)
            {
                return;
            }

            do
            {
                ProcessArgument(GetCurrentArgument());
            }
            while (MoveNext());
        }

        internal static void Start(string[] args)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(Parameters)} has already been initialized.");
            }

            arguments = args;
            length = args.Length;

            Initialize();

            isInitialized = true;
        }
    }
}
