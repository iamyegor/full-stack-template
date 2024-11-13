# Use an official Node.js runtime as the base image
FROM node:20-alpine

WORKDIR /app

RUN npm install -g pnpm

COPY package.json pnpm-lock.yaml ./
RUN pnpm install --frozen-lockfile

COPY . .

# Build the Next.js application
RUN pnpm build

EXPOSE 3000

ENTRYPOINT ["pnpm", "start"]