# Coding

## 1. Introduction

This section covers contributions involving coding for the project.

## 2. Branch Selection

When creating a pull request (PR) for this project, selecting the correct branch is crucial to ensure smooth integration without conflicts. Here's a detailed guide:

### 2.1. `stable-*` Branches

We recommend contributors work on a recent `stable-*` branch to ensure access to secure and stable code. These branches contain released versions with downloadable assets, providing a complete experience when building the project.

### 2.2. `main` Branch

While the `main` branch contains the latest code, it is advisable to avoid using it as your base. The code in `main` may be unstable, and certain assets may not have been publicly released yet. If you decide to work with `main`, be aware of these potential issues and consider `stable-*` branches as a more reliable alternative.

Differences between `stable-*` and `main` may exist, so always refer to the issues and stay updated on recent changes to understand the distinctions.

## 3. Versioning

We follow [Semantic Versioning (SemVer)](https://semver.org/). Prioritize PRs that involve `patch` or `minor` changes. Major changes should be discussed beforehand to be scheduled for the next major release. Mark any removal of public properties or methods as `Obsolete` in the latest release branch.

## 4. Code Style

You can find relevant information about the coding style used in the project by checking the [CODE_STYLE](./CODE_STYLE.md) file available in the repository.

## 5. Good Practices

It is essential for you as a developer to write efficient and functional code that adheres to the project's standards. For that reason, a dedicated document has been created with detailed information about restrictions and resource usage in the project. See [GOOD_PRACTICES](./GOOD_PRACTICES.md) for more details.
