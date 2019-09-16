#! /usr/bin/env sh

docker run --cap-add SYS_ADMIN --cap-add SYS_PTRACE --security-opt seccomp=unconfined --security-opt apparmor=unconfined -it --rm --name live-debug live-debug /bin/bash