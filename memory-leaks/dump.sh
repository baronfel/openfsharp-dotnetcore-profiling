#! /usr/bin/env sh

if [ -z "$1" ]
then
    echo "usage: dump.sh [2.2|3.0]"
    exit 1
fi

TFM=$1

OLD_CMD='/usr/share/dotnet/shared/Microsoft.NETCore.App/2.2.4/createdump --full --name /dump/dump_$TFM_$(date --iso-8601=''seconds'') 1'
NEW_CMD='dotnet dump collect -p 1 -o /dump/dump_$TFM_$(date --iso-8601=''seconds'')'

if [ "2.2" = "$1" ]
then 
    CMD=$OLD_CMD
else
    CMD=$NEW_CMD
fi

# take a dump from the container and put it in the '/dump' folder with a timestamped name
docker exec memoryleak_"$TFM" sh -c "$CMD"