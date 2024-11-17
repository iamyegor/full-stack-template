#!/bin/bash

source ./dependencies/text_replacer.sh

install_sentry_on_backend() {
    local dsn=$1
    local kebab_name=$2
    local pascal_name=$3

    echo "Installing Sentry..."

    local main_settings="${kebab_name}-server/${pascal_name}/Api/appsettings.json"
    replace_in_file "$main_settings" '"Dsn": "FILL_ME_IN",' "\"Dsn\": \"$dsn\","

    local auth_settings="${kebab_name}-server/Auth/Api/appsettings.json"
    replace_in_file "$auth_settings" '"Dsn": "FILL_ME_IN",' "\"Dsn\": \"$dsn\","

    echo "Sentry installed successfully!"
}

install_posthog() {
    local posthog_key=$1
    local kebab_name=$2

    echo "Installing Posthog..."

    local env_file="$kebab_name-client/.env.local"

    if [ ! -f "$env_file" ]; then
        echo "NEXT_PUBLIC_POSTHOG_KEY=FILL_ME_IN" > "$env_file"
        echo "NEXT_PUBLIC_POSTHOG_HOST=https://eu.i.posthog.com" >> "$env_file"
    fi

    replace_in_file "$env_file" "NEXT_PUBLIC_POSTHOG_KEY=FILL_ME_IN" "NEXT_PUBLIC_POSTHOG_KEY=$posthog_key"

    echo "Posthog installed successfully!"
}