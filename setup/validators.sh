#!/bin/bash

validate_pascal_case() {
    if [[ ! $1 =~ ^[A-Z][a-zA-Z0-9]+$ ]]; then
        echo "Error: Project name in PascalCase must start with uppercase and contain only letters and numbers"
        exit 1
    fi
}

validate_kebab_case() {
    if [[ ! $1 =~ ^[a-z][a-z0-9-]+$ ]]; then
        echo "Error: Project name in kebab-case must be lowercase, start with a letter, and contain only letters, numbers, and hyphens"
        exit 1
    fi
}