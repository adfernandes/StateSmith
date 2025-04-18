#!/bin/bash

# This is a helper script that runs some test input into ANTLR4 grun.

# See README.md in this dir to help with install and setup.

# USAGE:
# - put the text that you want to parse into file `test_input.txt`
# - ensure to use setup.sh first (see that file for details)
# - uncomment one of the test commands below. Feel free to add new commands.
# - run this script like this: . test.sh

jarfile=antlr-4.9.2-complete.jar
libdir=./lib
jarpath=$libdir/$jarfile

export CLASSPATH=".:./out:./$libdir/$jarfile"
alias grun='java -Dfile.encoding=UTF-8 org.antlr.v4.gui.TestRig'

#########################################################
# Helpful commands for testing the StateSmith Grammar:
#########################################################
# grun StateSmithLabelGrammar node -gui -tokens -trace test_input.txt
grun StateSmithLabelGrammar node -tokens test_input.txt
# grun StateSmithLabelGrammar node -tokens -trace test_input.txt | grep "consume"
# grun StateSmithLabelGrammar node -gui -tokens -trace test_input.txt
# grun StateSmithLabelGrammar node -tokens -trace test_input.txt > /dev/null

#########################################################
# Helpful commands for testing the PlantUML Grammar:
#########################################################
## parses and shows Parse Tree Inspector GUI
# grun PlantUML diagram -gui -tokens -trace test_input.txt

## parses and shows which tokens are consumed
# grun PlantUML diagram -tokens -trace test_input.txt | grep "consume"

## if you are looking for an error, this will show only stderr
# grun PlantUML diagram -tokens -trace test_input.txt > /dev/null
