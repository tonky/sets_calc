# Sets Calculator

Write a sets calculator. It should calculate union, intersection and difference of sets of integers. Grammar of calculator is given:

```ebnf
set := file | expression
expression := “[“ operator set_1 set_2 set_3 … set_n “]”
operator := “SUM” | “INT” | “DIF”
```

Files contains sorted integers, one integer in a line. At least one set should be given to each operator. Operator:
SUM - returns union of all sets
INT - returns intersection of all sets
DIF - returns difference of first set and the rest ones

Solution should include building script. Calculator should print result to stdout.

Example:

```console
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

```

## Build instructions

publish --configuration Release -r linux-x64 --self-contained --nologo /p:PublishSingleFile=true /p:PublishTrimmed=true