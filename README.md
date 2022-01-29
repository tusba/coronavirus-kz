# CoronavirusKz

A console crawler that fetches and collects official daily statistics from posts of [@coronavirus2020_kz](https://t.me/s/coronavirus2020_kz) Telegram channel

## Build

```bash
cd src/
dotnet build
```

## Run

```bash
cd src/
```

### Run fetch and extract for latest post, then parse obtained data

```bash
dotnet run
```

### Run fetch and extract from post ID, then parse obtained data

```bash
dotnet run -- 5251
```

### Run fetch or extract for latest post

```bash
dotnet run -- fetch
```

```bash
dotnet run -- extract
```

### Run fetch or extract from post ID

```bash
dotnet run -- 5251 fetch
```

or

```bash
dotnet run -- extract 5251
```

### Run parse for today

```bash
dotnet run -- parse
```

### Run parse for specific date

```bash
dotnet run -- parse 2021-12-05
```

### Run parse for specific period

```bash
dotnet run -- parse 2021-12-01 2021-12-05
```

### List of supported actions:

- `fetch [postId]` - fetch and store post HTML page
- `extract [postId]` - parse post HTML page, get content of all posts, filter out posts by type of their content, store appropriate posts as HTML
- `parse [date] [boundaryDate]` - parse HTML data from post statistics into a structured format

## Tests

```bash
cd test/
dotnet test
```

or

```bash
cd test/
dotnet test --filter "test.ClassName.MethodName"
```
