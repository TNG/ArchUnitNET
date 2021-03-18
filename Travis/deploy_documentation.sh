#!/bin/bash

# Generate the docs only if master, the travis_build_docs is true and we can use secure variables

set -ev
declare -r SSH_FILE="$(mktemp -u $HOME/.ssh/travis_temp_ssh_key_XXXX)"

echo -e "$SSH_PRIVATE" > "$SSH_FILE"

# Enable SSH authentication
chmod 600 "$SSH_FILE" \
  && printf "%s\n" \
       "Host github.com" \
       "  IdentityFile $SSH_FILE" \
       "  LogLevel ERROR" >> ~/.ssh/config

ssh-keyscan github.com >> "$HOME/.ssh/known_hosts"

chmod +x "$TRAVIS_BUILD_DIR/Travis/generate_documentation.sh"
"$TRAVIS_BUILD_DIR/Travis/generate_documentation.sh" || travis_terminate 1