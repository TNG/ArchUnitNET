#!/bin/bash

# Generate the docs only if master, the travis_build_docs is true and we can use secure variables

chmod +x "$TRAVIS_BUILD_DIR/Travis/setup-ssh.sh" "$TRAVIS_BUILD_DIR./Travis/generate-documentation.sh"
source ./Travis/setup-ssh.sh || travis_terminate 1
./Travis/generate-documentation.sh || travis_terminate 1