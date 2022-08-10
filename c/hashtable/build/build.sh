#!/usr/bin/env sh

gcc -o dht ../src/main.c ../src/hashtable.c ../src/hash.c ../../prime/prime.c
./dht
