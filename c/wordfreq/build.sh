#!/usr/bin/env sh

gcc -o wordfreq wordfreq.c ../hashtable/src/hashtable.c ../hashtable/src/hash.c ../prime/prime.c
./wordfreq -i input-l.txt 15
