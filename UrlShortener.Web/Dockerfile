# BUILD
FROM node:20 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# RUN
FROM nginx:alpine
EXPOSE 80
COPY --from=build /app/dist /usr/share/nginx/html