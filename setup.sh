#!/bin/bash

# Function to validate PascalCase
validate_pascal_case() {
    if [[ ! $1 =~ ^[A-Z][a-zA-Z0-9]+$ ]]; then
        echo "Error: Project name in PascalCase must start with uppercase and contain only letters and numbers"
        exit 1
    fi
}

# Function to validate kebab-case
validate_kebab_case() {
    if [[ ! $1 =~ ^[a-z][a-z0-9-]+$ ]]; then
        echo "Error: Project name in kebab-case must be lowercase, start with a letter, and contain only letters, numbers, and hyphens"
        exit 1
    fi
}

# Function to replace text in file
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

# Check if .git exists and remove it
if [ -d ".git" ]; then
    echo "Removing existing .git directory..."
    rm -rf .git
fi

# Initialize new git repository
git init

# Get project names from user
read -p "Enter project name in PascalCase (e.g., NetIQ): " pascal_name
read -p "Enter project name in kebab-case (e.g., netiq): " kebab_name

# Validate input formats
validate_pascal_case "$pascal_name"
validate_kebab_case "$kebab_name"

# Rename template directories
mv template-server "${kebab_name}-server"
mv template-client "${kebab_name}-client"

# Update docker-compose.yaml
echo "Updating docker-compose.yaml..."
replace_in_file "${kebab_name}-server/docker-compose.yaml" "template-rabbitmq" "${kebab_name}-rabbitmq"

# Rename Template folder to project name
mv "${kebab_name}-server/Template" "${kebab_name}-server/${pascal_name}"

# Update Api/appsettings.json in main project
main_settings="${kebab_name}-server/${pascal_name}/Api/appsettings.json"
echo "Updating ${main_settings}..."
replace_in_file "$main_settings" "template.server" "${kebab_name}.server"
replace_in_file "$main_settings" "template.client" "${kebab_name}.client"
replace_in_file "$main_settings" "template-rabbitmq" "${kebab_name}-rabbitmq"

# Update appsettings.Development.json in main project
main_dev_settings="${kebab_name}-server/${pascal_name}/Api/appsettings.Development.json"
echo "Updating ${main_dev_settings}..."
replace_in_file "$main_dev_settings" "template_db" "${kebab_name}_db"

# Update DependencyInjection.cs in main project
main_di="${kebab_name}-server/${pascal_name}/Api/DependencyInjection.cs"
echo "Updating ${main_di}..."
replace_in_file "$main_di" "Template Error" "${pascal_name} Error"

# Update Auth/Api/appsettings.json
auth_settings="${kebab_name}-server/Auth/Api/appsettings.json"
echo "Updating ${auth_settings}..."
replace_in_file "$auth_settings" "template.server" "${kebab_name}.server"
replace_in_file "$auth_settings" "template.client" "${kebab_name}.client"
replace_in_file "$auth_settings" "template-rabbitmq" "${kebab_name}-rabbitmq"

# Update Auth/Api/appsettings.Development.json
auth_dev_settings="${kebab_name}-server/Auth/Api/appsettings.Development.json"
echo "Updating ${auth_dev_settings}..."
replace_in_file "$auth_dev_settings" "template_auth_db" "${kebab_name}_auth_db"

# Update Auth DependencyInjection.cs
auth_di="${kebab_name}-server/Auth/Api/DependencyInjection.cs"
echo "Updating ${auth_di}..."
replace_in_file "$auth_di" "Template (Auth) Error" "${pascal_name} (Auth) Error"

# Update Dockerfile COPY commands
dockerfile="${kebab_name}-server/${pascal_name}/Dockerfile"
echo "Updating ${dockerfile}..."
replace_in_file "$dockerfile" 'COPY \["Template/Api/Api.csproj"' "COPY [\"${pascal_name}/Api/Api.csproj\""
replace_in_file "$dockerfile" 'COPY \["Template/Application/Application.csproj"' "COPY [\"${pascal_name}/Application/Application.csproj\""
replace_in_file "$dockerfile" 'COPY \["Template/Domain/Domain.csproj"' "COPY [\"${pascal_name}/Domain/Domain.csproj\""
replace_in_file "$dockerfile" 'COPY \["Template/Infrastructure/Infrastructure.csproj"' "COPY [\"${pascal_name}/Infrastructure/Infrastructure.csproj\""
replace_in_file "$dockerfile" 'COPY Template/' "COPY ${pascal_name}/"

# Rename migrators template folder
mv "${kebab_name}-server/migrators/template" "${kebab_name}-server/migrators/${kebab_name}"

# Final message
echo "Project setup complete!"
echo "Project ${pascal_name} (${kebab_name}) has been configured successfully."
echo "Please review the changes and commit them to the new repository."