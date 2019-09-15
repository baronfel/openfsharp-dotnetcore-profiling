#! /usr/bin/env sh

if [ -z "$1" ]
then
    echo "usage: build.sh [2.2|3.0]"
    exit 1
fi

TFM=$1

# build the app in the docker container
docker build -t memory-leak:"$TFM" -f Dockerfile."$TFM" .
