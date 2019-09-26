#! /usr/bin/env sh

# note that we add the SYS_ADMIN capability here so that we can use `perf` if necessary
docker run --cap-add SYS_ADMIN --privileged -v "$(pwd)/trace":/trace --rm --name profile-cpu profile-cpu &