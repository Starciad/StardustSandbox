# Contributing to the Project

We are excited to receive your contributions. However, it is crucial to follow guidelines to ensure effective collaboration.

## Branch Selection

When creating a pull request (PR) in this project, choosing the correct branch is essential for smooth and conflict-free integration. Here's a detailed guide:

### `stable-*` Branches

We recommend contributors choose a recent `stable-*` branch to work with secure and stable code. These branches contain released versions with assets available for download, ensuring a complete experience when building the project.

### `main` Branch

While it contains the latest code, avoid basing on `main`. It may contain unstable code and assets not publicly released. If opting for `main`, be aware of these challenges and consider `stable-*` branches as a more stable alternative.

Differences between `stable-*` and `main` may exist, so refer to issues and keep track of recent changes to understand the differences.

## Versioning

We follow [Semantic Versioning (SemVer)](https://semver.org/). Prioritize PRs with `patch` or `minor` changes. Significant changes require prior discussion for integration in the next major release. Mark removals of public properties or methods as `Obsolete` in the latest release branch.

## Descriptive Titles

Use concise and descriptive titles in issues or PRs. Details should be in the description.

## Commit Messages

Follow [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/). Each commit should describe the change. Reference open issues with `#`.

**Examples of good commit messages:**

- `fix: Fix potential memory leak (#142).`;
- `feat: Add new entity (#169).`;
- `refactor: Refactor code for the 5th entity.`;
- `feat: Add new GUI component.`.

**Examples of incorrect messages:**

- `I'm bad.`;
- `Tit and tat.`;
- `Fixed.`;
- `Oops.`.

## Code Style

We follow the [Microsoft C# coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions), with some exceptions.

### Main Preferences

1. Prefix `P` in file names and `IP` for interfaces;
1. Use `this` to reference fields and properties;
1. Avoid asynchronous code in inappropriate contexts.

### Other Preferences

1. Do not use `var`;
1. Mark immutable fields as `readonly`;
1. Use `internal` and `public` sparingly;
1. Utilize object initializers whenever possible.

### Inline `out` Declarations

Prefer inline form for `out` declarations: `SomeOutMethod(42, out PType value);`.

### Member Ordering

Always order members and properties: Properties, `const` Fields, Variable Fields, Events and Delegates, Constructors, Methods.

## Pull Request

When contributing and creating a PR, provide clear and informative documentation. This increases the chances of integration.

## Code Review

Each PR is carefully reviewed. If rejected, there will be justification. Thank you for dedicating time and effort to Pixel Dust. Questions? Contact the team or community.
