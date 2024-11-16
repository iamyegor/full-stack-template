#!/bin/bash

source text_replacer.sh

update_cicd() {
    update_cicd_config "$1" "$2"
    update_migration_paths "$1" "$2"
}

update_cicd_config() {
    local kebab_name=$1
    local pascal_name=$2

    mv .github/_workflows ".github/workflows"
    local cicd_file=".github/workflows/ci-cd.yaml"
    echo "Updating ${cicd_file}..."
    replace_in_file "$cicd_file" "FILL_ME_IN-k8s" "${kebab_name}-k8s"
    replace_in_file "$cicd_file" "DOCKER_REPO: FILL_ME_IN" "DOCKER_REPO: ${kebab_name}"
    replace_in_file "$cicd_file" "CLIENT_DIR: FILL_ME_IN" "CLIENT_DIR: ./${kebab_name}-client"
    replace_in_file "$cicd_file" "SERVER_DIR: FILL_ME_IN" "SERVER_DIR: ./${kebab_name}-server"
}

update_migration_paths() {
    local kebab_name=$1
    local pascal_name=$2
    local cicd_file=".github/workflows/ci-cd.yaml"

    # Auth paths
    replace_in_file "$cicd_file" "AUTH_MIGRATIONS_DIR: FILL_ME_IN" "AUTH_MIGRATIONS_DIR: ${kebab_name}-server/Auth/app/Infrastructure/Data/Migrations/*"
    replace_in_file "$cicd_file" "AUTH_MIGRATOR_MIGRATIONS_DIR: FILL_ME_IN" "AUTH_MIGRATOR_MIGRATIONS_DIR: ${kebab_name}-server/migrations/auth/Migrator/Migrations"
    replace_in_file "$cicd_file" "AUTH_MIGRATION_CHANGER: FILL_ME_IN" "AUTH_MIGRATION_CHANGER: ${kebab_name}-server/migrations/auth/MigrationChanger"
    replace_in_file "$cicd_file" "AUTH_APP_DIR: FILL_ME_IN" "AUTH_APP_DIR: ${kebab_name}-server/Auth"
    replace_in_file "$cicd_file" "AUTH_MIGRATOR_DIR: FILL_ME_IN" "AUTH_MIGRATOR_DIR: ${kebab_name}-server/migrations/auth"

    # Server paths
    replace_in_file "$cicd_file" "SERVER_MIGRATIONS_DIR: FILL_ME_IN" "SERVER_MIGRATIONS_DIR: ${kebab_name}-server/${pascal_name}/app/Infrastructure/Data/Migrations/*"
    replace_in_file "$cicd_file" "SERVER_MIGRATOR_MIGRATIONS_DIR: FILL_ME_IN" "SERVER_MIGRATOR_MIGRATIONS_DIR: ${kebab_name}-server/migrations/${kebab_name}/Migrator/Migrations"
    replace_in_file "$cicd_file" "SERVER_MIGRATION_CHANGER: FILL_ME_IN" "SERVER_MIGRATION_CHANGER: ${kebab_name}-server/migrations/${kebab_name}/MigrationChanger"
    replace_in_file "$cicd_file" "SERVER_APP_DIR: FILL_ME_IN" "SERVER_APP_DIR: ${kebab_name}-server/${pascal_name}"
    replace_in_file "$cicd_file" "SERVER_MIGRATOR_DIR: FILL_ME_IN" "SERVER_MIGRATOR_DIR: ${kebab_name}-server/migrations/${kebab_name}"
}

update_docker_compose() {
    local kebab_name=$1
    echo "Updating docker-compose.yaml..."
    # Add docker-compose update logic here
}

update_main_project_settings() {
    local kebab_name=$1
    local pascal_name=$2
    
    # Update Api/appsettings.json
    local main_settings="${kebab_name}-server/${pascal_name}/Api/appsettings.json"
    echo "Updating ${main_settings}..."
    replace_in_file "$main_settings" "template.server" "${kebab_name}.server"
    replace_in_file "$main_settings" "template.client" "${kebab_name}.client"

    # Update appsettings.Development.json
    local main_dev_settings="${kebab_name}-server/${pascal_name}/Api/appsettings.Development.json"
    echo "Updating ${main_dev_settings}..."
    replace_in_file "$main_dev_settings" "template_db" "${kebab_name}_db"

    # Update DependencyInjection.cs
    local main_di="${kebab_name}-server/${pascal_name}/Api/DependencyInjection.cs"
    echo "Updating ${main_di}..."
    replace_in_file "$main_di" "Template Error" "${pascal_name} Error"
}

update_auth_project_settings() {
    local kebab_name=$1
    local pascal_name=$2

    # Update Auth/Api/appsettings.json
    local auth_settings="${kebab_name}-server/Auth/Api/appsettings.json"
    echo "Updating ${auth_settings}..."
    replace_in_file "$auth_settings" "template.server" "${kebab_name}.server"
    replace_in_file "$auth_settings" "template.client" "${kebab_name}.client"

    # Update Auth/Api/appsettings.Development.json
    local auth_dev_settings="${kebab_name}-server/Auth/Api/appsettings.Development.json"
    echo "Updating ${auth_dev_settings}..."
    replace_in_file "$auth_dev_settings" "template_auth_db" "${kebab_name}_auth_db"

    # Update Auth DependencyInjection.cs
    local auth_di="${kebab_name}-server/Auth/Api/DependencyInjection.cs"
    echo "Updating ${auth_di}..."
    replace_in_file "$auth_di" "Template (Auth) Error" "${pascal_name} (Auth) Error"
}

update_dockerfiles() {
    local kebab_name=$1
    local pascal_name=$2

    # Update main Dockerfile
    local dockerfile="${kebab_name}-server/${pascal_name}/Dockerfile"
    echo "Updating ${dockerfile}..."
    replace_in_file "$dockerfile" 'COPY \["Template/Api/Api.csproj"' "COPY [\"${pascal_name}/Api/Api.csproj\""
    replace_in_file "$dockerfile" 'COPY \["Template/Application/Application.csproj"' "COPY [\"${pascal_name}/Application/Application.csproj\""
    replace_in_file "$dockerfile" 'COPY \["Template/Domain/Domain.csproj"' "COPY [\"${pascal_name}/Domain/Domain.csproj\""
    replace_in_file "$dockerfile" 'COPY \["Template/Infrastructure/Infrastructure.csproj"' "COPY [\"${pascal_name}/Infrastructure/Infrastructure.csproj\""
    replace_in_file "$dockerfile" 'COPY Template/' "COPY ${pascal_name}/"

    # Update migrations Dockerfile
    local migrations_dockerfile="${kebab_name}-server/migrators/${kebab_name}/Dockerfile"
    echo "Updating migrations ${migrations_dockerfile}..."
    replace_in_file "$migrations_dockerfile" 'COPY migrators/template/' "COPY migrators/${kebab_name}/"
    replace_in_file "$migrations_dockerfile" 'COPY \["migrators/template/' "COPY [\"migrators/${kebab_name}/"
    replace_in_file "$migrations_dockerfile" 'COPY \["Template/' "COPY [\"${pascal_name}/"
}

rename_directories() {
    local kebab_name=$1
    local pascal_name=$2

    # Rename template directories
    mv template-server "${kebab_name}-server"
    mv template-client "${kebab_name}-client"

    # Rename Template folder to project name
    mv "${kebab_name}-server/Template" "${kebab_name}-server/${pascal_name}"

    # Rename migrators template folder
    mv "${kebab_name}-server/migrators/template" "${kebab_name}-server/migrators/${kebab_name}"
}
