# Sets Calculator

Write a sets calculator. It should calculate union, intersection and difference of sets of integers. Grammar of calculator is given:

    set := file | expression
    expression := “[“ operator set_1 set_2 set_3 … set_n “]”
    operator := “SUM” | “INT” | “DIF”

Files contains sorted integers, one integer in a line. At least one set should be given to each operator. Operator:
SUM - returns union of all sets
INT - returns intersection of all sets
DIF - returns difference of first set and the rest ones

Solution should include building script. Calculator should print result to stdout.

Example:

    $ cat a.txt
    1
    2
    3

    $ cat b.txt
    2
    3
    4

    $ cat c.txt
    3
    4
    5

    ./scalc [ SUM [ DIF a.txt b.txt c.txt ] [ INT b.txt c.txt ] ]
    1
    3
    4

## Build instructions

Either run on native host, or run in container

### Running in container

    # docker build -t sets_calc .
    # docker run --rm sets_calc [ INT [ SUM b.txt c.txt ] a.txt ]
    2
    3

### Running on host

1. Download and install dotnet core 3.0 runtime or sdk: https://dotnet.microsoft.com/download/dotnet-core/3.0
2. Add `dotnet` executable to path

```shell
$ dotnet publish --configuration Release -r linux-x64 --self-contained --nologo -o bin /p:PublishSingleFile=true
Restore completed in 369.53 ms

$ bin/sets_calc [ SUM [ DIF a.txt b.txt c.txt ] [ INT b.txt c.txt ] ]
1
3
4
```

### Running tests

    dotnet test
