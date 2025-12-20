# ArchUnitNET Development Guide

ArchUnitNET is a C# architecture testing library inspired by ArchUnit (Java). It analyzes .NET assemblies using Mono.Cecil to enforce architectural rules through a fluent API.

## Issue Tracking with bd (beads)

**IMPORTANT**: This project uses **bd (beads)** for ALL issue tracking. Do NOT use markdown TODOs, task lists, or other tracking methods if the `bd` command is available.

### Why bd?

- Dependency-aware: Track blockers and relationships between issues
- Git-friendly: Auto-syncs to JSONL for version control
- Agent-optimized: JSON output, ready work detection, discovered-from links
- Prevents duplicate tracking systems and confusion

### Quick Start

**Check for ready work:**

```bash
bd ready --json
```

**Create new issues:**

```bash
bd create "Issue title" -t bug|feature|task -p 0-4 --json
bd create "Issue title" -p 1 --deps discovered-from:bd-123 --json
bd create "Subtask" --parent <epic-id> --json  # Hierarchical subtask (gets ID like epic-id.1)
```

**Claim and update:**

```bash
bd update bd-42 --status in_progress --json
bd update bd-42 --priority 1 --json
```

**Complete work:**

```bash
bd close bd-42 --reason "Completed" --json
```

### Issue Types

- `bug` - Something broken
- `feature` - New functionality
- `task` - Work item (tests, docs, refactoring)
- `epic` - Large feature with subtasks
- `chore` - Maintenance (dependencies, tooling)

### Priorities

- `0` - Critical (security, data loss, broken builds)
- `1` - High (major features, important bugs)
- `2` - Medium (default, nice-to-have)
- `3` - Low (polish, optimization)
- `4` - Backlog (future ideas)

### Workflow for AI Agents

1. **Check ready work**: `bd ready` shows unblocked issues
2. **Claim your task**: `bd update <id> --status in_progress`
3. **Work on it**: Implement, test, document
4. **Discover new work?** Create linked issue:
   - `bd create "Found bug" -p 1 --deps discovered-from:<parent-id>`
5. **Complete**: `bd close <id> --reason "Done"`
6. **Commit together**: Always commit the `.beads/issues.jsonl` file together with the code changes so issue state stays in sync with code state

### Writing Self-Contained Issues

Issues must be fully self-contained - readable without any external context (plans, chat history, etc.). A future session should understand the issue completely from its description alone.

**Required elements:**

- **Summary**: What and why in 1-2 sentences
- **Files to modify**: Exact paths (with line numbers if relevant)
- **Implementation steps**: Numbered, specific actions
- **Example**: Show before → after transformation when applicable

**Optional but helpful:**

- Edge cases or gotchas to watch for
- Test references (point to test files or test_data examples)
- Dependencies on other issues

**Bad example:**

```
Implement the refactoring from the plan
```

**Good example:**

```
Add timeout parameter to fetchUser() in src/api/users.ts

1. Add optional timeout param (default 5000ms)
2. Pass to underlying fetch() call
3. Update tests in src/api/users.test.ts

Example: fetchUser(id) → fetchUser(id, { timeout: 3000 })
Depends on: bd-abc123 (fetch wrapper refactor)
```

### Dependencies: Think "Needs", Not "Before"

`bd dep add X Y` = "X needs Y" = Y blocks X

**TRAP**: Temporal words ("Phase 1", "before", "first") invert your thinking!

```
WRONG: "Phase 1 before Phase 2" → bd dep add phase1 phase2
RIGHT: "Phase 2 needs Phase 1" → bd dep add phase2 phase1
```

**Verify**: `bd blocked` - tasks blocked by prerequisites, not dependents.

### Auto-Sync

bd automatically syncs with git:

- Exports to `.beads/issues.jsonl` after changes (5s debounce)
- Imports from JSONL when newer (e.g., after `git pull`)
- No manual export/import needed!

### GitHub Copilot Integration

If using GitHub Copilot, also create `.github/copilot-instructions.md` for automatic instruction loading.

Run `bd onboard` to get the content, or see step 2 of the onboard instructions.

### MCP Server (Recommended)

If using Claude or MCP-compatible clients, install the beads MCP server:

```bash
pip install beads-mcp
```

Add to MCP config (e.g., `~/.config/claude/config.json`):

```json
{
  "beads": {
    "command": "beads-mcp",
    "args": []
  }
}
```

Then use `mcp__beads__*` functions instead of CLI commands.

### Managing AI-Generated Planning Documents

AI assistants often create planning and design documents during development:

- PLAN.md, IMPLEMENTATION.md, ARCHITECTURE.md
- DESIGN.md, CODEBASE_SUMMARY.md, INTEGRATION_PLAN.md
- TESTING_GUIDE.md, TECHNICAL_DESIGN.md, and similar files

**Best Practice: Use a dedicated directory for these ephemeral files**

**Recommended approach:**

- Create a `history/` directory in the project root
- Store ALL AI-generated planning/design docs in `history/`
- Keep the repository root clean and focused on permanent project files
- Only access `history/` when explicitly asked to review past planning

