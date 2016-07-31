---
layout: series_index
title: "Understanding Map and Apply and Bind"
seriesIndexId: "Map and Bind and Apply, Oh my!"
---

In this series of posts, I'll attempt to describe some of the core functions for dealing with generic data types (such as `Option` and `List`).
This is a follow-up post to [my talk on functional patterns](http://fsharpforfunandprofit.com/fppatterns/).

Yes, I know that [I promised not to do this kind of thing](../posts/why-i-wont-be-writing-a-monad-tutorial/index.md),
but for this post I thought I'd take a different approach from most people. Rather than talking about abstractions such as type classes,
I thought it might be useful to focus on the core functions themselves and how they are used in practice.

In other words, a sort of "man page" for `map`, `return`, `apply`, and `bind`.  

So, there is a section for each function, describing their name (and common aliases), common operators, their type signature,
and then a detailed description of why they are needed and how they are used, along with some visuals (which I always find helpful).  



* [Understanding map and apply](../posts/elevated-world.md). A toolset for working with elevated worlds.
* [Understanding bind](../posts/elevated-world-2.md). Or, how to compose world-crossing functions.
* [Using the core functions in practice](../posts/elevated-world-3.md). Working with independent and dependent data.
* [Understanding traverse and sequence](../posts/elevated-world-4.md). Mixing lists and elevated values.
* [Using map, apply, bind and sequence in practice](../posts/elevated-world-5.md). A real-world example that uses all the techniques.
* [Reinventing the Reader monad](../posts/elevated-world-6.md). Or, designing your own elevated world.
* [Map and Bind and Apply, a summary](../posts/elevated-world-7.md). .
