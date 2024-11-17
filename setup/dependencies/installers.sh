#!/bin/bash

source text_replacer.sh

install_sentry() {
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
