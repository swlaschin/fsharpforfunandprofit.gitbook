---
layout: series_index
title: "The 'dependency cycle' series"
seriesIndexId: "Dependency cycles"
---

One of the most common complaints about F# is that it requires code to be in *dependency order*. That is, you cannot use forward references to code that hasn't been seen by the compiler yet.  

In this series, I discuss dependency cycles, why they are bad, and how to get rid of them.


* [Cyclic dependencies are evil](../posts/cyclic-dependencies.md). Cyclic dependencies: Part 1.
* [Refactoring to remove cyclic dependencies](../posts/removing-cyclic-dependencies.md). Cyclic dependencies: Part 2.
* [Cycles and modularity in the wild](../posts/cycles-and-modularity-in-the-wild.md). Comparing some real-world metrics of C# and F# projects.
