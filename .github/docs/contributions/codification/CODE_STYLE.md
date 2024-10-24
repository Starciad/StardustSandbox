# Code Style

We follow the [Microsoft C# coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions), with some exceptions.

## Main Preferences

1. Prefix `S` in file names and `IS` for interfaces;
1. Use `this` to reference fields and properties;
1. Avoid asynchronous code in inappropriate contexts.

## Other Preferences

1. Do not use `var`;
1. Mark immutable fields as `readonly`;
1. Use `internal` and `public` sparingly;
1. Utilize object initializers whenever possible.
1. All created constants must be strictly only in the constants directory present in some file.

## Inline `out` Declarations

Prefer inline form for `out` declarations: `SomeOutMethod(42, out PType value);`.

## Member Ordering

Always order members and properties:

1. Properties
1. Variable Fields
1. Events and Delegates
1. Constructors
1. Methods
