# Contributing

Contributions are very welcome. The following will provide some helpful guidelines.

## How to contribute

If you want to submit a contribution, please follow the following workflow:

* Fork the project
* Create a feature branch
* Add your contribution
* When you're completely done, build the project and run all tests.
* Create a Pull Request

### Commits

Commit messages should be clear and fully elaborate the context and the reason of a change.
If your commit refers to an issue, please post-fix it with the issue number, e.g.

```
Issue: #123
```

Furthermore, commits should be signed off according to the [DCO](DCO).

### Pull Requests

If your Pull Request resolves an issue, please add a respective line to the end, like

```
Resolves #123
```

## Tooling

This project uses [mise](https://mise.jdx.dev) to pin the .NET SDK and dev tools, and
[hk](https://hk.jdx.dev) to run formatting/lint checks. After installing mise, run:

```
mise install
mise run check
```

Formatting and autofixes can be applied using:

```
mise run check --fix
```
