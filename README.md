## Description
In order to overwrite the default template names, use setup.sh script.

This projects contains 5 pieces that need to work together:
1. Template ASP.NET Core application, which name can be overriden if you run setup.sh script.
2. Auth ASP.NET Core application
3. RabbitMQ message buss for the communication between the main application (template) and authentication.
4. NEXT.JS frontend
5. Directus CMS

## Running the project

- RabbitMQ and Directus are started together in the top level docker-compose.yaml file
- The remaining projects have to be started manually
- Backends use PostgreSQL database, connection string is located in appsettings.json in both projects

## When using the template

- Add Sentry to NEXT.JS
- Change Sentry DSN's in backend and Auth 
- Change google analytics id in CookieBanner.tsx
- Change umami script in layout/app.tsx