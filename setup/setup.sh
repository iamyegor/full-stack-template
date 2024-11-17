#!/bin/bash

source ./dependencies/update_service.sh
source ./dependencies/validators.sh
source ./dependencies/installers.sh

cd ..

if [ -d ".git" ]; then
    echo "Removing existing .git directory..."
    rm -rf .git
fi

git init

read -p "Enter project name in PascalCase (e.g., NetIQ): " pascal_name
read -p "Enter project name in kebab-case (e.g., netiq): " kebab_name
read -p "Enter main microservice's Sentry DSN: " main_sentry_dsn
read -p "Enter auth microservice's Sentry DSN: " auth_sentry_dsn
read -p "Enter Posthog key: " posthog_key

validate_pascal_case "$pascal_name"
validate_kebab_case "$kebab_name"

rename_directories "$kebab_name" "$pascal_name"
update_cicd "$kebab_name" "$pascal_name"
update_docker_compose "$kebab_name"
update_main_project_settings "$kebab_name" "$pascal_name"
update_auth_project_settings "$kebab_name" "$pascal_name"
update_dockerfiles "$kebab_name" "$pascal_name"
rename_sln "$kebab_name" "$pascal_name"

install_sentry_on_backend "$main_sentry_dsn" "$auth_sentry_dsn" "$kebab_name" "$pascal_name"
install_posthog "$posthog_key" "$kebab_name"

echo "Project setup complete!"
echo "Project ${pascal_name} (${kebab_name}) has been configured successfully."
echo "Please review the changes and commit them to the new repository."