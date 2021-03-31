# Electron-auto-update

A NodeJS solution to store / retrieve installers

## Backend

Port: 4000

| Method | Endpoint                      | Description                                       |
|--------|-------------------------------|---------------------------------------------------|
| POST   | /core/:app/:platform          | Add an application for the corresponding platform |
| GET    | /core/:app/version            | Get all versions for this app                     |
| GET    | /core/:app/:platform/version  | Get the latest version for this app / platform    |
| GET    | /core/:app/:platform          | Download latest app for this platform             |
| GET    | /core/:app/:platform/:version | Download a specific version of this apps          |
| GET    | /core/                        | Get all apps                                      |
