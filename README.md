# CoronavirusKz

A console application that fetches and collects official daily statistics from posts of [@coronavirus2020_kz](https://t.me/s/coronavirus2020_kz) Telegram channel

## Build

```
cd src/
dotnet build
```

## Run

```
cd src/
dotnet run
```

### Run from post ID

```
dotnet run -- 5251
```

### Run action from post ID

```
dotnet run -- 5251 fetch
```
or
```
dotnet run -- fetch 5251
```

### List of supported actions:

- `fetch` - fetch and store post HTML page
- `extract` - parse post HTML page, get content of all posts, filter out posts by type of their content, store appropriate posts as HTML

## Tests

```
cd test/
dotnet test
```
or
```
cd test/
dotnet test --filter "test.ClassName.MethodName"
```
