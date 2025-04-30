#!/bin/sh

# Exit on failure
set -e

# Default directory (in case no --directory is provided)
DEFAULT_DIRECTORY="/tmp"

# Parse arguments
DIRECTORY="$DEFAULT_DIRECTORY"
for ARG in "$@"; do
  case $ARG in
    --directory=*)
      DIRECTORY="${ARG#*=}"
      ;;
  esac
done

# Ensure the directory exists
if [ ! -d "$DIRECTORY" ]; then
  echo "Directory '$DIRECTORY' does not exist. Creating it..."
  mkdir -p "$DIRECTORY"
fi

echo "Using directory: $DIRECTORY"

# Execute the program with the provided arguments
exec /tmp/codecrafters-build-http-server-csharp/codecrafters-http-server --directory "$DIRECTORY"