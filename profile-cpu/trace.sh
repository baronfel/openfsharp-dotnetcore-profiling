#! /usr/bin/env sh

TRACE_CMD='dotnet trace collect -p 1 --format speedscope -o /trace/trace_$(date --iso-8601=''seconds'')'

docker exec -it profile-cpu sh -c "$TRACE_CMD"