**Example .gitignore entry (optional):**

```
# AI planning documents (ephemeral)
history/
```

**Benefits:**

- ✅ Clean repository root
- ✅ Clear separation between ephemeral and permanent documentation
- ✅ Easy to exclude from version control if desired
- ✅ Preserves planning history for archeological research
- ✅ Reduces noise when browsing the project

### CLI Help

Run `bd <command> --help` to see all available flags for any command.
For example: `bd create --help` shows `--parent`, `--deps`, `--assignee`, etc.

### Important Rules

- ✅ Use bd for ALL task tracking
- ✅ Always use `--json` flag for programmatic use
- ✅ Link discovered work with `discovered-from` dependencies
- ✅ Check `bd ready` before asking "what should I work on?"
- ✅ Store AI planning docs in `history/` directory
- ✅ Run `bd <cmd> --help` to discover available flags
- ❌ Do NOT create markdown TODO lists
- ❌ Do NOT use external issue trackers
- ❌ Do NOT duplicate tracking systems
- ❌ Do NOT clutter repo root with planning documents

For more details, see README.md and QUICKSTART.md.

## Architecture Overview

The codebase follows a strict 4-layer architecture (enforced by `ArchUnitNETTests/ArchitectureTests/ArchUnitArchitectureTests.cs`):

1. **Domain** (`ArchUnitNET/Domain/`) - Core type model representing analyzed code

   - `Architecture`: Container for all loaded types, assemblies, namespaces
   - Type hierarchy: `IType` → `Class`, `Interface`, `Attribute`, `Struct`, `Enum`
   - Members: `MethodMember`, `FieldMember`, `PropertyMember`
   - Dependencies tracked via `IHasDependencies` (inheritance, calls, field access, attributes)
   - NO dependencies on Loader or Fluent layers

2. **Loader** (`ArchUnitNET/Loader/`) - Bytecode analysis using Mono.Cecil

   - `ArchLoader`: Entry point - loads assemblies via `LoadAssemblies()` or `LoadAssembliesIncludingDependencies()`
   - `ArchBuilder`: Orchestrates loading process, manages registries
   - `TypeFactory`: Converts Mono.Cecil types to Domain types (handles generics, arrays, nested types)
   - `LoadTaskRegistry`: Deferred task execution system - load type structure first, resolve dependencies later
   - Load tasks in `LoadTasks/`: `AddMembers`, `AddMethodDependencies`, `AddAttributesAndAttributeDependencies`, etc.
   - Extension methods in `MonoCecilTypeExtensions.cs`, `MonoCecilMemberExtensions.cs` bridge Cecil → Domain
   - NO dependencies on Fluent layer

3. **Fluent** (`ArchUnitNET/Fluent/`) - Rule definition DSL

   - Entry: `ArchRuleDefinition` static class - `Types()`, `Classes()`, `Interfaces()`, `Members()`, etc.
   - Three-phase builder pattern:
     - **Predicates** (`Fluent/Predicates/`): Filter objects (`That().ResideInNamespace()`, `That().ImplementInterface()`)
     - **Conditions** (`Fluent/Conditions/`): Assertions (`Should().NotDependOnAny()`, `Should().BePublic()`)
     - **Conjunctions**: Chain rules with `.And()`, `.Or()`, add descriptions with `.As()`, `.Because()`
   - `IObjectProvider<T>`: Lazy evaluation - predicates don't filter until `.Check()` or `.Evaluate()` called
   - `ObjectProviderCache`: Memoizes filtered results per `IObjectProvider` instance
   - Syntax elements in `Fluent/Syntax/Elements/` implement fluent interface chains
   - NO dependencies on Loader layer

4. **Test Framework Integrations** - Thin assertion wrappers
   - `ArchUnitNET.xUnit`, `ArchUnitNET.NUnit`, `ArchUnitNET.MSTestV2`, `ArchUnitNET.TUnit`, `ArchUnitNET.xUnitV3`
   - Each provides `ArchRuleAssert.CheckRule()` (NUnit/MSTest) or `FailedArchRuleException` (xUnit/TUnit)
   - Extensions: `ArchRuleExtensions.Check()` convenience method

## Working with Mono.Cecil

When extending the Loader layer:

- **Cecil types vs Domain types**: Cecil's `TypeDefinition`/`TypeReference` → Domain's `IType`; Cecil's `MethodDefinition` → Domain's `MethodMember`
- **Type resolution**: Use `TypeFactory.GetOrCreateTypeFromTypeReference()` to convert Cecil references to Domain types
- **Unavailable types**: When dependencies can't be resolved, `UnavailableType` instances are created (e.g., external assemblies not loaded)
- **Generic handling**: `TypeFactory` handles generic instances, generic parameters, arrays, function pointers
- **Load tasks**: Add new dependency tracking by creating `ILoadTask` implementations, registering in `ArchBuilder`
- **Extension methods**: Add Cecil helper methods to `MonoCecilTypeExtensions.cs` or `MonoCecilMemberExtensions.cs`

