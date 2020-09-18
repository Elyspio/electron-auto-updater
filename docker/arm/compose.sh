#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

. "$DIR/variables.sh"

ssh $sshConnection "mkdir -p /apps/$app"
scp "$DIR/docker-compose.yml" "$sshConnection:/apps/$app/docker-compose.yml"
ssh $sshConnection "cd /apps/$app && docker-compose up -d"
