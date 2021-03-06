#!/bin/bash

origin=$(pwd)

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"


rm -rdf "$DIR/../back/build"
rm -rdf "$DIR/../front/build"


# Check if we are running on WSL (use Powershell increases performances a lot)
if [ "$(uname -r | sed -n 's/.*\( *Microsoft *\).*/\1/ip')" = 'microsoft' ]; then
  powershell.exe "cd ../front ; yarn build"
  powershell.exe "cd ../back ; yarn build"
else
  cd "$DIR/../front" && npm run build
  cd "$DIR/../back" && npm run build
fi

echo "Building for amd64"
"$DIR/amd/amd64.sh" &

echo "Building for arm64"
"$DIR/arm/arm64.sh" &

wait

cd $origin
