# GridColumnFor
Better columns for WebGrid

## Installation
1. Download WebGridExtensions.cs, or clone repo to your machine.
2. Place in your project directory and add to project in IDE of choice.

## Usage
### Defining the problem
`WebGrid` can be a great tool to set up tables of data in ASP.NET MVC. Unfortunately, its behavior is a little different 
than other tools and extensions in use in MVC. When customizing columns, the simplest syntax looks like this:

```cs
var grid = new WebGrid(model);
grid.Columns(
    grid.Column(columnName: "Address.StreetNumber", 
                header: @Html.DisplayNameFor(x => x.Address.StreetNumber)),
    grid.Column(columnName: "Address.StreetName", 
                header: @Html.DisplayNameFor(x => x.Address.StreetName)),
    grid.Column(columnName: "Address.StreetExt", 
                header: @Html.DisplayNameFor(x => x.Address.Ext))
);
```

That's a lot of duplication, and is not type-safe. Passing strings for column names means you won't get any compiler warnings
for typos or invalid properties. `WebGrid` also does not respect any `DisplayFormat` attributes you may have on your model, forcing 
you to write around these with additional lines of code.

### A Solution
`GridColumnFor` extends `HtmlHelper` for simplification and allows you to pass a single lambda expression instead, maintaining 
type-safety:

```cs
@using WebGridExtensions

var grid = new WebGrid(model);
grid.Columns(
    Html.GridColumnFor(x => x.Address.StreetNumber),
    Html.GridColumnFor(x => x.Address.StreetName),
    Html.GridColumnFor(x => x.Address.Ext)
);
```

It uses any `DisplayName` or `Display(Name=)` attributes on your model or metadata classes for header text. It respects any 
`DisplayFormat` attributes you have, say for formatting dates or currencies. You can also easily override the format:

```cs
grid.Columns(
    Html.GridColumnFor(x => x.Address.StreetNumber),
    Html.GridColumnFor(x => x.Address.StreetName, 
                       format: @<text><span class="bg-blue">@item.Address.StreetName.ToUpperInvariant()</span>),
    Html.GridColumnFor(x => x.Address.Ext)
);
```

## Explanations
**Why extend `HtmlHelper` instead of `WebGrid`?**  
`WebGrid` has no support for generics, which made it difficult to make the extension type-safe. `HtmlHelper`, however, does. 
It may be possible to do with `WebGrid`, but this was so much simpler.

**How much overhead will this add to my project?**  
Probably none. The entire extension file is less than 50 lines long, and that includes readability formatting and comments for 
Intellisense. The actual code inside the extension method could take up as few as four lines. It uses methods built into MVC and 
Linq.Expressions, which are likely already part of your project.