## Critical Development Workflows

### Testing

**ALWAYS run tests in Debug configuration:**

```bash
dotnet test -c Debug
```

Release builds omit debug symbols that ArchUnitNET needs to accurately analyze bytecode (see `documentation/docs/limitations/debug_artifacts.md`).

### Code Formatting

Format code with CSharpier before committing:

```bash
dotnet tool restore
dotnet csharpier .
```

Ignored files in `.csharpierignore` (snapshot test files with specific formatting).

### Snapshot Testing

Tests use Verify library (`ArchUnitNETTests/AssemblyTestHelper/AssemblyTestHelper.cs`):

- `AssertNoViolations()`: All objects pass
- `AssertAnyViolations()`: Some pass, some fail
- `AssertOnlyViolations()`: All objects fail
- Snapshots stored in `ArchUnitNETTests/**/Snapshots/`
- Update snapshots: Set `DiffEngine` environment variable or accept changes

### Static Architecture Pattern

Load architectures once per test class for performance (`ArchUnitNETTests/StaticTestArchitectures.cs`):

```csharp
private static readonly Architecture Architecture = new ArchLoader()
    .LoadAssemblies(typeof(MyClass).Assembly)
    .Build();
```

### Architecture Caching

`ArchitectureCache` (singleton) caches built architectures by assembly set - reuses across test runs if same assemblies loaded.

## Fluent API Patterns

### Adding Predicates (filter "That" clause)

1. Add method signature to interface in `Fluent/Syntax/Elements/IObjectPredicates.cs` or `Fluent/Syntax/Elements/Types/ITypePredicates.cs`
2. Implement in `ObjectPredicatesDefinition<T>` or `TypePredicatesDefinition<T>` - return `IPredicate<T>`
3. Wire up in `AddObjectPredicate<TRuleType, TNextElement>` or type-specific predicate classes

### Adding Conditions (assert "Should" clause)

1. Add method signature to interface in `Fluent/Syntax/Elements/IObjectConditions.cs` or `Fluent/Syntax/Elements/Types/ITypeConditions.cs`
2. Implement in `ObjectConditionsDefinition<T>` or `TypeConditionsDefinition<T>` - return `ICondition<T>` or `IOrderedCondition<T>`
3. Use `SimpleCondition<T>` for single-object checks, `ArchitectureCondition<T>` for cross-object analysis
4. Wire up in `ObjectsShould<TRuleTypeShouldConjunction, TRuleType>` or type-specific should classes

### Condition Ordering

`IOrderedCondition<T>` guarantees result order matches input order (critical for consistent error messages). Use `ConditionExtensions.AsOrderedCondition()` to wrap unordered conditions.

## Project Conventions

- **Multi-targeting**: netstandard2.0 + net462 - use lowest-common-denominator APIs
- **Annotations**: JetBrains.Annotations (`[NotNull]`, `[CanBeNull]`) for null reference analysis
- **Nullable context**: Disabled project-wide - use annotations instead
- **ReSharper settings**: `.DotSettings` files - respect formatting rules
- **Warnings as errors**: `TreatWarningsAsErrors=true` in `.csproj` files
- **Naming**: `I` prefix for interfaces, `_` prefix for private fields
- **Comments**: XML docs on public APIs, inline comments for complex logic

## Performance Considerations

- **Lazy evaluation**: Predicates/conditions don't execute until `.Check()` or `.Evaluate()`
- **Caching**: `ObjectProviderCache` memoizes predicate results - reuse `IObjectProvider` instances
- **Large collections**: `ConditionExtensions.OrderedConditionWrapper` uses dictionary (>256 objects) vs list lookup
- **Load tasks**: Deferred dependency resolution - structure loaded first, references resolved in batch
- **Architecture reuse**: Static `Architecture` instances avoid repeated assembly loading

## Example: Adding a New Predicate

To add `That().HaveVisibility(Visibility.Public)`:

1. Add to `ITypePredicates<TReturnType, TRuleType>`:

   ```csharp
   TReturnType HaveVisibility(Visibility visibility);
   ```

2. Implement in `TypePredicatesDefinition<T>`:

   ```csharp
   public static IPredicate<T> HaveVisibility(Visibility visibility)
   {
       return new SimplePredicate<T>(
           type => type.Visibility == visibility,
           $"have visibility {visibility}"
       );
   }
   ```

3. Wire up in predicate classes (auto-generated pattern from interface)

4. Test in `ArchUnitNETTests/Fluent/Syntax/Elements/TypeSyntaxElementsTests.cs`

## Common Pitfalls

- **Release vs Debug**: Tests fail in Release due to missing debug info - always use Debug
- **Cecil type confusion**: Don't expose `TypeDefinition` outside Loader - convert to Domain `IType`
- **Predicate reuse**: Don't create new `IObjectProvider` instances in loops - cache and reuse
- **Null handling**: Check `UnavailableType` when type resolution fails (external dependencies)
- **Layer violations**: Domain can't reference Loader/Fluent, Loader can't reference Fluent (enforced by tests)
