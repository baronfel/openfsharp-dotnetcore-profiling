#! /usr/bin/env sh

if [ -z "$1" ]
then
    echo "usage: start.sh [2.2|3.0]"
    exit 1
fi

TFM=$1

# start the app in the background with a particular name
docker run --cap-add SYS_ADMIN --cap-add SYS_PTRACE -v "$(pwd)/dump":/dump --rm -p=5000:80 --name memoryleak_"$TFM" memory-leak:"$TFM" &