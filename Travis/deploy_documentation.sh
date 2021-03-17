#!/bin/bash

# Generate the docs only if master, the travis_build_docs is true and we can use secure variables

if [[ "$TRAVIS_BRANCH" = "master" && "$TRAVIS_PULL_REQUEST" = "false" ]] ; then
  chmod +x ./Travis/setup-ssh.sh ./Travis/generate-documentation.sh
  source ./Travis/setup-ssh.sh || travis_terminate 1
  ./Travis/generate-documentation.sh || travis_terminate 1
fi