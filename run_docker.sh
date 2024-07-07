#!/bin/sh

DOCKER_IMAGE="thread-api:local"
DOCKER_CONTAINER="thread-api"

# Function to build the Docker image
build() {
    echo "Building Docker image..."
    docker build -t "$DOCKER_IMAGE" .
}

# Function to stop and remove the Docker container
remove() {
    echo "Stopping and removing Docker container..."
    docker stop "$DOCKER_CONTAINER" || true
    docker rm "$DOCKER_CONTAINER" || true
}

# Function to update the Docker image and restart the container
update() {
    echo "Building new Docker image..."
    build
    remove
    run
}

# Function to run the Docker container
run() {
    echo "Running Docker container..."
    docker run -d --name "$DOCKER_CONTAINER" -p 3003:8080 "$DOCKER_IMAGE"
}

# Check arguments
case "$1" in
    build)
        build
        ;;
    remove)
        remove
        ;;
    update)
        update
        ;;
    run)
        run
        ;;
    *)
        echo "Usage: $0 {build|remove|update|run}"
        exit 1
        ;;
esac
