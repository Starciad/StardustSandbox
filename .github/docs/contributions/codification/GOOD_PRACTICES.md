# Good Practices

> [!NOTE]  
> The content listed here was adapted from the following article: <https://docs.monogame.net/articles/getting_started/preparing_for_consoles.html>

## 1. Introduction

This document addresses essential coding topics that should be followed to avoid long-term performance and stability issues. Read carefully and follow all the guidelines described here to ensure your game runs efficiently.

## 2. Restrictions

### 2.1. Avoid runtime reflection

Using reflection at runtime can be harmful to game performance and stability, and it increases garbage generation. Avoiding reflection ensures more stable and predictable execution, especially in production environments, where runtime optimizations are crucial.

Check your code to ensure that you are not using reflection or libraries that rely on it, as this could cause crashes or unexpected behavior in the game.

### 2.2. Avoid runtime compilation and `IL emit` usage

Runtime code generation, such as using `IL emit`, should be avoided. It adds complexity and can introduce hard-to-debug issues. Always try to pre-compile as much of your code as possible and avoid mechanisms that depend on dynamic code generation.

### 2.3. Third-party libraries

Many third-party libraries use reflection or dynamic code generation, which can harm game performance. Libraries dealing with parsing, such as JSON or XML, are particularly prone to this issue.

When selecting external libraries, prioritize those known for good performance optimization compatibility and that do not use reflection or dynamic generation. For instance, we choose lightweight libraries such as `TurboXML` and `TinyJSON` instead of heavier ones like `Newtonsoft JSON`.

Any new dependency addition should be pre-announced and submitted for discussion regarding its necessity and utility to the project.

### 2.4. Avoid system calls

Direct calls to the operating system, such as kernel commands or system-specific functions, should be avoided. These make the code less portable and can introduce compatibility issues.

## 3. Tips

### 3.1. Avoid LINQ

Although LINQ is a powerful and convenient tool in C#, its use can negatively affect game performance due to high garbage generation and lack of optimization in certain contexts. LINQ creates temporary collections and intermediate objects that directly impact garbage collection, which can cause performance drops, such as stuttering (small freezes).

### 3.2. Avoid unnecessary garbage generation

Even if the game performs well on PCs, that doesn’t guarantee efficient garbage collection in other environments. Games that generate a lot of dynamic allocations tend to suffer from performance drops, especially in high-intensity gameplay situations.

Here are some best practices to avoid excessive garbage generation:

- Use `const` for strings. Avoid dynamically creating strings during the game's main loop.
- Don’t dynamically allocate objects (`new`) during the game cycle. Pre-allocate whenever possible.
- Implement object pools. Instead of creating and destroying objects repeatedly (such as projectiles or particles), reuse them.
- Prefer data structures with sufficient initial capacity to avoid internal reallocations.
- Be cautious with `foreach`. In some scenarios, it can generate temporary allocations. Using `for` provides more control over this behavior.

### 3.3. Treat I/O as asynchronous

Input/output operations, such as saving player data or setting preferences, should be treated as asynchronous. Processes like saving data or accessing file information can cause stuttering if executed synchronously during the game’s main loop.

Therefore, it is recommended to move such operations to separate threads, ensuring the game loop continues smoothly without interruptions.

### 3.4. Thread Usage

The use of threads in a game must be carefully justified. While it may seem like an efficient solution for improving performance, excessive or improper use of threads can introduce a range of problems, such as race conditions, deadlocks, and debugging difficulties. Furthermore, synchronizing data between different threads can cause unnecessary overhead and increase code complexity.

In most cases, it's preferable for the game to run on a single thread. This simplifies flow control and avoids introducing complex errors. Operations that do not directly affect the main game loop, such as asset loading, data saving, or server communication, can be executed on separate threads but always with due caution.
