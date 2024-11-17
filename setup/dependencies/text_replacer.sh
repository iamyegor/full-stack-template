#!/bin/bash

replace_in_file() {
    local file=$1
    local search=$2
    local replace=$3
    
    if [[ "$OSTYPE" == "darwin"* ]]; then
        # macOS requires an empty string after -i
        sed -i '' "s|$search|$replace|g" "$file"
    else
        # Linux version
        sed -i "s|$search|$replace|g" "$file"
    fi
}