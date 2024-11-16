#!/bin/bash

source update_service.sh
source validators.sh

cd ..

if [ -d ".git" ]; then
    echo "Removing existing .git directory..."
    rm -rf .git
fi

git init

read -p "Enter project name in PascalCase (e.g., NetIQ): " pascal_name
read -p "Enter project name in kebab-case (e.g., netiq): " kebab_name

validate_pascal_case "$pascal_name"
validate_kebab_case "$kebab_name"

rename_directories "$kebab_name" "$pascal_name"
update_cicd "$kebab_name" "$pascal_name"
update_docker_compose "$kebab_name"
update_main_project_settings "$kebab_name" "$pascal_name"
update_auth_project_settings "$kebab_name" "$pascal_name"
update_dockerfiles "$kebab_name" "$pascal_name"

echo "Project setup complete!"
echo "Project ${pascal_name} (${kebab_name}) has been configured successfully."
echo "Please review the changes and commit them to the new repository."